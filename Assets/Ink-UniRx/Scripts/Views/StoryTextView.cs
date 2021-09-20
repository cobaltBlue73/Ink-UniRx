using Cysharp.Threading.Tasks;
using UnityEngine;

namespace InkUniRx.Views
{
    public abstract class StoryTextView : MonoBehaviour
    {
        #region Internals

        public enum WhiteSpaceHandling
        {
            Skip,
            SkipAnimation
        }

        #endregion
        #region Inspector

        [SerializeField] protected bool trim;
        [SerializeField] protected WhiteSpaceHandling handleWhiteSpace;

        #endregion
        #region Properties
        public abstract string Text { get; }
        public abstract bool IsEmpty { get; }

        #endregion
        #region Methods
        public abstract void ClearText();
        public abstract UniTask AddTextAsync(string text);
        #endregion
    }
}