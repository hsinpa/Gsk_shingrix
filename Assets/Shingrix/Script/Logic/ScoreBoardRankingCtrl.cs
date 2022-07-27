using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hsinpa.Ranking;
using System.Linq;
using Hsinpa.Model;
using Hsinpa.UI;

namespace Hsinpa.Ctrl {
    public class ScoreBoardRankingCtrl : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField]
        private SimpleCanvasView simpleCanvasView;

        [SerializeField]
        private RankingView rankingView;

        [SerializeField]
        private TMPro.TextMeshProUGUI totalScoreText;

        private WebSocket.SocketIOManager _socketIOManager;
        private RankModel _rankModel;
        void Start()
        {

            simpleCanvasView.ShowFrontPage();

            _socketIOManager = new WebSocket.SocketIOManager(new System.Uri(TypeStruct.URL.SocketDev));
            _rankModel = new RankModel(_socketIOManager);

            var g_structs = AlgorithmTesting.GenQuickSortData(20);
            g_structs = g_structs.OrderByDescending(x => x.Value).ToList();

            rankingView.PrepareItemView();
            rankingView.SetRankingData(g_structs);

            int total_score = g_structs.Sum(x => x.Value);
            totalScoreText.text = string.Format(TypeStruct.StaticText.TotalScore, total_score);

            _rankModel.OnDataUpdateEvent += OnRankDataUpdate;

            simpleCanvasView.Front_Page.SetStartBtnAction(OnStartBtnClick);
        }

        private void OnRankDataUpdate(List<TypeStruct.RankStruct> rankStructs) {
            rankingView.SetRankingData(rankStructs);

            int total_score = rankStructs.Sum(x => x.Value);
            totalScoreText.text = string.Format(TypeStruct.StaticText.TotalScore, total_score);
        }

        private void OnStartBtnClick() {
            _socketIOManager.Emit(TypeStruct.SocketEvent.StartGame);
            simpleCanvasView.ShowMainPage();
        }


    }
}
