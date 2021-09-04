using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using Utility.DoTweenPro;
using Utility.TextMeshPro;

namespace InkUniRx
{
    public class DialogBoxStoryPresenter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Button continueButton;
        [SerializeField] private StoryPlayer storyPlayer;

        private Tweener _tweener;
        private DOTweenTMPAnimator _tmpAnimator;
        private async void Start()
        {
            continueButton.OnClickAsObservable().Subscribe(_ => ShowNextPage()).AddTo(this);
            text.textInfo.OnPointerEnterWordAsObservable().Subscribe(info => Debug.LogFormat("Enter {0}", info.GetWord()));
            text.textInfo.OnPointerExitWordAsObservable().Subscribe(info => Debug.LogFormat("Exit {0}", info.GetWord()));
            text.textInfo.OnPointerClickedWordAsObservable().Subscribe(info => Debug.LogFormat("Clicked {0}", info.GetWord()));
            await UniTask.WaitForEndOfFrame();
            TweenNext();
        }

        private void ShowNextPage()
        {
            if (_tweener != null && _tweener.IsPlaying())
            {
                _tweener.Complete();
                _tweener = null;
                return;
            }
            var curPageIdx = text.pageToDisplay - 1;
            curPageIdx = (curPageIdx + 1) % text.textInfo.pageCount;
            text.pageToDisplay = curPageIdx + 1;
            
            TweenNext();
        }

        private void TweenNext()
        {
            text.textInfo.GetFirstAndLastWordIndexOnPage(text.pageToDisplay - 1, out var firstWordIndex, out var lastWordIndex);
            Debug.LogFormat("First {0}, Last {1}", firstWordIndex, lastWordIndex);
            text.maxVisibleWords = firstWordIndex;
            _tweener = text.DOVisibleWords(lastWordIndex + 1, 30f)
                .SetEase(Ease.Linear).SetSpeedBased().SetRecyclable().OnKill(()=> _tweener = null);
        }
        
    }
}