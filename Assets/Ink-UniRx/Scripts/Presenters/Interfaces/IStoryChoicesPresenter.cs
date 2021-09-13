using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using InkUniRx.ViewModels;

namespace InkUniRx.Presenters.Interfaces
{
    public interface IStoryChoicesPresenter
    {
        public UniTask OnShowStoryChoicesAsync(StoryChoices storyChoices, CancellationToken ct);
    }
}