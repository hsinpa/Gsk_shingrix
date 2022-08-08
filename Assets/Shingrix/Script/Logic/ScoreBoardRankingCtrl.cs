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

        [SerializeField]
        private TMPro.TextMeshProUGUI totalScoreTextEnd;

        [SerializeField]
        private UnityEngine.UI.Button restartBtn;

        [SerializeField]
        private UnityEngine.Video.VideoPlayer video_player;

        private WebSocket.SocketIOManager _socketIOManager;
        private RankModel _rankModel;

        [Header("Video Configs")]
        [SerializeField]
        private float video_speed_scaler = 2;
        private float _dy_video_speed_scaler;

        [SerializeField]
        private float max_video_speed = 2;

        [SerializeField]
        private float expected_video_targets = 150;

        void Start()
        {
            simpleCanvasView.ShowFrontPage();

            _socketIOManager = new WebSocket.SocketIOManager(new System.Uri(TypeStruct.URL.SocketProd));
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

            Hsinpa.Utility.UtilityFunc.SetSimpleBtnEvent(restartBtn, () => simpleCanvasView.ShowFrontPage());
            video_player.Prepare();

            ResetVideo();
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
            totalScoreTextEnd.text = string.Format(TypeStruct.StaticText.TotalScoreEnd, total_score);

            _dy_video_speed_scaler *= 0.98f;
            _dy_video_speed_scaler = Mathf.Clamp(_dy_video_speed_scaler, 0.5f, _dy_video_speed_scaler);
            video_player.playbackSpeed = Mathf.Clamp((total_score / expected_video_targets) * _dy_video_speed_scaler, 0 , max_video_speed);
        }

        private void OnGameStartEvent(string raw_json_string) {
            Debug.Log(raw_json_string);

            rankingView.ResetAll();

            video_player.Play();

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

            video_player.Stop();

            await Task.Delay(System.TimeSpan.FromSeconds(3));

            simpleCanvasView.ShowEndTransitionPage();
            simpleCanvasView.Main_Page.ShowTimeUp(false);
            video_player.time = 0;
            //await Task.Delay(System.TimeSpan.FromSeconds(8));
            //simpleCanvasView.Main_Page.ShowTimeUp(false);

            //simpleCanvasView.ShowFrontPage();
            //simpleCanvasView.ShowRankPage();
            //simpleCanvasView.Rank_Page.SetRanking(_rankModel.SortedList);
            //_rankModel.Dispose();
        }

        private async void OnStartBtnClick() {
            ResetVideo();

            rankingView.ResetAll();
            simpleCanvasView.Main_Page.ResetTimer();
            simpleCanvasView.ShowMainPage();
            simpleCanvasView.Main_Page.ShowStart(true);

            totalScoreText.text = string.Format(TypeStruct.StaticText.TotalScore, 0);
            totalScoreTextEnd.text = string.Format(TypeStruct.StaticText.TotalScoreEnd, 0);

            await Task.Delay(System.TimeSpan.FromSeconds(3));

            _socketIOManager.Emit(TypeStruct.SocketEvent.StartGame, "{\"game_id\" : " + simpleCanvasView.Front_Page.Tab.Index + "}");
            //simpleCanvasView.ShowMainPage();

            //simpleCanvasView.Main_Page.SetTimer();
        }

        private void ResetVideo() {
            //Video
            _dy_video_speed_scaler = video_speed_scaler;
            video_player.time = 0;
            video_player.Play();
            video_player.Pause();

            video_player.playbackSpeed = 0;
        }
    }
}
