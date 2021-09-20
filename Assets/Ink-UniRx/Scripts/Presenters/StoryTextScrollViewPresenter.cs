using System;
using Cysharp.Threading.Tasks;
using InkUniRx.Presenters.Events;
using InkUniRx.Views;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

namespace InkUniRx.Presenters
{
    public class StoryTextScrollViewPresenter: StoryTextViewPresenter<StoryTextScrollView>
    {
        protected override async UniTask<Unit> OnNewStoryTextAsync(StoryPathNewText newStoryText)
        {
            await textView.AddTextAsync(newStoryText.Story.currentText, newStoryText.CancelAnimationToken);

            var whenContinue = MessageBroker.Default.Receive<ContinueStory>().AsUnitObservable();

            if (autoContinue)
            {
                whenContinue = whenContinue.Merge(Observable.Timer(TimeSpan.FromSeconds(autoContinueDelay))
                    .AsUnitObservable());
            }

            await whenContinue.ToUniTask(true, newStoryText.CancelStoryToken)
                .SuppressCancellationThrow();
            
            return Unit.Default;
        }
    }
}