# InputLayer System — 実装仕様書（LighthouseArchitecture版）

## 概要

スタック型の入力レイヤー管理システム。
上位レイヤーが優先的に入力を処理し、使用したキーを「消費済み」としてマークすることで、
下位レイヤーへの伝播を防ぐ。

**採用するコアコンセプト**
- スタックによるレイヤー優先順位
- キー消費による下位レイヤーへのブロック

**このシステムが解決する問題**
- UIを開いている間はゲーム操作が効かない
- Scene遷移時に前のSceneのキー入力が残らない
- レイヤーを追加するだけで入力の優先順位が自動的に決まる

**キーバインド管理はUnity Input Systemに委ねる**
- 物理キーとアクションの対応はUnity EditorのInputActionAssetで定義する
- ゲームパッド・キーボード両対応やランタイムリバインドが標準機能として使える
- このシステムはアクション名（文字列）でInputActionAssetを参照するだけ

---

## 依存関係

```
Lighthouse/Runtime            ← 変更なし（依存されない）
LighthouseExtends/ScreenStack ← 変更なし（依存されない）
LighthouseExtends/InputLayer  ← 新規（フレームワーク層）
SampleProduct                 ← InputLayerを使う側（実装例）
```

**依存の向きは一方向を厳守する。**
InputLayerはScene/ScreenStackを参照しない。
Scene/ScreenStackとの組み合わせはSampleProduct側（Product層）で行う。

---

## ファイル構成

### LighthouseExtends/InputLayer（フレームワーク層）

```
LighthouseExtends/InputLayer/Runtime/Scripts/
  IInputLayerController.cs     # コントローラーインターフェース（DI用）
  InputLayerController.cs      # スタック管理・Update処理（MonoBehaviour）
  InputLayer.cs                # 抽象基底クラス

LighthouseExtends/InputLayer/Runtime/
  LighthouseExtends.InputLayer.Runtime.asmdef
```

### SampleProduct（実装例）

```
SampleProduct/Runtime/Scripts/
  Input/
    InputActionNames.cs              # アクション名定数
    Layer/
      SceneInputLayerBase.cs         # Scene用InputLayer基底
      ScreenStackInputLayerBase.cs   # ScreenStack用InputLayer基底
  View/Base/
    ProductCanvasMainSceneBase.cs    # InputLayerフック追加（既存ファイル更新）
  Core/
    ProductLifetimeScope.cs          # InputLayerController登録（既存ファイル更新）

SampleProduct/Runtime/
  PlayerInputActions.inputactions    # Unity InputActionAsset
```

---

## Unity InputActionAsset 設定

### SampleProduct用アクション定義

アクションマップ名: `UI`

| アクション名 | Action Type | バインド |
|---|---|---|
| `Confirm` | Button | Enter, Space, Gamepad East |
| `Cancel` | Button | Backspace |
| `Back` | Button | Escape, Gamepad West |
| `NavigateUp` | Button | Up Arrow, W, Gamepad LeftStick Up |
| `NavigateDown` | Button | Down Arrow, S, Gamepad LeftStick Down |
| `NavigateLeft` | Button | Left Arrow, A, Gamepad LeftStick Left |
| `NavigateRight` | Button | Right Arrow, D, Gamepad LeftStick Right |

**アクション名にモード名・文脈名を含めない**（設計ルール参照）

---

## 設計ルール

### ルール1: アクション名はモード名・文脈名を含まない

InputActionAsset のアクション名と `InputActionNames.cs` の定数名の両方に適用する。

**禁止**: `HomeSceneBack`, `DialogCancel`, `PurposeSceneConfirm`
**正しい**: `Back`, `Cancel`, `Confirm`

---

### ルール2: 消費済みコントロールの管理に HashSet を使う

`InputControl[]` で `Contains` を使うのは毎フレームO(n)になるため禁止。
`HashSet<InputControl>` を使う。

---

### ルール3: PopLayer(target) は対象のみを削除する

現在位置に関わらず、指定したレイヤーだけをスタックから取り除く。
それより上位のレイヤーは消さない。

---

### ルール4: UpdateInput の戻り値を守る

| 戻り値 | 意味 |
|--------|------|
| `true` | このレイヤーで処理終了。下位レイヤーは実行しない |
| `false` | 透過。下位レイヤーも引き続き実行する |

