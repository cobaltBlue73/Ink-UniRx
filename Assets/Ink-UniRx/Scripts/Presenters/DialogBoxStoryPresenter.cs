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
using Utility.UniRx;

namespace InkUniRx
{
    public class DialogBoxStoryPresenter : MonoBehaviour, IStoryPathBeginningPresenter, IStoryTextPresenter, IStoryPathEndingPresenter
    {
        [SerializeField] private StoryPresenterSettings settings;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Button continueButton;
        [SerializeField] private bool trim;
        
        private IObservable<TMP_LinkInfo> _whenLinkClicked;
        private bool _autoContinue;
        private float _autoContinueDelay;
        private float _textAnimSpd;
        private Ease _textAnimEase;

        private void Awake()
        {
            text.overflowMode = TextOverflowModes.Page;
            _whenLinkClicked = text.textInfo.OnPointerClickedLinkAsObservable();

            if (settings)
            {
                settings.AutoContinue.SetAndSubscribe(ref _autoContinue,
                    val => _autoContinue = val).AddTo(this);
                settings.AutoContinueDelay.SetAndSubscribe(ref _autoContinueDelay,
                    val => _autoContinueDelay = val).AddTo(this);

                settings.TextAnimationSpeed.SetAndSubscribe(ref _textAnimSpd,
                    val => _textAnimSpd = val).AddTo(this);
                settings.TextAnimationEasing.SetAndSubscribe(ref _textAnimEase,
                    ease => _textAnimEase = ease);
            }
        }

        private void OnPathEnd()
        {
           
        }

        /*private void OnLinkClicked(TMP_LinkInfo info)
        {
            var choiceIndex = int.Parse(info.GetLinkID());
            _story?.ChooseChoiceIndex(choiceIndex);
        }*/

        private async UniTask AnimateTextAsync(CancellationToken ct)
        {
            text.maxVisibleCharacters = text.textInfo.characterCount;
            var pageInfo = text.textInfo.pageInfo[text.pageToDisplay - 1];
            var from =  pageInfo.firstCharacterIndex;
            var to = text.pageToDisplay >= text.textInfo.pageCount? 
                text.textInfo.characterCount: pageInfo.lastCharacterIndex + 1;

            text.maxVisibleCharacters = from;
            var tween = text.DOVisibleCharacters(to, _textAnimSpd)
                .SetEase(_textAnimEase).SetSpeedBased().SetRecyclable(); 
            await tween.WithCancellation(ct).SuppressCancellationThrow();
            
            if (ct.IsCancellationRequested)
            {
                text.maxVisibleCharacters = text.textInfo.characterCount;
            }
        }
        
        public UniTask OnShowPathBeginningAsync(Story story, CancellationToken ct)
        {
            continueButton.gameObject.SetActive(true);
            return UniTask.CompletedTask;
        }

        public async UniTask OnShowStoryTextAsync(Story story, CancellationToken ct)
        {
            text.text = trim? story.currentText.Trim(): story.currentText;
            text.ForceMeshUpdate();

            for (int page = 1; page <= text.textInfo.pageCount && !ct.IsCancellationRequested; page++)
            {
                text.pageToDisplay = page;
                await AnimateTextAsync(ct);
                
                if (!story.canContinue && text.pageToDisplay >= text.textInfo.pageCount) break;

                if (!_autoContinue)
                {
                    await continueButton.OnClickAsync(ct);
                    continue;
                }

                await UniTask.WhenAny(UniTask.Delay(TimeSpan.FromSeconds(_autoContinueDelay),
                        false, PlayerLoopTiming.Update, ct),
                    continueButton.OnClickAsync(ct));

            }
        }

        public async UniTask OnShowPathEndingAsync(Story story, CancellationToken ct)
        {
            continueButton.gameObject.SetActive(false);
            for (int i = 0; i < story.currentChoices.Count; i++)
            {
                var choice = story.currentChoices[i];
                text.text += $"\n<align=center><link={choice.index}>{choice.text}</link></align>";
            }
            text.ForceMeshUpdate();
            text.maxVisibleCharacters = text.textInfo.characterCount;
            var linkInfo = await _whenLinkClicked.ToUniTask(true);
            story.ChooseChoiceIndex(int.Parse(linkInfo.GetLinkID()));
        }
    }
}