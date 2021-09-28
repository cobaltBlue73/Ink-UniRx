using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace InkUniRx.Views
{
    public class StoryContentSingleElementScrollView: StoryContentScrollView
    {
        #region Inspector

        [SerializeField, Required, InlineEditor] 
        private StoryTextElementView elementView;
        
        #endregion

        #region Properties

        public override string ContentText => elementView.Text;
        public override bool IsEmpty => elementView.IsEmpty;

        #endregion
        
        #region Methods

        #region Unity Callbacks

        protected override void Reset()
        {
            base.Reset();
            
            if (!elementView)
                elementView = GetComponentInChildren<StoryTextElementView>();
        }
        
        #endregion

        #region Public

        public override void ClearContent()
        {
            base.ClearContent();
            elementView.ClearText();
            elementView.MaxVisibleCharacters = 0;
        }

        public override void AddContent(string contentText)
        {
            elementView.Text += IsEmpty ? contentText : $"\n{contentText}";
            elementView.ForceTextUpdate();
        }

        protected override UniTask ShowNewTextAsync(CancellationToken animationCancelToken)
        {
            if (elementView.MaxVisibleCharacters >= 
                elementView.CharacterCount) return UniTask.CompletedTask;
            
            var from = elementView.MaxVisibleCharacters;
            var to = elementView.CharacterCount - 1;
            elementView.MaxVisibleCharacters = elementView.CharacterCount;

            return elementView.AnimateTextAsync(from, to, animationCancelToken);
        }

        #endregion
        
        #endregion
        
    }
}