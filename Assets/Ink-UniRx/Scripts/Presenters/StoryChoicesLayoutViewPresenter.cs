using Cysharp.Threading.Tasks;
using InkUniRx.Presenters.Events;
using InkUniRx.Views;
using UniRx;

namespace InkUniRx.Presenters
{
    public class StoryChoicesLayoutViewPresenter: StoryChoicesViewPresenter<StoryChoicesLayoutView>
    {
        #region Story Callback
        protected override async UniTask<Unit> OnChoiceSelectionAsync(StoryPathSelectChoice selectChoice)
        {
            choicesView.SetChoices(selectChoice.Story.currentChoices);

            await choicesView.ShowAsync(selectChoice.CancelAnimationToken);

            var (isCanceled, choice) = await WhenChoiceSelected
                .ToUniTask(true, selectChoice.CancelStoryToken)
                .SuppressCancellationThrow();

            if (!selectChoice.CancelStoryToken.IsCancellationRequested)
            {
                await choicesView.HideAsync(selectChoice.CancelAnimationToken);
            
                if(selectChoice.Story.HasChoices())
                    selectChoice.Story.ChooseChoice(choice);
            }
            else
            {
                choicesView.HideAsync(selectChoice.CancelAnimationToken, false);
            }

            return Unit.Default;
        }
        #endregion
       
    }
}