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
        
        public IObservable<StoryTextScrollElementView> WhenBecameVisible => 
            _becameVisible.AsObservable();

        public IObservable<StoryTextScrollElementView> WhenBecameInvisible =>
            _becameInvisible.AsObservable();

        public int ChildIndex => transform.GetSiblingIndex();
        
        #endregion

        #region Variables

        private readonly Subject<StoryTextScrollElementView> _becameVisible = 
            new Subject<StoryTextScrollElementView>();

        private readonly Subject<StoryTextScrollElementView> _becameInvisible = 
            new Subject<StoryTextScrollElementView>();

        #endregion
        
        #region Unity Callbacks

        protected override void Reset()
        {
            base.Reset();

            if (!LayoutElement)
                layoutElement = GetComponent<LayoutElement>();
        }
        
        private void OnBecameVisible() => _becameVisible.OnNext(this);

        private void OnBecameInvisible() => _becameInvisible.OnNext(this);

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _becameVisible.OnCompleted();
            _becameVisible.Dispose();
            _becameInvisible.OnCompleted();
            _becameInvisible.Dispose();
        }

        #endregion

      
    }
}