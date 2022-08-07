using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Hsinpa.View {
    public class Tab : MonoBehaviour
    {
        public Color ActiveColor;
        public Color DisableColor;

        public int Index => _index;
        private int _index = -1;

        private Button[] buttons;

        public System.Action<int> OnTabActivate;

        private void Start()
        {
            buttons = gameObject.GetComponentsInChildren<Button>(includeInactive: true);

            for (int i = 0; i < buttons.Length; i++) {
                int index = i;
                Hsinpa.Utility.UtilityFunc.SetSimpleBtnEvent(buttons[index], () => OnTabClick(index));
            }

            if (buttons.Length > 0)
                SetIndex(0);
        }

        public void SetIndex(int index) {
            _index = index;
            Hsinpa.Utility.UtilityFunc.ListOpt(buttons, (x) => x.image.color = DisableColor);
            buttons[index].image.color = ActiveColor;

            if (OnTabActivate != null) OnTabActivate(index);
        }

        private void OnTabClick(int index) {
            SetIndex(index);
        }
    }
}
