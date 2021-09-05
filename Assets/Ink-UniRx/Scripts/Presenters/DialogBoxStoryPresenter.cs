using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using DG.Tweening;
using Ink.Runtime;
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
    public class DialogBoxStoryPresenter : MonoBehaviour, INewLineTransition, IStoryContinueOverride
    {
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Button continueButton;
        [SerializeField] private StoryPlayer storyPlayer;
        [SerializeField] private bool trim;

        private Story _story;
        private CancellationTokenSource _animationCts;
        
        private void Awake()
        {
            text.overflowMode = TextOverflowModes.Page;
            storyPlayer.SetStoryContinueOverride(this);   
            storyPlayer.SubmitNewLineTransition(this);
            continueButton.OnClickAsObservable().Subscribe(_ => OnContinue()).AddTo(this);
            storyPlayer.WhenStoryBegins.Subscribe(_ => OnStoryBegin()).AddTo(this);
            storyPlayer.WhenNewLine.Subscribe(_=> OnNewLine()).AddTo(this);
            storyPlayer.WhenPathBegins.Subscribe(_ => OnPathBegin()).AddTo(this);
            storyPlayer.WhenPathEnds.Subscribe(_ => OnPathEnd()).AddTo(this);
            text.textInfo.OnPointerClickedLinkAsObservable().Subscribe(OnLinkClicked).AddTo(this);
        }

        private void OnDestroy()
        {
            _animationCts?.Dispose();
        }

        private void OnStoryBegin()
        {
            _story = storyPlayer.Story;
        }

        private void OnNewLine()
        {
            Debug.Log(nameof(OnNewLine));
            text.text = trim? _story.currentText.Trim(): _story.currentText;
            text.pageToDisplay = 1;
        }

        private void OnPathBegin()
        {
            continueButton.gameObject.SetActive(true);
        }

        private void OnPathEnd()
        {
            Debug.Log(nameof(OnPathEnd));
            continueButton.gameObject.SetActive(false);
            for (int i = 0; i < _story.currentChoices.Count; i++)
            {
                var choice = _story.currentChoices[i];
                text.text += $"\n<align=center><link={choice.index}>{choice.text}</link></align>";
            }
        }

        private void OnLinkClicked(TMP_LinkInfo info)
        {
            var choiceIndex = int.Parse(info.GetLinkID());
            storyPlayer.SelectChoice(choiceIndex);
        }

        private void OnContinue()
        {
            _animationCts?.Cancel();
            storyPlayer.ContinueStory();
        }

        private async UniTask AnimateTextAsync(CancellationToken ct)
        {
            // Debug.Log(nameof(AnimateTextAsync));
            // var pageIdx = text.pageToDisplay - 1;
            // var pageInfo = text.textInfo.pageInfo[pageIdx];
            // Debug.LogFormat("Fist: {0}, Last: {1}", pageInfo.firstCharacterIndex, pageInfo.lastCharacterIndex);
            // text.maxVisibleCharacters = pageInfo.firstCharacterIndex;
            // var tween = text.DOVisibleCharacters(pageInfo.lastCharacterIndex + 1, 60f)
            //     .SetSpeedBased().Pause();
            // var canceled  = await tween.Play().WithCancellation(ct).SuppressCancellationThrow();
            //
            // if (canceled) tween.Complete();
            await UniTask.WaitForEndOfFrame();
        }

        public async UniTask PlayNewLineTransitionAsync(CancellationToken ct)
        {
            await AnimateTextAsync(ct);
        }
        
        public async UniTask WaitForContinueAsync(Func<UniTask> defaultWaitForContinue)
        {
            await defaultWaitForContinue();
            
            while (text.pageToDisplay < text.textInfo.pageCount)        
            {
                ++text.pageToDisplay;
                using (_animationCts = new CancellationTokenSource())
                {
                    await AnimateTextAsync(_animationCts.Token);
                }
                _animationCts = null;

                await defaultWaitForContinue();
            }
        }
    }
}