using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hsinpa.Ranking;
using System.Linq;
using Hsinpa.Model;
using Hsinpa.UI;
using System.Threading.Tasks;

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
            _socketIOManager.OnSocketConnectEvent += (string id) => {
                _socketIOManager.Emit(TypeStruct.SocketEvent.Request_UserCountSync);
            };
            _rankModel = new RankModel(_socketIOManager);

            //var g_structs = AlgorithmTesting.GenQuickSortData(20);
            //g_structs = g_structs.OrderByDescending(x => x.Value).ToList();

            rankingView.PrepareItemView();
            //rankingView.SetRankingData(g_structs);

            //int total_score = g_structs.Sum(x => x.Value);
            //totalScoreText.text = string.Format(TypeStruct.StaticText.TotalScore, total_score);

            _rankModel.OnDataUpdateEvent += OnRankDataUpdate;
            _rankModel.OnPlayerCountEvent += OnPlayerCountUpdate;

            simpleCanvasView.Front_Page.SetStartBtnAction(OnStartBtnClick);
            simpleCanvasView.Main_Page.OnTimeUpEvent += OnTimeup;
            simpleCanvasView.Rank_Page.SetRestartAction(() => simpleCanvasView.ShowFrontPage());
        }

        private void OnPlayerCountUpdate(int p_count)
        {
            simpleCanvasView.Front_Page.SetPlayerCount(p_count);

        }

        private void OnRankDataUpdate(List<TypeStruct.RankStruct> rankStructs)
        {
            rankingView.SetRankingData(rankStructs);

            int total_score = rankStructs.Sum(x => x.Value);
            totalScoreText.text = string.Format(TypeStruct.StaticText.TotalScore, total_score);
        }

        private void OnGameStartEvent(string raw_json_string) {
            Debug.Log(raw_json_string);

            rankingView.ResetAll();

            TypeStruct.RoomComponentType roomStruct = JsonUtility.FromJson<TypeStruct.RoomComponentType>(raw_json_string);
            simpleCanvasView.Main_Page.SetTimer(roomStruct.end_time);
            simpleCanvasView.Main_Page.ShowStart(false);
            simpleCanvasView.Main_Page.ShowTimeUp(false);

            _rankModel.Stop_receive_net_message = false;
        }

        private async void OnTimeup() {
            simpleCanvasView.Main_Page.ShowTimeUp(true);
            _socketIOManager.Emit(TypeStruct.SocketEvent.TerminateGame);
            _rankModel.Stop_receive_net_message = true;

            await Task.Delay(System.TimeSpan.FromSeconds(3));

            simpleCanvasView.ShowEndTransitionPage();

            await Task.Delay(System.TimeSpan.FromSeconds(5));

            simpleCanvasView.ShowRankPage();
            simpleCanvasView.Rank_Page.SetRanking(_rankModel.SortedList);
            _rankModel.Dispose();
        }

        private async void OnStartBtnClick() {

            rankingView.ResetAll();
            simpleCanvasView.Main_Page.ResetTimer();
            simpleCanvasView.ShowMainPage();
            simpleCanvasView.Main_Page.ShowStart(true);

            await Task.Delay(System.TimeSpan.FromSeconds(3));

            _socketIOManager.Emit(TypeStruct.SocketEvent.StartGame);
            //simpleCanvasView.ShowMainPage();
            totalScoreText.text = string.Format(TypeStruct.StaticText.TotalScore, 0);

            //simpleCanvasView.Main_Page.SetTimer();
        }


    }
}
