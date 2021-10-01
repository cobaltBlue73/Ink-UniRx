using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using TMPro;
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

        private string _whiteSpaceBuffer = string.Empty;

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
            _whiteSpaceBuffer = string.Empty;
            textPagedView.MaxVisibleCharacters = 0;
            _lastDisplayedPage = 1;
            SetPage(0);
        }

        public override void AddContent(string contentText)
        {
            if (string.IsNullOrWhiteSpace(contentText))
            {
                _whiteSpaceBuffer += string.IsNullOrEmpty(_whiteSpaceBuffer)? 
                    contentText: $"\n{contentText}";
                return;
            }
            
            var newText = string.Empty;
            
            if (!string.IsNullOrEmpty(_whiteSpaceBuffer))
            {
                newText = $"{_whiteSpaceBuffer}\n";
                _whiteSpaceBuffer = string.Empty;
            }

            newText += contentText;
            
            if (textPagedView.IsEmpty)
            {
                textPagedView.Text = newText;
                textPagedView.ForceTextUpdate();
                return;
            }
         
            var prevTextLength = textPagedView.Text.Length;
            var prevPageCount = textPagedView.PageCount;
            textPagedView.Text += $"\n{newText}";
            textPagedView.ForceTextUpdate();

            if (textPagedView.PageCount > prevPageCount)
            {
                textPagedView.Text = textPagedView.Text.Substring(0, prevTextLength);
                textPagedView.Text += $"<page>{contentText}";
                textPagedView.ForceTextUpdate();
            }
        }

        public override UniTask ShowNewContentAsync(CancellationToken animationCancelToken)
        {
            var from = textPagedView.MaxVisibleCharacters;
            var to = textPagedView.CharacterCount - 1;

            if (PageCount > 0)
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