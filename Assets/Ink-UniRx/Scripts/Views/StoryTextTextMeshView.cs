using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using InkUniRx.Animators;
using Sirenix.OdinInspector;
using TMPro;
using UniRx;
using UnityEngine;

namespace InkUniRx.Views
{
    [RequireComponent(typeof(TMP_Text))]
    public class StoryTextTextMeshView : StoryTextView
    {
        #region Inpsector

        [SerializeField, Required, InlineEditor] private TMP_Text textMesh;
        [SerializeField] private TextAnimator[] textAnimators;
        #endregion
        
        #region Properties
        public TMP_Text TextMesh => textMesh;

        public override string Text => textMesh.text;

        public override bool IsEmpty => textMesh.text.Length <= 0;

        public int VisibleCharacters
        {
            get => textMesh.maxVisibleCharacters;
            set => textMesh.maxVisibleCharacters = value;
        }

        public int CharacterCount => textMesh.textInfo.characterCount;

        public IObservable<StoryTextTextMeshView> WhenRectTransformDimensionsChange =>
            _rectTransformDimensionsChange.AsObservable();
        
        #endregion

        #region Variables

        private readonly Subject<StoryTextTextMeshView> _rectTransformDimensionsChange =
            new Subject<StoryTextTextMeshView>();
        
        #endregion

        #region Methods
        
        #region Unity Callbacks

        protected virtual void Reset()
        {
            if (!textMesh)
                textMesh = GetComponent<TMP_Text>();
            
            if (textAnimators == null || textAnimators.Length <= 0)
                textAnimators = GetComponents<TextAnimator>();
        }
        
        private void OnRectTransformDimensionsChange() => _rectTransformDimensionsChange.OnNext(this);

        protected virtual void OnDisable() => ClearText();

        protected virtual void OnDestroy()
        {
            _rectTransformDimensionsChange.OnCompleted();
            _rectTransformDimensionsChange.Dispose();
        }

        #endregion
        
        #region Public
        
        public override void ClearText() => textMesh.text = string.Empty;

        public override void AddText(string text) => textMesh.text += text;

        public UniTask PlayTextAnimationsAsync(int fromCharIndex, int toCharIndex, CancellationToken cancelAnimationToken) =>
            UniTask.WhenAll(textAnimators.Select(animator => 
                animator.PlayTextAnimationAsync(fromCharIndex, toCharIndex, cancelAnimationToken)));

        #endregion
        
        #endregion


    }
}