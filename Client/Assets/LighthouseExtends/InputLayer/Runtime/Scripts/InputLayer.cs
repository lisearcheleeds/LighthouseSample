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
                if (action == null)
                {
                    continue;
                }
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
            if (action == null)
            {
                return false;
            }
            if (IsConsumed(action, consumedControls))
            {
                return false;
            }
            return action.IsPressed();
        }

        /// <summary>このフレームにアクションが押されたか。</summary>
        protected bool WasPressedThisFrame(string actionName, HashSet<InputControl> consumedControls)
        {
            var action = controller.GetAction(actionName);
            if (action == null)
            {
                return false;
            }
            if (IsConsumed(action, consumedControls))
            {
                return false;
            }
            return action.WasPressedThisFrame();
        }

        /// <summary>このフレームにアクションが離されたか。</summary>
        protected bool WasReleasedThisFrame(string actionName, HashSet<InputControl> consumedControls)
        {
            var action = controller.GetAction(actionName);
            if (action == null)
            {
                return false;
            }
            if (IsConsumed(action, consumedControls))
            {
                return false;
            }
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
                if (consumedControls.Contains(control))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
