using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace InkUniRx.Views
{
    public class StoryTextPagedView: StoryTextView
    {
        #region Properties

        public int PageCount => TextMesh.textInfo.pageCount;

        public int CurrentPage
        {
            get => TextMesh.pageToDisplay;
            set => TextMesh.pageToDisplay = Mathf.Clamp(value, 1, PageCount);
        }

        public int FirstCharacterIndexOnPage => 
            TextMesh.textInfo.pageInfo[CurrentPage - 1]
                .firstCharacterIndex;
        
        public int LastCharacterIndexOnPage => CurrentPage < PageCount?
            TextMesh.textInfo.pageInfo[CurrentPage - 1].lastCharacterIndex: 
            CharacterCount - 1;

        #endregion
        
        #region Methoos

        #region Unity Callbacks
        
        private void OnValidate()
        {
            if (TextMesh && OverflowMode != TextOverflowModes.Page)
                OverflowMode = TextOverflowModes.Page;
        }

        #endregion

        #region Public
        
        #endregion
        

        #endregion
    }
}