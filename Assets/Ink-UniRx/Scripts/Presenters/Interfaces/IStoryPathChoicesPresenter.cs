using System.Threading;
using Cysharp.Threading.Tasks;
using InkUniRx.ViewModels;

namespace InkUniRx.Presenters.Interfaces
{
    public interface IStoryPathChoicesPresenter
    {
        public UniTask OnShowStoryPathChoicesAsync(StoryPathChoices storyPathChoices, CancellationToken ct);
    }
}