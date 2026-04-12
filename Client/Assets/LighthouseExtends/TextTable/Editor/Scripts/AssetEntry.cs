namespace LighthouseExtends.TextTable.Editor
{
    public class AssetEntry
    {
        public readonly string assetPath;
        public readonly string displayName;
        public readonly bool isScene;
        public readonly string tsvBaseName;

        public AssetEntry(string assetPath, string displayName, string tsvBaseName, bool isScene)
        {
            this.assetPath = assetPath;
            this.displayName = displayName;
            this.tsvBaseName = tsvBaseName;
            this.isScene = isScene;
        }
    }
}
