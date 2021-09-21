using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utility.General;

namespace InkUniRx.Views
{
    public class StoryTextSingleElementScrollView: StoryTextScrollView
    {
        #region Inspector

        [SerializeField, InlineEditor] private TMP_Text textMesh;
        [SerializeField, InlineEditor] private LayoutElement textLayoutElement;

        #endregion

        #region Properties

        public override string Text => textMesh.text;
        public override bool IsEmpty => Text.Length <= 0;

        #endregion

        #region Variables

        private int _prevCharCount;

        #endregion

        #region Methods

        #region Unity Callbacks

        protected override void Reset()
        {
            base.Reset();

            if (!textMesh)
                textMesh = GetComponentInChildren<TMP_Text>();

            if (textMesh && !textLayoutElement)
                textLayoutElement = textMesh.GetOrAddComponent<LayoutElement>();
        }
        
        private void Awake()
        {
            ClearText();
        }

        private void Start()
        {
            
        }

        #endregion

        #region Public

        public override void ClearText()
        {
            _prevCharCount = 0;
            textMesh.text = string.Empty;
        }

        public override void AddText(string text)
        {
            _prevCharCount = textMesh.textInfo.characterCount;
            textMesh.text += IsEmpty? text: $"\n{text}";
            textMesh.ForceMeshUpdate();
            textMesh.maxVisibleCharacters = _prevCharCount;
        }
        
        public override async UniTask ShowNewTextAsync(CancellationToken cancelAnimationToken)
        {
            var animationTasks = UniTask.WhenAll(textAnimators.Select(animator =>
                animator.PlayTextAnimationAsync(textMesh, _prevCharCount, 
                    textMesh.textInfo.characterCount - 1,
                    cancelAnimationToken)));

            await UniTask.WhenAll(animationTasks, ScrollToBottomAsync(cancelAnimationToken));
        }


        #endregion

        #region Private

        

        #endregion

        #endregion
        
        
     
      
    }
}