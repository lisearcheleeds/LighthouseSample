# PR レビュー: feature/input-layer → master

**レビュー日:** 2026-04-18  
**対象ブランチ:** `feature/input-layer` → `master`  
**レビュー観点:** 汎用性・拡張性、一貫性、エラーハンドリング、クリーンアーキテクチャ

---

## 評価サマリー

| 観点 | 評価 |
|---|---|
| 汎用性・拡張性 | ★★☆☆☆ (2/5) |
| 一貫性 | ★★★☆☆ (3/5) |
| エラーハンドリング | ★★☆☆☆ (2/5) |
| クリーンアーキテクチャ | ★★★☆☆ (3/5) |
| **総合** | **★★☆☆☆ (2/5)** |

---

## 重大な問題 (Blocker)

### [B-1] `IInputLayer` が `InputAction.CallbackContext` ではなく `InputAction` を受け取る

**ファイル:** `IInputLayer.cs:9-13`, `InputLayerController.cs:126,143,160`

これはフレームワーク全体で最も深刻な設計上の欠陥です。

```csharp
// 現状 - InputAction を渡している
bool OnActionStarted(InputAction action);

// InputLayerController 内部
var consumed = entry.Layer.OnActionStarted(ctx.action);  // ctx を捨てている
```

**問題点:**
- `InputAction.CallbackContext` はコールバック内でのみ有効な一時的な値型です。その中の `ReadValue<T>()`, `ReadValueAsObject()`, `interaction`, `phase` などの情報が全て失われます
- 例えば「押した値（Vector2）に応じて処理を変える」入力レイヤーが作れません
- `InputAction` を後で `ReadValue<T>()` しても、コールバック外では正しい値を返す保証がありません
- 汎用フレームワークとして致命的な制約です

**修正案:**
```csharp
// IInputLayer.cs
bool OnActionStarted(InputAction.CallbackContext context);
bool OnActionPerformed(InputAction.CallbackContext context);
bool OnActionCanceled(InputAction.CallbackContext context);
```

---

### [B-2] `SetGlobalLayer` に対応する削除・リセット手段がない

**ファイル:** `IInputLayerController.cs`, `InputLayerController.cs:60-65`

```csharp
// 現状
public void SetGlobalLayer(IInputLayer layer, InputActionMap actionMap)
{
    globalActionMap = actionMap;   // 旧 actionMap は Disable されない
    globalLayer = layer;
    actionMap.Enable();            // 新しい actionMap だけ Enable
}
```

**問題点:**
- `SetGlobalLayer` を2回呼ぶと旧 `globalActionMap` が `Disable` されずリークします
- `RemoveGlobalLayer` / `ClearGlobalLayer` が存在しないため、グローバルレイヤーを除去できません
- シーン遷移時にグローバルレイヤーをリセットしたいケースで詰まります

**修正案:**
```csharp
// IInputLayerController.cs に追加
void RemoveGlobalLayer();

// InputLayerController.cs
public void SetGlobalLayer(IInputLayer layer, InputActionMap actionMap)
{
    globalActionMap?.Disable();   // 旧マップを無効化
    globalActionMap = actionMap;
    globalLayer = layer;
    actionMap?.Enable();
}

public void RemoveGlobalLayer()
{
    globalActionMap?.Disable();
    globalActionMap = null;
    globalLayer = null;
}
```

---

### [B-3] `IInputLayerController` が `IDisposable` を公開していない

**ファイル:** `IInputLayerController.cs`, `InputLayerController.cs:47`

`InputLayerController` は `IDisposable` を実装しているのに、インターフェースには宣言がありません。

```csharp
// 現状 - インターフェース経由で Dispose できない
public class InputLayerController : IInputLayerController, IDisposable  // OK
public interface IInputLayerController { ... }  // IDisposable なし！
```

DIコンテナ（VContainer）はインターフェースを通じて解決するため、`IDisposable` が漏れるとライフサイクル管理が破綻します。

**修正案:**
```csharp
public interface IInputLayerController : IDisposable
{
    void SetGlobalLayer(IInputLayer layer, InputActionMap actionMap);
    void RemoveGlobalLayer();
    void PushLayer(IInputLayer layer, InputActionMap actionMap);
    void PopLayer();
    void PopLayer(IInputLayer target);
}
```

---

## 重要な問題 (Major)

### [M-1] レイヤーコールバック内の例外が伝播を破壊する

**ファイル:** `InputLayerController.cs:124-131`

```csharp
foreach (var entry in reversedStack)
{
    var consumed = entry.Layer.OnActionStarted(ctx.action);  // ここで例外が出ると
    if (consumed || entry.Layer.BlocksAllInput)              // 以降のレイヤーに届かない
    {
        break;
    }
}
```

**問題点:** あるレイヤーで例外が発生すると、その下のレイヤーへの入力が止まります。汎用フレームワークでは各レイヤーの実装品質に依存しない堅牢性が必要です。

**修正案:**
```csharp
foreach (var entry in reversedStack)
{
    bool consumed;
    try
    {
        consumed = entry.Layer.OnActionStarted(ctx.action);
    }
    catch (Exception e)
    {
        Debug.LogException(e);
        consumed = false;
    }
    if (consumed || entry.Layer.BlocksAllInput)
    {
        break;
    }
}
```

