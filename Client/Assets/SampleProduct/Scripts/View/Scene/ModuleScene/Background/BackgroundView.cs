using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace SampleProduct.View.Scene.ModuleScene.Background
{
    public class BackgroundView : MonoBehaviour
    {
        [SerializeField] Image background;

        public async UniTask SetBackground(string backgroundAsset)
        {
            // background.SetBackground(backgroundAsset);
        }
    }
}