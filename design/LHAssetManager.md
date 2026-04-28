# LHAssetManager 設計書

LighthouseExtends に追加する Addressable アセット管理拡張の設計ドキュメント。

---

## 目的と解決する課題

Unity の Addressable アセットシステムは強力だが、ロード・アンロードのタイミング管理を手動で行う必要があり、参照カウントの管理漏れによるメモリリークや、逆に早期解放によるアセット消失が起きやすい。

`LHAssetManager` は以下を解決する：

- **参照カウント管理の自動化** — 同一アドレスを複数箇所からロードしても、内部的に1つの Addressables ハンドルを共有し、全員が手放したときだけ解放する
- **ライフタイムへの紐づけ** — シーン・ダイアログ・セッションといったライフタイム単位でアセットを束ね、そのライフタイムが終わったときに自動解放する
- **意図の明示** — アセットのロードはシーン・スクリーンのライフサイクルオーナーだけが行うと決めることで、どこで何が読まれているかを追いやすくする

---

## 対象ユースケース

スマートフォンゲームのように画面遷移が明確なゲームを主な対象とする。シーン遷移とアセットのライフタイムが一致するため、**ほぼ手動管理不要**になることを目標とする。

3D ゲームやオープンワールドで距離や視錐台に応じて大量のアセットを動的ストリーミングする用途は対象外。そういった用途には将来別途 `LHDynamicAssetManager` を設ける予定。

---

## インターフェース設計

### 全体構成

```
ILHAssetManager     グローバルシングルトン。Addressables ハンドルと参照カウントを一元管理。
    └── CreateScope() → ILHAssetScope

ILHAssetScope       アセットの所有単位。所有者が生成・破棄を明示的に管理する。
    └── LoadAsync<T>()      → T（ハンドルは内部追跡）
    └── LoadAssetAsync<T>() → IAssetHandle<T>（個別解放したい場合）
    └── Dispose()           → 束ねた全アセットを一括解放

IAssetHandle<T>     個別ハンドル。Dispose でそのアセットの参照カウントをデクリメント。
    └── Asset { get; }
    └── Dispose()
```

### ILHAssetManager

```csharp
public interface ILHAssetManager
{
    ILHAssetScope CreateScope();
}
```

`CreateScope()` だけを持つシンプルなファクトリー。内部では Addressables ハンドルの重複排除と参照カウントを管理する。

### ILHAssetScope

```csharp
public interface ILHAssetScope : IDisposable
{
    UniTask<T> LoadAsync<T>(string address, CancellationToken ct = default)
        where T : UnityEngine.Object;

    UniTask<IAssetHandle<T>> LoadAssetAsync<T>(string address, CancellationToken ct = default)
        where T : UnityEngine.Object;
}
```

**`LoadAsync<T>` が標準 API。** `T` だけを返し、`IAssetHandle` は呼び出し側に見せない。Scope が `Dispose` されたとき、この Scope を通じてロードした全アセットが一括解放される。

**`LoadAssetAsync<T>` は上級 API。** Scope の破棄より前に個別に解放したい場合に使う。返した `IAssetHandle` も Scope が内部追跡するため、明示的に `Dispose` しなくても Scope 破棄時に解放される（二重解放は内部でガード）。

### IAssetHandle\<T\>

```csharp
public interface IAssetHandle<out T> : IDisposable
{
    T Asset { get; }
}
```

`R3` の `CompositeDisposable` にそのまま追加できる。

---

## 「Scope」という命名について

当初 `ILHAssetManagerLayer` という名前を検討したが、"Layer" はスタック構造を連想させるため採用しなかった。この設計での Scope は**スタックではなく独立したバケツ**であり、複数の Scope が同時に存在して互いに影響しない。"Scope = 有効範囲" という一般的なプログラミング用語の意味に沿っている。

---

## 参照カウントと重複排除

同一アドレスを複数の Scope からロードしたとき：

```
ScopeA が "icon_a" をロード → 内部 refCount = 1、Addressables ハンドル生成
ScopeB が "icon_a" をロード → 内部 refCount = 2、同じハンドルを再利用
ScopeC が "icon_a" をロード → 内部 refCount = 3

ScopeA が Dispose される    → refCount = 2、ハンドル維持
ScopeB が Dispose される    → refCount = 1、ハンドル維持
ScopeC が Dispose される    → refCount = 0、Addressables.Release() 実行
```

Addressables のメモリ上のアセットは常に1つだけ存在する。

---

## アセットロードの責務分離

**アセットのロードはシーン・スクリーンのライフサイクルオーナーだけが行う。**