クラスのコメントに `return true/false` の意図を書くこと。

---

### ルール5: ボタン入力はすべてヘルパーメソッド経由で読む

`IsPressed`, `WasPressedThisFrame`, `WasReleasedThisFrame` 経由で読む。
これにより消費済みチェックが自動で行われる。

アナログ軸（マウスの delta, scroll, スティックの Vector2 など）はキー消費の対象外なので
レイヤー内で直接読んでよい。ただしコメントで「キー消費対象外のアナログ入力」と明記すること。

---

### ルール6: 依存の向きを守る

InputLayerはScene/ScreenStackに依存しない。
SceneやScreenStackとの組み合わせはSampleProduct（またはプロジェクト固有）層で行う。

---

## 実装

### IInputLayerController.cs

VContainerでDI注入するためのインターフェース。
`InputLayer.cs` から `GetAction` 経由で `InputAction` を取得するために参照する。

```csharp
using UnityEngine.InputSystem;

namespace LighthouseExtends.InputLayer
{
    public interface IInputLayerController
    {
        void PushLayer(InputLayer layer);
        void PopLayer();
        void PopLayer(InputLayer target);
        InputAction GetAction(string actionName);
    }
}
```

---

### InputLayer.cs

`SetController` は `InputLayerController.PushLayer` 時に自動で呼ばれる。
具体的なLayerクラスはコンストラクタ引数なしで作成できる。

```csharp
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LighthouseExtends.InputLayer
{
    public abstract class InputLayer
    {
        IInputLayerController controller;

        /// <summary>このレイヤーがアクティブな間のカーソル状態</summary>
        public abstract CursorLockMode CursorLockMode { get; }

        /// <summary>
        /// このレイヤーが消費するアクション名一覧。InputActionNames の定数を使うこと。
        /// ここに列挙したアクションに対応する物理コントロールは下位レイヤーに届かなくなる。
        /// </summary>
        protected abstract IReadOnlyList<string> ConsumedActions { get; }

        /// <summary>
        /// 入力処理。毎フレーム InputLayerController から呼ばれる。
        /// </summary>
        /// <param name="consumedControls">上位レイヤーが消費済みのコントロールセット</param>
        /// <returns>true=下位レイヤーへの伝播を止める / false=伝播を続ける</returns>
        public abstract bool UpdateInput(HashSet<InputControl> consumedControls);

        /// <summary>
        /// コントローラーを設定する。InputLayerController.PushLayer 時に自動呼び出し。
        /// </summary>
        internal void SetController(IInputLayerController controller)
        {
            this.controller = controller;
        }

        /// <summary>
        /// このレイヤーが消費する物理コントロールのセットを返す。
        /// InputLayerController が RecalculateBlockedControls を呼ぶ際に使用する。
        /// アクションに複数のバインドがある場合（キーボード+ゲームパッド等）はすべて含まれる。
        /// </summary>
        public HashSet<InputControl> GetConsumedControls()
        {
            var result = new HashSet<InputControl>();
            foreach (var actionName in ConsumedActions)
            {
                var action = controller.GetAction(actionName);
                if (action == null) continue;
                foreach (var control in action.controls)
                {
                    result.Add(control);
                }
            }
            return result;
        }

        // ── ヘルパーメソッド ──────────────────────────────────────────

        /// <summary>
        /// アクションが現在押されているか。
        /// 上位レイヤーが同アクションのコントロールを消費済みの場合は false を返す。
        /// </summary>
        protected bool IsPressed(string actionName, HashSet<InputControl> consumedControls)
        {
            var action = controller.GetAction(actionName);
            if (action == null) return false;
            if (IsConsumed(action, consumedControls)) return false;
            return action.IsPressed();
        }

        /// <summary>このフレームにアクションが押されたか。</summary>
        protected bool WasPressedThisFrame(string actionName, HashSet<InputControl> consumedControls)
        {
            var action = controller.GetAction(actionName);
            if (action == null) return false;
            if (IsConsumed(action, consumedControls)) return false;
            return action.WasPressedThisFrame();
        }

        /// <summary>このフレームにアクションが離されたか。</summary>
        protected bool WasReleasedThisFrame(string actionName, HashSet<InputControl> consumedControls)
        {
            var action = controller.GetAction(actionName);
            if (action == null) return false;
            if (IsConsumed(action, consumedControls)) return false;
            return action.WasReleasedThisFrame();
        }

        // ── Private ──────────────────────────────────────────────────

        /// <summary>
        /// アクションに紐づくコントロールのいずれかが消費済みセットに含まれるか確認する。
        /// 1つでも含まれていれば消費済みとみなす。
        /// </summary>
        static bool IsConsumed(InputAction action, HashSet<InputControl> consumedControls)
        {
            foreach (var control in action.controls)
            {
                if (consumedControls.Contains(control)) return true;
            }
            return false;
        }
    }
}
```