---

### [M-2] 引数の null ガードが全くない

**ファイル:** `InputLayerController.cs:60,67,97`

```csharp
public void SetGlobalLayer(IInputLayer layer, InputActionMap actionMap)
{
    // actionMap が null なら actionMap.Enable() でクラッシュ
    actionMap.Enable();
}

public void PushLayer(IInputLayer layer, InputActionMap actionMap)
{
    // layer が null でも Insert される → コールバック時に NRE
    reversedStack.Insert(0, new LayerEntry(layer, actionMap));
}
```

**修正案:**
```csharp
public void PushLayer(IInputLayer layer, InputActionMap actionMap)
{
    if (layer == null) throw new ArgumentNullException(nameof(layer));
    if (actionMap == null) throw new ArgumentNullException(nameof(actionMap));
    ...
}
```

---

### [M-3] `Debug.Log` がリリースビルドに出力される

**ファイル:** `InputLayerController.cs:76,94,113`

```csharp
Debug.Log($"[InputLayer] Push: {StackToString()}");  // リリースにも出る
```

不特定多数のプロジェクトに使われる基盤ライブラリがデフォルトでログを垂れ流すのは許容できません。

**修正案（2択）:**
```csharp
// 案A: UNITY_EDITOR のみ
#if UNITY_EDITOR
Debug.Log($"[InputLayer] Push: {StackToString()}");
#endif

// 案B: 開発ビルドも含める
#if DEVELOPMENT_BUILD || UNITY_EDITOR
Debug.Log($"[InputLayer] Push: {StackToString()}");
#endif
```

---

### [M-4] `PopLayer()` / `PopLayer(target)` が `globalActionMap` を考慮しない

**ファイル:** `InputLayerController.cs:89,108`

```csharp
if (reversedStack.All(e => e.ActionMap != removedMap))
{
    removedMap.Disable();  // globalActionMap と同じ ActionMap だった場合でも Disable される
}
```

`globalLayer` と同じ `InputActionMap` をスタックに積んだ場合（意図的かどうかに関わらず）、Pop 時にグローバルレイヤーのマップも無効化されます。

**修正案:**
```csharp
if (reversedStack.All(e => e.ActionMap != removedMap) && globalActionMap != removedMap)
{
    removedMap.Disable();
}
```

---

### [M-5] 3つのディスパッチメソッドに重複コードがある

**ファイル:** `InputLayerController.cs:116-168`

`OnActionStarted`, `OnActionPerformed`, `OnActionCanceled` の実装が完全に同じ構造で3つ存在します。B-1 の修正と合わせて統合できます。

**修正案:**
```csharp
void DispatchAction(InputAction.CallbackContext ctx, Func<IInputLayer, InputAction.CallbackContext, bool> handler)
{
    if (globalActionMap != null && ctx.action.actionMap == globalActionMap)
    {
        if (globalLayer != null)
        {
            try { handler(globalLayer, ctx); }
            catch (Exception e) { Debug.LogException(e); }
        }
        return;
    }

    foreach (var entry in reversedStack)
    {
        bool consumed;
        try { consumed = handler(entry.Layer, ctx); }
        catch (Exception e) { Debug.LogException(e); consumed = false; }
        if (consumed || entry.Layer.BlocksAllInput) { break; }
    }
}

void OnActionStarted(InputAction.CallbackContext ctx) =>
    DispatchAction(ctx, (layer, c) => layer.OnActionStarted(c));
void OnActionPerformed(InputAction.CallbackContext ctx) =>
    DispatchAction(ctx, (layer, c) => layer.OnActionPerformed(c));
void OnActionCanceled(InputAction.CallbackContext ctx) =>
    DispatchAction(ctx, (layer, c) => layer.OnActionCanceled(c));
```

---

## 中程度の問題 (Minor)

### [N-1] `reversedStack` という変数名が誤解を招く

**ファイル:** `InputLayerController.cs:17`

名前から「逆順リスト」のように読めますが、実際には「先頭が最新（スタックのトップ）」な自然な順序のリストです。

**修正案:** `layerStack` に改名

---

### [N-2] Back アクションを `OnActionStarted` で処理している

**ファイル:** `DefaultSceneInputLayer.cs:22`, `DefaultScreenStackInputLayer.cs:22`

```csharp
public virtual bool OnActionStarted(InputAction action)
{
    if (action.id == sceneActions.Back.id && onBack != null)
    {
        onBack();  // Started はボタンを押し始めた瞬間
        return true;
    }
    return false;
}
```

`Started` はインタラクション開始時（Press 開始）に発火します。Back はユーザーが「意図的に押した」`Performed` で処理するのが一般的であり、他の入力処理との一貫性も保てます。（意図的な設計であれば許容範囲）

---

### [N-3] `SampleProduct` 基底クラスが具体型 `InputActions` に直接依存している

**ファイル:** `ProductCanvasMainSceneBase.cs:24,27-35`

