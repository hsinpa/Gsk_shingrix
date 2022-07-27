using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hsinpa.UI
{
    public class SimpleCanvasView : MonoBehaviour
    {
        [SerializeField]
        private FrontPageView front_page;
        public FrontPageView Front_Page => front_page;

        [SerializeField]
        private MainPageView main_page;
        public MainPageView Main_Page => main_page;

        public void ShowFrontPage() {
            main_page.gameObject.SetActive(false);
            front_page.gameObject.SetActive(true);
        }

        public void ShowMainPage()
        {
            main_page.gameObject.SetActive(true);
            front_page.gameObject.SetActive(false);
        }

    }
}