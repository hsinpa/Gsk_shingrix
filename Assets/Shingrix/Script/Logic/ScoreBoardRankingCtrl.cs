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
            _socketIOManager.socket.On<string>(TypeStruct.SocketEvent.ReceiveStartGame, OnGameStartEvent);

            _rankModel = new RankModel(_socketIOManager);

            //var g_structs = AlgorithmTesting.GenQuickSortData(20);
            //g_structs = g_structs.OrderByDescending(x => x.Value).ToList();

            rankingView.PrepareItemView();
            //rankingView.SetRankingData(g_structs);

            //int total_score = g_structs.Sum(x => x.Value);
            //totalScoreText.text = string.Format(TypeStruct.StaticText.TotalScore, total_score);

            _rankModel.OnDataUpdateEvent += OnRankDataUpdate;

            simpleCanvasView.Front_Page.SetStartBtnAction(OnStartBtnClick);
            simpleCanvasView.Main_Page.OnTimeUpEvent += OnTimeup;
            simpleCanvasView.Rank_Page.SetRestartAction(() => simpleCanvasView.ShowFrontPage());
        }

        private void OnRankDataUpdate(List<TypeStruct.RankStruct> rankStructs)
        {
            rankingView.SetRankingData(rankStructs);

            int total_score = rankStructs.Sum(x => x.Value);
            totalScoreText.text = string.Format(TypeStruct.StaticText.TotalScore, total_score);
        }

        private void OnGameStartEvent(string raw_json_string) {
            Debug.Log(raw_json_string);
            TypeStruct.RoomComponentType roomStruct = JsonUtility.FromJson<TypeStruct.RoomComponentType>(raw_json_string);
            simpleCanvasView.Main_Page.SetTimer(roomStruct.end_time);
            simpleCanvasView.ShowMainPage();
            simpleCanvasView.Main_Page.ShowTimeUp(false);
        }

        private void OnTimeup() {
            simpleCanvasView.Main_Page.ShowTimeUp(true);
            _socketIOManager.Emit(TypeStruct.SocketEvent.TerminateGame);

            _ = Hsinpa.Utility.UtilityFunc.DoDelayWork(5, () =>
            {
                simpleCanvasView.ShowRankPage();
                simpleCanvasView.Rank_Page.SetRanking(_rankModel.SortedList);
            });
        }

        private void OnStartBtnClick() {
            _socketIOManager.Emit(TypeStruct.SocketEvent.StartGame);
            //simpleCanvasView.ShowMainPage();
            totalScoreText.text = string.Format(TypeStruct.StaticText.TotalScore, 0);

            //simpleCanvasView.Main_Page.SetTimer();
        }


    }
}
