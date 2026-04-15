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

        string globalActionMapName;
        IInputLayer globalLayer;

        readonly List<LayerEntry> reversedStack = new();

        class LayerEntry
        {
            public IInputLayer Layer { get; }
            public string ActionMapName { get; }

            public LayerEntry(IInputLayer layer, string actionMapName)
            {
                Layer = layer;
                ActionMapName = actionMapName;
            }
        }

        [Inject]
        public void Construct()
        {
            foreach (var map in inputActionAsset.actionMaps)
            {
                foreach (var action in map.actions)
                {
                    action.started += OnActionStarted;
                    action.canceled += OnActionCanceled;
                }
            }
        }

        void OnDestroy()
        {
            foreach (var map in inputActionAsset.actionMaps)
            {
                foreach (var action in map.actions)
                {
                    action.started -= OnActionStarted;
                    action.canceled -= OnActionCanceled;
                }
            }
        }

        void OnActionStarted(InputAction.CallbackContext ctx)
        {
            if (ctx.action.actionMap.name == globalActionMapName)
            {
                globalLayer?.OnActionStarted(ctx.action);
            }
            else if (reversedStack.Count > 0)
            {
                reversedStack[0].Layer.OnActionStarted(ctx.action);
            }
        }

        void OnActionCanceled(InputAction.CallbackContext ctx)
        {
            if (ctx.action.actionMap.name == globalActionMapName)
            {
                globalLayer?.OnActionCanceled(ctx.action);
            }
            else if (reversedStack.Count > 0)
            {
                reversedStack[0].Layer.OnActionCanceled(ctx.action);
            }
        }

        public void SetGlobalLayer(IInputLayer layer, string actionMapName)
        {
            globalActionMapName = actionMapName;
            globalLayer = layer;

            var map = inputActionAsset.FindActionMap(actionMapName);
            map?.Enable();

            Debug.Log($"[InputLayer] SetGlobal: {layer.GetType().Name} ({actionMapName})");
        }

        public void PushLayer(IInputLayer layer, string actionMapName)
        {
            if (reversedStack.Count > 0)
            {
                inputActionAsset.FindActionMap(reversedStack[0].ActionMapName)?.Disable();
            }

            reversedStack.Insert(0, new LayerEntry(layer, actionMapName));
            inputActionAsset.FindActionMap(actionMapName)?.Enable();

#if UNITY_EDITOR
            ValidateNoOverlapWithGlobal(actionMapName);
#endif

            Debug.Log($"[InputLayer] Push: {StackToString()}");
        }

        public void PopLayer()
        {
            if (reversedStack.Count == 0)
            {
                return;
            }

            inputActionAsset.FindActionMap(reversedStack[0].ActionMapName)?.Disable();
            reversedStack.RemoveAt(0);

            if (reversedStack.Count > 0)
            {
                inputActionAsset.FindActionMap(reversedStack[0].ActionMapName)?.Enable();
            }

            Debug.Log($"[InputLayer] Pop: {StackToString()}");
        }

        public void PopLayer(IInputLayer target)
        {
            var index = reversedStack.FindIndex(e => e.Layer == target);
            if (index < 0)
            {
                return;
            }

            var wasTop = index == 0;
            var removedMapName = reversedStack[index].ActionMapName;
            reversedStack.RemoveAt(index);

            if (wasTop)
            {
                inputActionAsset.FindActionMap(removedMapName)?.Disable();
                if (reversedStack.Count > 0)
                {
                    inputActionAsset.FindActionMap(reversedStack[0].ActionMapName)?.Enable();
                }
            }

            Debug.Log($"[InputLayer] PopTarget({target.GetType().Name}): {StackToString()}");
        }

#if UNITY_EDITOR
        void ValidateNoOverlapWithGlobal(string actionMapName)
        {
            if (string.IsNullOrEmpty(globalActionMapName))
            {
                return;
            }

            var globalMap = inputActionAsset.FindActionMap(globalActionMapName);
            if (globalMap == null)
            {
                return;
            }

            var stackMap = inputActionAsset.FindActionMap(actionMapName);
            if (stackMap == null)
            {
                return;
            }

            var globalBindingPaths = new HashSet<string>();
            foreach (var action in globalMap.actions)
            {
                foreach (var binding in action.bindings)
                {
                    if (!string.IsNullOrEmpty(binding.effectivePath))
                    {
                        globalBindingPaths.Add(binding.effectivePath);
                    }
                }
            }

            foreach (var action in stackMap.actions)
            {
                foreach (var binding in action.bindings)
                {
                    if (!string.IsNullOrEmpty(binding.effectivePath) && globalBindingPaths.Contains(binding.effectivePath))
                    {
                        Debug.LogError($"[InputLayer] Binding overlap: Global '{globalActionMapName}' and '{actionMapName}' both bind '{binding.effectivePath}' (action: {action.name})");
                    }
                }
            }
        }
#endif

        string StackToString() =>
            string.Join(" > ", reversedStack.Select(e => e.Layer.GetType().Name));
    }
}
