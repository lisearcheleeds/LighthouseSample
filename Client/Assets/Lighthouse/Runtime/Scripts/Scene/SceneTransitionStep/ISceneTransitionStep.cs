using System.Threading;
using Cysharp.Threading.Tasks;

namespace Lighthouse.Scene.SceneTransitionStep
{
    public interface ISceneTransitionStep
    {
        UniTask Run(SceneTransitionContext context, CancellationToken cancelToken);
    }
}