---

### InputLayerController.cs

`RegisterComponentInNewPrefab` で DontDestroyOnLoad の Singleton として登録する。
`LHInputBlocker` と同じ登録パターン。

```csharp
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace LighthouseExtends.InputLayer
{
    public class InputLayerController : MonoBehaviour, IInputLayerController
    {
        [SerializeField] InputActionAsset inputActionAsset;

        readonly List<LayerEntry> reversedStack = new(); // index 0 = 最上位レイヤー

        class LayerEntry
        {
            public InputLayer Layer { get; }

            /// <summary>上位レイヤーが消費済みのコントロールセット（このレイヤーには届かない）</summary>
            public HashSet<InputControl> BlockedControls { get; set; } = new();

            public LayerEntry(InputLayer layer) => Layer = layer;
        }

        [Inject]
        public void Construct()
        {
            inputActionAsset.Enable();
        }

        void OnDestroy()
        {
            inputActionAsset.Disable();
        }

        void Update()
        {
            for (var i = 0; i < reversedStack.Count; i++)
            {
                var entry = reversedStack[i];
                var stop = entry.Layer.UpdateInput(entry.BlockedControls);
                if (stop) break;
            }
        }

        public void PushLayer(InputLayer layer)
        {
            layer.SetController(this);
            reversedStack.Insert(0, new LayerEntry(layer));
            Cursor.lockState = layer.CursorLockMode;
            RecalculateBlockedControls();
            Debug.Log($"[InputLayer] Push: {StackToString()}");
        }

        public void PopLayer()
        {
            if (reversedStack.Count == 0) return;

            reversedStack.RemoveAt(0);
            Cursor.lockState = reversedStack.Count > 0
                ? reversedStack[0].Layer.CursorLockMode
                : CursorLockMode.None;
            RecalculateBlockedControls();
            Debug.Log($"[InputLayer] Pop: {StackToString()}");
        }

        public void PopLayer(InputLayer target)
        {
            var removed = reversedStack.RemoveAll(e => e.Layer == target);
            if (removed == 0) return;

            Cursor.lockState = reversedStack.Count > 0
                ? reversedStack[0].Layer.CursorLockMode
                : CursorLockMode.None;
            RecalculateBlockedControls();
            Debug.Log($"[InputLayer] PopTarget({target.GetType().Name}): {StackToString()}");
        }

        public InputAction GetAction(string actionName)
        {
            var action = inputActionAsset.FindAction(actionName, throwIfNotFound: false);
            if (action == null)
            {
                Debug.LogWarning($"[InputLayer] Action not found: {actionName}");
            }
            return action;
        }

        void RecalculateBlockedControls()
        {
            var accumulated = new HashSet<InputControl>();
            for (var i = 0; i < reversedStack.Count; i++)
            {
                reversedStack[i].BlockedControls = new HashSet<InputControl>(accumulated);
                accumulated.UnionWith(reversedStack[i].Layer.GetConsumedControls());
            }
        }

        string StackToString() =>
            string.Join(" > ", reversedStack.Select(e => e.Layer.GetType().Name));
    }
}
```

---

## SampleProduct側の実装

### ProductLifetimeScope への登録

`LHInputBlocker` と同じパターンで `InputLayerController` Prefabを登録する。

```csharp
// ProductLifetimeScope.cs 追加分
[SerializeField] InputLayerController inputLayerControllerPrefab;

// Configure() 内
builder.RegisterComponentInNewPrefab(inputLayerControllerPrefab, Lifetime.Singleton)
    .DontDestroyOnLoad()
    .As<IInputLayerController>();
```

---

### ProductCanvasMainSceneBase への統合

