using System;
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
    public class DialogBoxStoryPresenter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Button continueButton;
        [SerializeField] private StoryPlayer storyPlayer;
        [SerializeField] private bool trim;

        private Story _story;
        
        private void Awake()
        {
            text.overflowMode = TextOverflowModes.Page;
            continueButton.OnClickAsObservable().Subscribe(_ => OnContinue()).AddTo(this);
            storyPlayer.WhenStoryBegins.Subscribe(_ => OnStoryBegin());
            storyPlayer.WhenNewLine.Subscribe(_=> OnNewLine()).AddTo(this);
            storyPlayer.WhenPathBegins.Subscribe(_ => OnPathBegin()).AddTo(this);
            storyPlayer.WhenPathEnds.Subscribe(_ => OnPathEnd()).AddTo(this);
            text.textInfo.OnPointerClickedLinkAsObservable().Subscribe(OnLinkClicked).AddTo(this);
        }

        private void OnStoryBegin()
        {
            _story = storyPlayer.Story;
        }

        private void OnNewLine()
        {
            text.text = trim? _story.currentText.Trim(): _story.currentText;
        }

        private void OnPathBegin()
        {
            continueButton.gameObject.SetActive(true);
        }

        private void OnPathEnd()
        {
            continueButton.gameObject.SetActive(false);
            for (int i = 0; i < _story.currentChoices.Count; i++)
            {
                var choice = _story.currentChoices[i];
                text.text += $"\n<align=center><link={choice.index}>{choice.text}</link></align>";
            }
        }

        private void OnLinkClicked(TMP_LinkInfo linkInfo)
        {
            var choiceIdx = int.Parse(linkInfo.GetLinkID());
            storyPlayer.SelectChoice(choiceIdx);
        }

        private void OnContinue()
        {
            if (text.pageToDisplay < text.textInfo.pageCount)
            {
                ++text.pageToDisplay;
                return;
            }
            storyPlayer.ContinueNext();
        }
        
    }
}