```csharp
InputActions inputActions;   // SampleProduct 固有の具体型

[Inject]
public void ConstructInputLayer(
    ISceneManager sceneManager,
    IInputLayerController inputLayerController,
    InputActions inputActions)  // 別プロジェクトでは使えない
```

別プロジェクトがこのフレームワークを採用する際、`ProductCanvasMainSceneBase` は `InputActions` という具体型に縛られるため、そのまま再利用できません。

**修正案（ジェネリクスで抽象化）:**
```csharp
public abstract class ProductCanvasMainSceneBase<TTransitionData, TInputActions>
    : CanvasMainSceneBase<TTransitionData>
    where TTransitionData : ProductTransitionDataBase
{
    protected abstract IInputLayer CreateInputLayer(TInputActions inputActions);
    protected abstract InputActionMap GetInputLayerActionMap(TInputActions inputActions);
}
```

---

### [N-4] `ValidateNoOverlapWithGlobal` が `PushLayer` の後に実行される

**ファイル:** `InputLayerController.cs:67-75`

```csharp
public void PushLayer(IInputLayer layer, InputActionMap actionMap)
{
    reversedStack.Insert(0, new LayerEntry(layer, actionMap));
    actionMap.Enable();  // 先に Enable してしまう

#if UNITY_EDITOR
    ValidateNoOverlapWithGlobal(actionMap);  // バリデーションは後
#endif
```

バリデーションは Push 前に実行すべきです。

**修正案:**
```csharp
public void PushLayer(IInputLayer layer, InputActionMap actionMap)
{
    if (layer == null) throw new ArgumentNullException(nameof(layer));
    if (actionMap == null) throw new ArgumentNullException(nameof(actionMap));

#if UNITY_EDITOR
    ValidateNoOverlapWithGlobal(actionMap);  // Push 前にバリデーション
#endif

    layerStack.Insert(0, new LayerEntry(layer, actionMap));
    actionMap.Enable();
    ...
}
```

---

### [N-5] `IInputLayer` / `IInputLayerController` にドキュメントコメントがない

汎用フレームワークとして不特定多数のプロジェクトに提供するなら、インターフェースの契約（`bool` 戻り値の意味、`BlocksAllInput` の動作仕様）を XML コメントで明記すべきです。

```csharp
public interface IInputLayer
{
    /// <summary>
    /// true を返すと、このレイヤーより下位のレイヤーへの伝播を全て遮断する。
    /// consumed の戻り値とは独立して動作する。
    /// </summary>
    bool BlocksAllInput { get; }

    /// <summary>
    /// アクション開始時に呼ばれる。
    /// </summary>
    /// <returns>true: 入力を消費（下位レイヤーに伝播しない）, false: 伝播を続ける</returns>
    bool OnActionStarted(InputAction.CallbackContext context);

    /// <inheritdoc cref="OnActionStarted"/>
    bool OnActionPerformed(InputAction.CallbackContext context);

    /// <inheritdoc cref="OnActionStarted"/>
    bool OnActionCanceled(InputAction.CallbackContext context);
}
```

---

## 評価5を得るための修正ロードマップ

### Phase 1: Breaking Changes（Blocker の解消）

- [ ] `IInputLayer` の全コールバックを `InputAction.CallbackContext` に変更（B-1）
- [ ] `IInputLayerController` に `IDisposable` と `RemoveGlobalLayer()` を追加（B-2, B-3）
- [ ] `SetGlobalLayer` に旧マップの `Disable` 処理を追加（B-2）
- [ ] `InputLayerController.Dispose()` が `globalActionMap` も `Disable` するよう修正

### Phase 2: 堅牢性の強化

- [ ] 全パブリックメソッドに `ArgumentNullException` ガードを追加（M-2）
- [ ] ディスパッチループを `try-catch` で囲む（M-1）
- [ ] `Pop` 系メソッドに `globalActionMap` チェックを追加（M-4）
- [ ] `Debug.Log` を `#if UNITY_EDITOR` または `DEVELOPMENT_BUILD` で制限（M-3）

### Phase 3: 設計品質の向上

- [ ] 3つのディスパッチメソッドを `DispatchAction` に統合（M-5）
- [ ] `reversedStack` を `layerStack` に改名（N-1）
- [ ] `ValidateNoOverlapWithGlobal` を Push 前に移動（N-4）
- [ ] `IInputLayer` / `IInputLayerController` に XML ドキュメントコメントを追加（N-5）

### Phase 4: 汎用性の強化（推奨）

- [ ] `Back` 処理を `OnActionStarted` → `OnActionPerformed` に変更を検討（N-2）
- [ ] `ProductCanvasMainSceneBase<T>` のジェネリクス化で `InputActions` 依存を抽象化（N-3）
- [ ] スタック状態のクエリメソッド追加（`ContainsLayer`, `LayerCount` など）

---

**Phase 1〜3 完了後の見込み評価: ★★★★☆ (4/5)**  
**Phase 4 完了後の見込み評価: ★★★★★ (5/5)**

> 最優先は **B-1**（`CallbackContext` への変更）。これ単体でフレームワークの汎用性が大幅に向上します。