`CanvasMainSceneBase` の `OnEnter` / `OnLeave` をオーバーライドして InputLayer を管理する。
Lighthouseフレームワーク層（`CanvasMainSceneBase`）には一切手を加えない。

**呼び出し順序（Enter）:**
1. `ProductCanvasMainSceneBase.OnEnter` → InputLayer Push
2. `base.OnEnter`（canvasGroup.alpha=1）
3. `MainSceneBase.OnEnter`（gameObject SetActive, TransitionData取得）
4. 具体的なSceneの `OnEnter(TTransitionData, ...)` → ユーザーコード

**呼び出し順序（Leave）:**
1. `ProductCanvasMainSceneBase.OnLeave` → `base.OnLeave`（canvasGroup.alpha=0）
2. 具体的なSceneの `OnLeave` → ユーザーコード
3. base完了後 → InputLayer Pop

```csharp
public abstract class ProductCanvasMainSceneBase<TTransitionData>
    : CanvasMainSceneBase<TTransitionData>
    where TTransitionData : ProductTransitionDataBase
{
    // ... 既存フィールド ...

    IInputLayerController inputLayerController;
    InputLayer currentInputLayer;

    [Inject]
    public void ConstructInputLayer(IInputLayerController inputLayerController)
    {
        this.inputLayerController = inputLayerController;
    }

    /// <summary>
    /// このSceneで使用するInputLayerを返す。
    /// null を返した場合はInputLayerの操作を行わない。
    /// </summary>
    protected virtual InputLayer CreateInputLayer() => null;

    protected override async UniTask OnEnter(SceneTransitionContext context, CancellationToken cancelToken)
    {
        currentInputLayer = CreateInputLayer();
        if (currentInputLayer != null)
        {
            inputLayerController.PushLayer(currentInputLayer);
        }
        await base.OnEnter(context, cancelToken);
    }

    protected override async UniTask OnLeave(SceneTransitionContext context, CancellationToken cancelToken)
    {
        await base.OnLeave(context, cancelToken);
        if (currentInputLayer != null)
        {
            inputLayerController.PopLayer(currentInputLayer);
            currentInputLayer = null;
        }
    }
}
```

---

### ScreenStackでのInputLayer使用（手動Push/Pop）

ScreenStack・ScreenStackBase・ScreenStackManagerには手を加えない。
PresenterにIInputLayerControllerを注入して手動管理する。
`isResume`（前の画面から戻ってきた）時はPushしない。

```csharp
public class SomeDialogPresenter : IScreenStackPresenter
{
    readonly IInputLayerController inputLayerController;
    InputLayer inputLayer;

    [Inject]
    public SomeDialogPresenter(IInputLayerController inputLayerController)
    {
        this.inputLayerController = inputLayerController;
    }

    public async UniTask OnEnter(bool isResume)
    {
        if (!isResume)
        {
            inputLayer = new SomeDialogInputLayer();
            inputLayerController.PushLayer(inputLayer);
        }
        // ...
    }

    public async UniTask OnLeave()
    {
        inputLayerController.PopLayer(inputLayer);
        inputLayer = null;
        // ...
    }
}
```

---

### InputActionNames.cs（SampleProduct）

```csharp
namespace SampleProduct.Input
{
    /// <summary>
    /// InputActionAsset のアクション名定数。
    /// InputActionAsset 側のアクション名と常に一致させること。
    /// </summary>
    public static class InputActionNames
    {
        public const string Confirm        = "Confirm";
        public const string Cancel         = "Cancel";
        public const string Back           = "Back";
        public const string NavigateUp     = "NavigateUp";
        public const string NavigateDown   = "NavigateDown";
        public const string NavigateLeft   = "NavigateLeft";
        public const string NavigateRight  = "NavigateRight";
    }
}
```

---

### SceneInputLayerBase.cs（SampleProduct）

```csharp
using System.Collections.Generic;
using LighthouseExtends.InputLayer;
using UnityEngine;

namespace SampleProduct.Input
{
    /// <summary>
    /// Scene用InputLayer基底。
    /// Back/Cancel をデフォルトで消費する。
    /// return false で下位レイヤーにも処理を渡す想定。
    /// </summary>
    public abstract class SceneInputLayerBase : InputLayer
    {
        public override CursorLockMode CursorLockMode => CursorLockMode.None;

        protected override IReadOnlyList<string> ConsumedActions => new[]
        {
            InputActionNames.Back,
            InputActionNames.Cancel,
        };
    }
}
```

