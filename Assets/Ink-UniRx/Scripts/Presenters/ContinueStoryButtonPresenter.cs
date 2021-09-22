using InkUniRx.Presenters.Events;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace InkUniRx.Presenters
{
    [RequireComponent(typeof(Button))]
    public class ContinueStoryButtonPresenter : MonoBehaviour
    {
        #region Inspector

        [SerializeField, Required] private Button button;

        #endregion

        #region Unity Callbacks

        private void Reset()
        {
            if (!button)
                button = GetComponent<Button>();
        }

        private void Awake()
        {
            button.OnClickAsObservable()
                .Select(_ => ContinueStory.Default)
                .Subscribe(MessageBroker.Default.Publish)
                .AddTo(this);

            AsyncMessageBroker.Default.Subscribe<StoryPathStart>(_ => {
                gameObject.SetActive(true);
                return Observable.Empty<Unit>();
            }).AddTo(this);
            
            AsyncMessageBroker.Default.Subscribe<StoryPathEnd>(_ => {
                gameObject.SetActive(false);
                return Observable.Empty<Unit>();
            }).AddTo(this);
        }

        #endregion
    }
}