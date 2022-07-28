using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Hsinpa.Ranking;

namespace Hsinpa.UI
{
    public class MainPageView : MonoBehaviour
    {
        [SerializeField]
        TextMeshProUGUI timer;

        private DateTime _endTimeStamp;
        private DateTime _startTimeStamp;

        public System.Action OnTimeUpEvent;

        public void SetTimer(long endTimestamp) {
            this._endTimeStamp = DateTimeOffset.FromUnixTimeMilliseconds(endTimestamp).DateTime;
            this._startTimeStamp = DateTime.UtcNow;

            timer.text = string.Format(TypeStruct.StaticText.Timer, "00");
        }

        private void Update()
        {
            if (this._endTimeStamp == DateTime.MinValue) return;

            TimeSpan t = this._endTimeStamp - DateTime.UtcNow;

            timer.text = string.Format(TypeStruct.StaticText.Timer,t.Seconds);

            if (t.Seconds < 0)
            {
                Debug.Log("Teacher : Time up");

                if (OnTimeUpEvent != null) OnTimeUpEvent();

                this._endTimeStamp = DateTime.MinValue;
            }


        }

    }
}