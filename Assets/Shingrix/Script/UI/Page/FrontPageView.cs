using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Hsinpa.Ranking;

namespace Hsinpa.UI
{
    public class FrontPageView : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI WaitPersonText;

        [SerializeField]
        private Button StartBtn;

        public Hsinpa.View.Tab Tab;

        public void SetStartBtnAction(System.Action callback)
        {
            Hsinpa.Utility.UtilityFunc.SetSimpleBtnEvent(this.StartBtn, callback);
        }

        public void SetPlayerCount(int count)
        {
            WaitPersonText.text = string.Format(TypeStruct.StaticText.FrontPageWaitingPerson, count);
        }
    }
}