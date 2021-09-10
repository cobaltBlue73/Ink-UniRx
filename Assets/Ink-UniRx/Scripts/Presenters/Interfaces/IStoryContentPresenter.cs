using System.Threading;
using Cysharp.Threading.Tasks;
using InkUniRx.ViewModels;

namespace InkUniRx.Presenters.Interfaces
{
    public interface IStoryContentPresenter
    {
        UniTask OnShowStoryContentAsync(StoryContent storyContent, CancellationToken ct);
    }

}