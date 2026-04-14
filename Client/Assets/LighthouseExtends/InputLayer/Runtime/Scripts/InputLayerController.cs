using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace LighthouseExtends.InputLayer
{
    public class InputLayerController : MonoBehaviour, IInputLayerController
    {
        // ── Serialized Fields ─────────────────────────────────────────

        /// <summary>
        /// Unity Editor で作成した InputActionAsset をアタッチする。
        /// Construct 時に Enable される。
        /// </summary>
        [SerializeField] InputActionAsset inputActionAsset;

        // ── Private Fields ────────────────────────────────────────────

        readonly List<LayerEntry> reversedStack = new(); // index 0 = 最上位レイヤー

        // ── Inner Types ──────────────────────────────────────────────

        class LayerEntry
        {
            public InputLayer Layer { get; }

            /// <summary>上位レイヤーが消費済みのコントロールセット（このレイヤーには届かない）</summary>
            public HashSet<InputControl> BlockedControls { get; set; } = new();

            public LayerEntry(InputLayer layer) => Layer = layer;
        }

        // ── Lifecycle ────────────────────────────────────────────────

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
                if (stop)
                {
                    break;
                }
            }
        }

        // ── Public API ───────────────────────────────────────────────

        /// <summary>レイヤーをスタックに積む。</summary>
        public void PushLayer(InputLayer layer)
        {
            layer.SetController(this);
            reversedStack.Insert(0, new LayerEntry(layer));
            Cursor.lockState = layer.CursorLockMode;
            RecalculateBlockedControls();
            Debug.Log($"[InputLayer] Push: {StackToString()}");
        }

        /// <summary>最上位レイヤーを取り除く。</summary>
        public void PopLayer()
        {
            if (reversedStack.Count == 0)
            {
                return;
            }

            reversedStack.RemoveAt(0);
            Cursor.lockState = reversedStack.Count > 0
                ? reversedStack[0].Layer.CursorLockMode
                : CursorLockMode.None;
            RecalculateBlockedControls();
            Debug.Log($"[InputLayer] Pop: {StackToString()}");
        }

        /// <summary>
        /// 指定レイヤーをスタック内のどの位置からでも取り除く。
        /// それより上位のレイヤーはそのまま維持される。
        /// </summary>
        public void PopLayer(InputLayer target)
        {
            var removed = reversedStack.RemoveAll(e => e.Layer == target);
            if (removed == 0)
            {
                return;
            }

            Cursor.lockState = reversedStack.Count > 0
                ? reversedStack[0].Layer.CursorLockMode
                : CursorLockMode.None;
            RecalculateBlockedControls();
            Debug.Log($"[InputLayer] PopTarget({target.GetType().Name}): {StackToString()}");
        }

        /// <summary>
        /// アクション名で InputAction を検索して返す。
        /// 見つからない場合は null を返す（警告ログあり）。
        /// アクション名が複数のマップに存在する場合は最初に見つかったものを返す。
        /// 区別が必要な場合は "MapName/ActionName" 形式で指定する。
        /// </summary>
        public InputAction GetAction(string actionName)
        {
            var action = inputActionAsset.FindAction(actionName, throwIfNotFound: false);
            if (action == null)
            {
                Debug.LogWarning($"[InputLayer] Action not found: {actionName}");
            }
            return action;
        }

        // ── Private ──────────────────────────────────────────────────

        /// <summary>
        /// 各レイヤーの BlockedControls を再計算する。
        /// スタックへの Push/Pop 時に呼ぶ。
        /// </summary>
        void RecalculateBlockedControls()
        {
            var accumulated = new HashSet<InputControl>();

            for (var i = 0; i < reversedStack.Count; i++)
            {
                // このレイヤーへの入力は、上位レイヤーの消費分がブロックされる
                reversedStack[i].BlockedControls = new HashSet<InputControl>(accumulated);

                // このレイヤーが消費するコントロールを次以降に積み上げる
                accumulated.UnionWith(reversedStack[i].Layer.GetConsumedControls());
            }
        }

        string StackToString() =>
            string.Join(" > ", reversedStack.Select(e => e.Layer.GetType().Name));
    }
}
