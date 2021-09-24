using System;
using Sirenix.OdinInspector;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace InkUniRx.Views
{
    [RequireComponent(typeof(TMP_Text),
        typeof(LayoutElement))]
    public class StoryTextScrollElementView : StoryTextTextMeshView
    {
        #region Inpsector
        
        [SerializeField, Required, InlineEditor] private LayoutElement layoutElement;
        
        #endregion
        
        #region Properties
        public LayoutElement LayoutElement => layoutElement;

        public int ChildIndex => transform.GetSiblingIndex();
        
        public IObservable<StoryTextScrollElementView> WhenCulled => 
          IsCulled.Where(result=> result).Select(_=> this);
        
        public IObservable<StoryTextScrollElementView> WhenVisible => 
            IsCulled.Where(result=> !result).Select(_=> this);

        #endregion

        #region Unity Callbacks

        protected override void Reset()
        {
            base.Reset();

            if (!LayoutElement)
                layoutElement = GetComponent<LayoutElement>();
        }
        
        #endregion

      
    }
}