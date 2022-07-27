using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hsinpa.UI
{
    public class FrontPageView : MonoBehaviour
    {
        [SerializeField]
        private Button StartBtn;

        public void SetStartBtnAction(System.Action callback)
        {
            Hsinpa.Utility.UtilityFunc.SetSimpleBtnEvent(this.StartBtn, callback);
        }

    }
}