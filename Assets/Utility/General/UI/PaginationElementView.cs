using System;
using Sirenix.OdinInspector;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Utility.General.UI
{
    [RequireComponent(typeof(Toggle))]
    public class PaginationElementView : MonoBehaviour
    {
        #region Inspector

        [SerializeField, Required] private Toggle toggle;
        [SerializeField] private TMP_Text label;

        #endregion

        #region Properties

        public int PageNo { get; private set; }
        
        public IObservable<int> WhenSelected =>
            Toggle.onValueChanged.AsObservable()
                .Where(val => val).Select(_ => PageNo);

        public Toggle Toggle => toggle;

        public TMP_Text Label => label;

        #endregion

        #region Unity Callback

        private void Reset()
        {
            if (!Toggle)
                toggle = GetComponent<Toggle>();

            if (!Label)
                label = GetComponentInChildren<TMP_Text>();
        }

        #endregion

        #region Public Methods

        public void SetPageNo(int pageNo)
        {
            PageNo = pageNo;
            
            if (Label) Label.text = pageNo.ToString();
        }

        #endregion
    }
}