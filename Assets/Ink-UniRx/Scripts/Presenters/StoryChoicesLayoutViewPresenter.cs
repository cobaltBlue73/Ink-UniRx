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
            choicesView.gameObject.SetActive(true);
            choicesView.SetChoices(selectChoice.Story.currentChoices);

            var (isCanceled, choice) = await choicesView.WhenChoiceSelected
                .Merge(selectChoice.Story.OnMakeChoiceAsObservable())
                .ToUniTask(true, selectChoice.CancelStoryToken)
                .SuppressCancellationThrow();
            
            if(selectChoice.CancelStoryToken.IsCancellationRequested)
                return Unit.Default;
            
            choicesView.gameObject.SetActive(false);
            
            if(selectChoice.Story.HasChoices())
                selectChoice.Story.ChooseChoice(choice);
            
            return Unit.Default;
        }
        #endregion
       
    }
}