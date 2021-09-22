using InkUniRx.Presenters.Events;
using UniRx;
using UnityEngine;

namespace Ink_UniRx.Scripts.KeyBinders
{
    public class ContinueStoryKeyBinder : KeyBinder
    {
        #region Inspector

        [SerializeField] private KeyCode keyCode;

        #endregion
        #region Callbacks

        protected override bool GetKeyPress() => Input.GetKeyDown(keyCode);

        protected override void OnKeyPress() => MessageBroker.Default.Publish(ContinueStory.Default);

        #endregion
      
    }
}