using System.Threading;
using Cysharp.Threading.Tasks;
using InkUniRx.ViewModels;

namespace InkUniRx.Presenters.Interfaces
{
    public interface IStoryPathContentPresenter
    {
        UniTask OnShowStoryPathContentAsync(StoryPathContent storyPathContent, CancellationToken ct);
    }

}