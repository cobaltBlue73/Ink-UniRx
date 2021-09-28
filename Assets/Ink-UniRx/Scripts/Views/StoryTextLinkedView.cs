using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UniRx.Toolkit;
using UnityEngine;

namespace InkUniRx.Views
{
    public class StoryTextLinkedView: StoryTextView
    {
        #region Properties

        public bool IsHead => !PrevTextView;

        public bool IsLast => !NextTextView;

        public bool IsTextOverflowing => TextMesh.isTextOverflowing;
        
        public StoryTextLinkedView PrevTextView { get; private set; }
        public StoryTextLinkedView NextTextView { get; private set; }
       

        #endregion

        #region Vairables

     
        #endregion
        
        
        #region Unity Callbacks

        protected override void Reset()
        {
            base.Reset();

            if (!TextMesh && OverflowMode != TextOverflowModes.Overflow)
                OverflowMode = TextOverflowModes.Overflow;
        }

        protected override void Awake()
        {
            base.Awake();
            PrevTextView = NextTextView = null;
        }

        #endregion

        #region Methods

        #region Public

    
        #endregion

        #region private

        public void LinkNextTextView(StoryTextLinkedView nextTextView)
        {
            NextTextView = nextTextView;
            NextTextView.PrevTextView = this;
            OverflowMode = TextOverflowModes.Linked;
            NextTextView.OverflowMode = TextOverflowModes.Overflow;
            TextMesh.linkedTextComponent = nextTextView.TextMesh;
            nextTextView.MaxVisibleCharacters = MaxVisibleCharacters;
            ForceTextUpdate();
            NextTextView.ForceTextUpdate();
        }

        public void UnlinkNextTextView()
        {
            if (!NextTextView) return;
            
            TextMesh.linkedTextComponent = null;
            OverflowMode = TextOverflowModes.Overflow;
            ForceTextUpdate();
            NextTextView.ForceTextUpdate();
            NextTextView = null;
        }

        public void UnlinkPrevTextView()
        {
            if (!PrevTextView) return;
            
            PrevTextView.UnlinkNextTextView();
            PrevTextView = null;
        }

        public void UnlinkAllViews()
        {
            UnlinkNextTextView();
            UnlinkPrevTextView();
        }
        
        #endregion
        

        #endregion
    }
}