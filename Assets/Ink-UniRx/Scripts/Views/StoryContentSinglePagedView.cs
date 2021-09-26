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

        #endregion

        #region Variables

        private string _whiteSpaceBuffer = string.Empty;
        private int _displayedPagesCount = 1;
        
        #endregion

        #region Methods

        #region Unity Callabcks

        private void Reset()
        {
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
            _displayedPagesCount = 1;
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
            
            
            if (_displayedPagesCount < textPagedView.PageCount)
            {
                var lastDisplayedPageInfo = textPagedView.TextMesh
                    .textInfo.pageInfo[_displayedPagesCount - 1];

                if (textPagedView.MaxVisibleCharacters >
                    lastDisplayedPageInfo.lastCharacterIndex)
                {
                    ++_displayedPagesCount;
                }

                textPagedView.CurrentPage = _displayedPagesCount;
                
                if ( textPagedView.CurrentPage < textPagedView.PageCount)
                    to = textPagedView.LastCharacterIndexOnPage;
            }
            
            textPagedView.MaxVisibleCharacters = to + 1;

            return textPagedView.AnimateTextAsync(from, to, animationCancelToken);
        }

        #endregion

        #endregion
        
        
     
    }
}