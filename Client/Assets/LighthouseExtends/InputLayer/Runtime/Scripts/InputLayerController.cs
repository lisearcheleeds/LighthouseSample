using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace LighthouseExtends.InputLayer
{
    public class InputLayerController : IInputLayerController, IDisposable
    {
        readonly InputActionAsset inputActionAsset;

        InputActionMap globalActionMap;
        IInputLayer globalLayer;

        readonly List<LayerEntry> reversedStack = new();

        class LayerEntry
        {
            public IInputLayer Layer { get; }
            public InputActionMap ActionMap { get; }

            public LayerEntry(IInputLayer layer, InputActionMap actionMap)
            {
                Layer = layer;
                ActionMap = actionMap;
            }
        }

        [Inject]
        public InputLayerController(InputActionAsset inputActionAsset)
        {
            this.inputActionAsset = inputActionAsset;

            foreach (var map in inputActionAsset.actionMaps)
            {
                foreach (var action in map.actions)
                {
                    action.started += OnActionStarted;
                    action.performed += OnActionPerformed;
                    action.canceled += OnActionCanceled;
                }
            }
        }

        public void Dispose()
        {
            foreach (var map in inputActionAsset.actionMaps)
            {
                foreach (var action in map.actions)
                {
                    action.started -= OnActionStarted;
                    action.performed -= OnActionPerformed;
                    action.canceled -= OnActionCanceled;
                }
            }
        }

        public void SetGlobalLayer(IInputLayer layer, InputActionMap actionMap)
        {
            globalActionMap = actionMap;
            globalLayer = layer;
            actionMap.Enable();

            Debug.Log($"[InputLayer] SetGlobal: {layer.GetType().Name} ({actionMap.name})");
        }

        public void PushLayer(IInputLayer layer, InputActionMap actionMap)
        {
            if (reversedStack.Count > 0)
            {
                reversedStack[0].ActionMap.Disable();
            }

            reversedStack.Insert(0, new LayerEntry(layer, actionMap));
            actionMap.Enable();

#if UNITY_EDITOR
            ValidateNoOverlapWithGlobal(actionMap);
#endif

            Debug.Log($"[InputLayer] Push: {StackToString()}");
        }

        public void PopLayer()
        {
            if (reversedStack.Count == 0)
            {
                return;
            }

            reversedStack[0].ActionMap.Disable();
            reversedStack.RemoveAt(0);

            if (reversedStack.Count > 0)
            {
                reversedStack[0].ActionMap.Enable();
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
            var removedMap = reversedStack[index].ActionMap;
            reversedStack.RemoveAt(index);

            if (wasTop)
            {
                removedMap.Disable();
                if (0 < reversedStack.Count)
                {
                    reversedStack[0].ActionMap.Enable();
                }
            }

            Debug.Log($"[InputLayer] PopTarget({target.GetType().Name}): {StackToString()}");
        }

        void OnActionStarted(InputAction.CallbackContext ctx)
        {
            if (globalActionMap != null && ctx.action.actionMap == globalActionMap)
            {
                globalLayer?.OnActionStarted(ctx.action);
            }
            else if (0 < reversedStack.Count)
            {
                reversedStack[0].Layer.OnActionStarted(ctx.action);
            }
        }

        void OnActionPerformed(InputAction.CallbackContext ctx)
        {
            if (globalActionMap != null && ctx.action.actionMap == globalActionMap)
            {
                globalLayer?.OnActionPerformed(ctx.action);
            }
            else if (0 < reversedStack.Count)
            {
                reversedStack[0].Layer.OnActionPerformed(ctx.action);
            }
        }

        void OnActionCanceled(InputAction.CallbackContext ctx)
        {
            if (globalActionMap != null && ctx.action.actionMap == globalActionMap)
            {
                globalLayer?.OnActionCanceled(ctx.action);
            }
            else if (0 < reversedStack.Count)
            {
                reversedStack[0].Layer.OnActionCanceled(ctx.action);
            }
        }

#if UNITY_EDITOR
        void ValidateNoOverlapWithGlobal(InputActionMap actionMap)
        {
            if (globalActionMap == null)
            {
                return;
            }

            var globalBindingPaths = new HashSet<string>();
            foreach (var action in globalActionMap.actions)
            {
                foreach (var binding in action.bindings)
                {
                    if (!string.IsNullOrEmpty(binding.effectivePath))
                    {
                        globalBindingPaths.Add(binding.effectivePath);
                    }
                }
            }

            foreach (var action in actionMap.actions)
            {
                foreach (var binding in action.bindings)
                {
                    if (!string.IsNullOrEmpty(binding.effectivePath) && globalBindingPaths.Contains(binding.effectivePath))
                    {
                        Debug.LogError($"[InputLayer] Binding overlap: Global '{globalActionMap.name}' and '{actionMap.name}' both bind '{binding.effectivePath}' (action: {action.name})");
                    }
                }
            }
        }
#endif

        string StackToString()
        {
            return string.Join(" > ", reversedStack.Select(e => e.Layer.GetType().Name));
        }
    }
}
