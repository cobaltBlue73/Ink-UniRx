using System;
using System.Linq;
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
    public class StoryTextView : MonoBehaviour
    {
        #region Inpsector

        [SerializeField, Required, InlineEditor] private TMP_Text textMesh;
        [SerializeField, InlineEditor] private TextMeshAnimator textAnimator;
        #endregion
        
        #region Properties
        public TMP_Text TextMesh => textMesh;
        
        public TextMeshAnimator TextAnimator => textAnimator;

        public string Text
        {
            get => textMesh.text;
            set => textMesh.text = value;
        }

        public TextOverflowModes OverflowMode
        {
            get => textMesh.overflowMode;
            set => textMesh.overflowMode = value;
        }

        public bool IsEmpty => string.IsNullOrEmpty(textMesh.text);

        public int MaxVisibleCharacters
        {
            get => textMesh.maxVisibleCharacters;
            set => textMesh.maxVisibleCharacters = value;
        }

        public int CharacterCount => textMesh.textInfo.characterCount;
        

        public IObservable<StoryTextView> WhenRectTransformDimensionsChange =>
            _rectTransformDimensionsChange.AsObservable();

        public IReadOnlyReactiveProperty<bool> IsTextMeshCulled => _isTextMeshCulled ??= 
            textMesh.onCullStateChanged.AsObservable().ToReactiveProperty(false);
        
        #endregion

        #region Variables

        private readonly Subject<StoryTextView> _rectTransformDimensionsChange =
            new Subject<StoryTextView>();

        private IReadOnlyReactiveProperty<bool> _isTextMeshCulled;

        #endregion

        #region Methods
        
        #region Unity Callbacks

        protected virtual void Reset()
        {
            if (!textMesh)
                textMesh = GetComponent<TMP_Text>();

            if (!textAnimator)
                textAnimator = GetComponent<TextMeshAnimator>();
        }

        private void Awake()
        {
            _isTextMeshCulled ??= textMesh.onCullStateChanged
                .AsObservable().ToReactiveProperty(false);
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
        
        public void ClearText() => textMesh.text = string.Empty;

        public void ForceTextUpdate() => textMesh.ForceMeshUpdate(); 

        public UniTask AnimateTextAsync(int fromCharIndex, int toCharIndex,
            CancellationToken animationCancelToken) =>
            textAnimator ? 
                textAnimator.PlayAsync(fromCharIndex, toCharIndex, animationCancelToken) : 
                UniTask.CompletedTask;

        #endregion
        
        #endregion


    }
}