---

### ScreenStackInputLayerBase.cs（SampleProduct）

```csharp
using System.Collections.Generic;
using LighthouseExtends.InputLayer;
using UnityEngine;

namespace SampleProduct.Input
{
    /// <summary>
    /// ScreenStack用InputLayer基底。
    /// ナビゲーション系アクションをすべて消費する。
    /// return true で下位レイヤー（Scene等）への伝播を止める想定。
    /// </summary>
    public abstract class ScreenStackInputLayerBase : InputLayer
    {
        public override CursorLockMode CursorLockMode => CursorLockMode.None;

        protected override IReadOnlyList<string> ConsumedActions => new[]
        {
            InputActionNames.Confirm,
            InputActionNames.Cancel,
            InputActionNames.Back,
            InputActionNames.NavigateUp,
            InputActionNames.NavigateDown,
            InputActionNames.NavigateLeft,
            InputActionNames.NavigateRight,
        };
    }
}
```

---

### 具体的なScene/ScreenStack Layerの実装例

#### HomeSceneInputLayer

```csharp
using System.Collections.Generic;
using UnityEngine.InputSystem;

namespace SampleProduct.Input
{
    /// <summary>
    /// HomeScene用InputLayer。
    /// return false で下位レイヤーにも処理を渡す。
    /// </summary>
    public class HomeSceneInputLayer : SceneInputLayerBase
    {
        public override bool UpdateInput(HashSet<InputControl> consumedControls)
        {
            // HomeSceneで使いたいキー操作をここに書く
            return false;
        }
    }
}
```

#### SomeDialogInputLayer

```csharp
using System.Collections.Generic;
using UnityEngine.InputSystem;

namespace SampleProduct.Input
{
    /// <summary>
    /// Dialog用InputLayer。
    /// return true で下位レイヤー（Scene操作等）への伝播を止める。
    /// </summary>
    public class SomeDialogInputLayer : ScreenStackInputLayerBase
    {
        public override bool UpdateInput(HashSet<InputControl> consumedControls)
        {
            if (WasPressedThisFrame(InputActionNames.Back, consumedControls)
                || WasPressedThisFrame(InputActionNames.Cancel, consumedControls))
            {
                // DialogをCloseする処理（MessageBus等）
            }

            return true;
        }
    }
}
```

---

## asmdef 参照関係

```
LighthouseExtends.InputLayer.Runtime
  参照: Unity.InputSystem, VContainer

SampleProduct.Runtime（既存）
  参照に追加: LighthouseExtends.InputLayer.Runtime
```

---

## チェックリスト（実装後の確認）

- [ ] InputActionAsset のすべてのアクション名にモード名・文脈名が含まれていない
- [ ] `InputActionNames.cs` の定数名が InputActionAsset のアクション名と一致している
- [ ] `ConsumedActions` にアクション名を列挙し忘れていない（ブロック漏れの原因）
- [ ] `return true/false` の意図がクラスのコメントに書かれている
- [ ] アナログ軸を直接読む箇所に「キー消費対象外のアナログ入力」コメントがある
- [ ] `InputLayerController` の Inspector に InputActionAsset がアタッチされている
- [ ] `ProductLifetimeScope` に InputLayerController の Prefab フィールドと登録がある
- [ ] InputLayer → Scene/ScreenStack の依存が発生していないこと

---

## 既知の制限

- `ConsumedActions` は静的なリスト。状態によって消費するアクションを変えたい場合は `GetConsumedControls()` をオーバーライドして動的に返す。
- 同じアクション名が複数のアクションマップに存在する場合、`GetAction` は最初に見つかったものを返す。区別が必要な場合は `"MapName/ActionName"` 形式で定数を定義する。
- `RecalculateBlockedControls` はアクションに紐づく全デバイスのコントロールを消費済みセットに追加する。実際に使っていないデバイスのコントロールも含まれるが、そのコントロールが押されることはないため実害はない。
- シーン遷移アニメーション中（既存の `IInputBlocker` がブロック中）も InputLayerController の Update は動作する。これはCanvas入力ブロック（UI操作防止）と InputLayer（ゲームロジック上の優先制御）が別システムであるため意図的な動作。
