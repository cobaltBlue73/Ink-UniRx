using System.Threading;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace InkUniRx.Views
{
    public class StoryContentSinglePagedView: StoryContentPagedView
    {
        #region Inpsector

        [SerializeField, Required, InlineEditor] 
        private StoryTextPagedView textPagedView;

        #endregion

        #region Properties
        public override string ContentText => textPagedView.Text;
        public override bool IsEmpty => textPagedView.IsEmpty;
        public override int PageCount => textPagedView.PageCount;
        public override int CurrentPage => textPagedView.CurrentPage;

        public override int LastDisplayedPage => _lastDisplayedPage;

        #endregion

        #region Variables

        private int _lastDisplayedPage;

        #endregion

        #region Methods

        #region Unity Callabcks

        protected override void Reset()
        {
            base.Reset();
            
            if (!textPagedView)
                textPagedView = GetComponentInChildren<StoryTextPagedView>();
        }

        #endregion
        
        #region Public

        public override void ClearContent()
        {
            textPagedView.ClearText();
            textPagedView.ForceTextUpdate();
            textPagedView.MaxVisibleCharacters = 0;
            _lastDisplayedPage = 1;
            SetPage(0);
        }

        public override void AddContent(string contentText)
        {
            textPagedView.Text += contentText;
            textPagedView.ForceTextUpdate();
        }

        public override UniTask ShowNewContentAsync(CancellationToken animationCancelToken)
        {
            var from = textPagedView.MaxVisibleCharacters;
            var to = textPagedView.CharacterCount - 1;

            if (PageCount > 1)
            {
                SetPage(_lastDisplayedPage);
                to = textPagedView.LastCharacterIndexOnPage;
                
                if (from >= to)
                {
                    if (_lastDisplayedPage >= PageCount)
                        return UniTask.CompletedTask;
                    
                    SetPage(++_lastDisplayedPage);
                    to = textPagedView.LastCharacterIndexOnPage;
                }
            }
            
            textPagedView.MaxVisibleCharacters = to + 1;
            
            return textPagedView.AnimateTextAsync(from, to, animationCancelToken);
        }

        #endregion

        #region Protected

        protected override void SetPage(int page)
        {
            base.SetPage(page);
            OnPageSelected(page);
        }

        protected override void OnPageSelected(int page)
        {
            textPagedView.CurrentPage = page;
        }

        #endregion

        #endregion
        
        
     
    }
}