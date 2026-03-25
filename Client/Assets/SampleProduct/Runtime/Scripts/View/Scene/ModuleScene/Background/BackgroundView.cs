using UnityEngine;
using UnityEngine.UI;

namespace SampleProduct.View.Scene.ModuleScene.Background
{
    public class BackgroundView : MonoBehaviour
    {
        static readonly int FocusCenter = Shader.PropertyToID("_FocusCenter");
        static readonly int FocusRatio = Shader.PropertyToID("_FocusRatio");

        [SerializeField] RawImage background;

        BackgroundLayout backgroundLayout;

        Vector2 focusCenter;
        float focusRatio;

        Material backgroundMaterial;

        void Awake()
        {
            backgroundMaterial = new Material(background.material);
            background.material = backgroundMaterial;
        }

        void Update()
        {
            switch (backgroundLayout)
            {
                case BackgroundLayout.HomeLayout:
                    focusCenter = Vector2.Lerp(focusCenter, Vector2.zero, 0.1f);
                    focusRatio = Mathf.Lerp(focusRatio, 1.0f, 0.1f);
                    break;

                case BackgroundLayout.Sample1Layout:
                    focusCenter = Vector2.Lerp(focusCenter, new Vector2(0.1f, 0), 0.1f);
                    focusRatio = Mathf.Lerp(focusRatio, 0.9f, 0.1f);
                    break;

                case BackgroundLayout.Sample2Layout:
                    focusCenter = Vector2.Lerp(focusCenter, new Vector2(0.2f, 0), 0.1f);
                    focusRatio = Mathf.Lerp(focusRatio, 0.8f, 0.1f);
                    break;

                case BackgroundLayout.Sample3Layout:
                    focusCenter = Vector2.Lerp(focusCenter, new Vector2(0.3f, 0), 0.1f);
                    focusRatio = Mathf.Lerp(focusRatio, 0.7f, 0.1f);
                    break;
            }

            backgroundMaterial.SetVector(FocusCenter, focusCenter);
            backgroundMaterial.SetFloat(FocusRatio, focusRatio);
        }

        public void SetBackgroundLayout(BackgroundLayout backgroundLayout)
        {
            this.backgroundLayout = backgroundLayout;
        }
    }
}