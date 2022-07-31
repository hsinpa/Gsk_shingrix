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

        [SerializeField]
        private GameObject end_transition_page;
        public GameObject End_Transition_Page => end_transition_page;

        [SerializeField]
        private RankPageView rank_page;
        public RankPageView Rank_Page => rank_page;

        public void ShowFrontPage() {
            main_page.gameObject.SetActive(false);
            front_page.gameObject.SetActive(true);
            rank_page.gameObject.SetActive(false);
            end_transition_page.gameObject.SetActive(false);
        }

        public void ShowMainPage()
        {
            main_page.gameObject.SetActive(true);
            front_page.gameObject.SetActive(false);
            rank_page.gameObject.SetActive(false);
            end_transition_page.gameObject.SetActive(false);
        }

        public void ShowEndTransitionPage()
        {
            main_page.gameObject.SetActive(false);
            front_page.gameObject.SetActive(false);
            rank_page.gameObject.SetActive(false);
            end_transition_page.gameObject.SetActive(true);
        }

        public void ShowRankPage()
        {
            main_page.gameObject.SetActive(false);
            front_page.gameObject.SetActive(false);
            end_transition_page.gameObject.SetActive(false);
            rank_page.gameObject.SetActive(true);
        }
    }
}