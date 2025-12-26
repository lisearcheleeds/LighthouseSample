using LighthouseExtends.UIComponent.Button;
using SampleProduct.Extensions;
using UnityEngine;

namespace SampleProduct.View.Common
{
    public sealed class OpenURLButton : LHButton
    {
        [SerializeField] string url;

        protected override void Start()
        {
            base.Start();

            this.SubscribeOnClick(() =>
            {
                Application.OpenURL(url);
            });
        }
    }
}