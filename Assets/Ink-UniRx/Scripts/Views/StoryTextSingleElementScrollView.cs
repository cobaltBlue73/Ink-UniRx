using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace InkUniRx.Views
{
    public class StoryTextSingleElementScrollView: StoryTextScrollView
    {
        #region Inspector

        [SerializeField, Required, InlineEditor] 
        private StoryTextScrollElementView elementView;
        
        #endregion

        #region Properties

        public override string Text => elementView.Text;
        public override bool IsEmpty =>  elementView.IsEmpty;

        #endregion

        #region Variables

        private Tweener _scrollTween;

        #endregion

        #region Methods

        #region Unity Callbacks

        protected override void Reset()
        {
            base.Reset();
            
            if (!elementView)
                elementView = GetComponentInChildren<StoryTextScrollElementView>();
        }

        private void Start() => ClearText();

        #endregion

        #region Public

        public override void ClearText()
        {
            base.ClearText();
            elementView.ClearText();
            elementView.VisibleCharacters = 0;
        }

        public override void AddText(string text) => elementView.AddText(IsEmpty ? text : $"\n{text}");

        protected override UniTask PlayTextAnimationsAsync(CancellationToken cancelAnimationToken)
        {
            elementView.TextMesh.ForceMeshUpdate();
            
            var from = elementView.VisibleCharacters;
            var to = elementView.CharacterCount - 1;
            elementView.VisibleCharacters = elementView.CharacterCount;
            
            return elementView.PlayTextAnimationsAsync(from, to, cancelAnimationToken);
        }

        #endregion

        #region Private
        

        #endregion

        #endregion
        
    }
}