using System.IO;
using UnityEngine;

namespace LighthouseExtends.Popup.Editor.ScriptableObject
{
    public class PopupGenerateSettings : UnityEngine.ScriptableObject
    {
        [SerializeField] string popupEntityFactoryDirectory = "";
        [SerializeField] string popupEntityFactoryClassName = "PopupEntityFactory";
        [SerializeField] string popupEntityFactoryNamespace = "";

        public string PopupEntityFactoryFilePath =>
            string.IsNullOrEmpty(popupEntityFactoryDirectory)
                ? string.Empty
                : Path.Combine(Application.dataPath, popupEntityFactoryDirectory, $"{popupEntityFactoryClassName}.g.cs");

        public string PopupEntityFactoryClassName => popupEntityFactoryClassName;
        public string PopupEntityFactoryNamespace => popupEntityFactoryNamespace;
    }
}
