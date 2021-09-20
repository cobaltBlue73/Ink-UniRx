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
    public class StoryTextSingleTextScrollView: StoryTextScrollView
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
            
        }

        private void Start()
        {
            
        }

        #endregion

        #region Public

        public override void ClearText() => textMesh.text = string.Empty;

        public override async UniTask AddTextAsync(string text, CancellationToken cancelAnimationToken)
        {
            var prevCharCount = textMesh.textInfo.characterCount;
            textMesh.text += $"{text}\n";
            //textMesh.ForceMeshUpdate();
            var curCharCount = textMesh.textInfo.characterCount;

            var textAnimationTask = UniTask.WhenAll(textAnimators.Select(animator =>
                animator.PlayTextAnimationAsync(prevCharCount, curCharCount - 1,
                    cancelAnimationToken)));

            await UniTask.WhenAll(textAnimationTask, ScrollToBottomAsync(cancelAnimationToken));
        }

        #endregion

        #region Private

        

        #endregion

        #endregion
        
        
     
      
    }
}