```
HomeScene.OnEnter()
    └── AssetScope.LoadAsync<Sprite>("banner")  ← ここだけがロードする

HomePresenter   → ロード済みの Sprite を受け取るだけ
HomeViewModel   → ロード済みの Sprite を受け取るだけ
```

この原則を設けた理由は2つある。

1. **ライフタイムの責務が明確になる。** `ILHAssetScope` はシーンの `OnLoad` / `OnUnload`（または ScreenStack の `OnInitialize` / `Dispose`）と対になっている。アセットのライフタイムは画面のライフタイムと一致するべきで、その管理は画面クラスが担うのが自然。

2. **どこで何が読まれているかを追いやすくなる。** ViewModel や Presenter がそれぞれアセットをロードし始めると、どのタイミングで何が生きているかを把握するのが難しくなる。一箇所に集約することでデバッグ・レビューが容易になる。

イベントバナーのような「ホーム画面に表示されるが、ホーム画面のライフタイムとは独立した要素」は、そのイベントシステム側が独自の `ILHAssetScope` を持って管理するべきであり、`HomeScene` の Scope に含めるべきではない。

---

## 各ユースケースの実装パターン

### ProductLifetimeScope（セッション共通アセット・プリロード相当）

```csharp
// ProductLifetimeScope.Configure() 内
builder.Register<ILHAssetScope>(
    resolver => resolver.Resolve<ILHAssetManager>().CreateScope(),
    Lifetime.Singleton); // このスコープの Singleton = スコープ破棄時に自動 Dispose
```

`CreateScope()` の呼び出しがコードに明示されているため、いつ・どこで Scope が生まれるかが分かりやすい。`Lifetime.Scoped` を使わない理由は、`Scoped` では生成タイミングと破棄タイミングがコードから読み取りにくく、指定ミスに気づきにくいため。

### シーン / モジュールシーン（ProductCanvasMainSceneBase）

```csharp
public abstract class ProductCanvasMainSceneBase<T> : CanvasMainSceneBase<T>
{
    [Inject] ILHAssetManager _assetManager;

    protected ILHAssetScope AssetScope { get; private set; }

    protected override UniTask OnLoad()
    {
        AssetScope = _assetManager.CreateScope();
        return UniTask.CompletedTask;
    }

    protected override UniTask OnUnload()
    {
        AssetScope?.Dispose();
        return UniTask.CompletedTask;
    }
}
```

継承クラスは `AssetScope.LoadAsync<T>()` を呼ぶだけ。`ILHAssetScope` を DI 注入しない理由は、ViewModel・Presenter にアセット管理を持ち込まないという原則を強制するため。

### ScreenStack（ProductScreenStackBase）

```csharp
public class ProductScreenStackBase : ScreenStackBase
{
    [Inject] ILHAssetManager _assetManager;

    protected ILHAssetScope AssetScope { get; private set; }

    public override UniTask OnInitialize()
    {
        AssetScope = _assetManager.CreateScope();
        return UniTask.CompletedTask;
    }

    public override void Dispose()
    {
        AssetScope?.Dispose(); // PlayOutAnimation → OnLeave の後に確実に呼ばれる
        base.Dispose();
    }
}
```

ScreenStack のダイアログは VContainer の LifetimeScope を持たない（`ScreenStackEntityFactory` が `objectResolver.Inject()` を使うため）。それでも `CreateScope()` により、ダイアログ単位でアセットの生成・解放が保証される。

---

## パッケージ依存関係

```
LighthouseExtends.Addressable
    依存: Lighthouse.Runtime、UniTask、VContainer、Unity.Addressables
    依存しない: LighthouseExtends.ScreenStack（依存方向を逆にしない）
```

`ProductScreenStackBase` への統合は SampleProduct 層（`ProductScreenStackBase.cs`）で行う。これは `ProductCanvasMainSceneBase` が InputLayer や Animation を統合しているのと同じパターン。

---

## 実装場所

- **開発**: `LighthouseSample/Client/Assets/LighthouseExtends.Addressable/`（asmdef を切って実装・検証）
- **移行先**: `Lighthouse/Client/Assets/LighthouseExtends/Addressable/`（完成後コピー、`package.json` 追加）

---

## 今後の検討事項

- **ScreenStack LifetimeScope 対応**: ダイアログが独自の VContainer LifetimeScope を持てるよう `ScreenStackEntityFactory` を拡張する。現状は `objectResolver.Inject()` ベースのため未対応。アセット管理目的では `CreateScope()` パターンで解決済みのため、DI グラフが必要になったタイミングで着手する。
