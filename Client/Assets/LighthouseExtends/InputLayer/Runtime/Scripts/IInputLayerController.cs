namespace LighthouseExtends.InputLayer
{
    public interface IInputLayerController
    {
        void SetGlobalLayer(IInputLayer layer, string actionMapName);
        void PushLayer(IInputLayer layer, string actionMapName);
        void PopLayer();
        void PopLayer(IInputLayer target);
    }
}
