using BestHTTP;
using BestHTTP.SocketIO3;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Hsinpa.ClientSample {

    public class ClientSocketSample
    {
        private SocketManager _socketManager;
        public Socket socket => (_socketManager == null) ? null : _socketManager.Socket;

        public string socket_id => _socketManager.Handshake.Sid;

        public bool IsConnected => (socket != null && socket.IsOpen);

        public System.Action<string> OnSocketConnectEvent;

        private string display_name;
        private const string domain = "https://34.170.230.188/";

        public ClientSocketSample()
        {
            display_name = System.Guid.NewGuid().ToString().Substring(0, 8);

            Uri uri = new Uri(domain);
            _socketManager = new SocketManager(uri);
            
            socket.On("connect", () =>
            {
                EmitJoinRoom(display_name);
            });

            socket.On<string>("event@start_game", EmitStartGameEvent);
            socket.On("event@end_game", EmitEndGameEvent);
        }

        #region RESTFUL
        /// <summary>
        /// If -1 => socket id is not exist
        /// </summary>
        /// <returns></returns>
        public void GetRanking(int game_id, System.Action<int> callback) {
            var request = new HTTPRequest(new Uri(domain + "rank/"+ socket_id +"/"+ game_id), HTTPMethods.Get, callback: (HTTPRequest req, HTTPResponse resp) => 
                {
                    var jsonstr = resp.DataAsText;
                    var json = SimpleJSON.JSON.Parse(jsonstr);
                    callback(json["rank"].AsInt);
                }
            );

            request.Send();
        }

        public void SendFeedback(string display_name, string message)
        {
            var request = new HTTPRequest(new Uri(domain+ "report_feedback"), HTTPMethods.Post);
            request.AddHeader("Content-Type", "application/json");

            var json = new
            {
                feedback = message,
                name = display_name,
            };

            request.RawData = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(json));
            request.Send();
        }

        #endregion

        #region Emit
        private void EmitJoinRoom(string p_name) {
            socket.Emit("event@on_join_game", JsonConvert.SerializeObject(new
            {
                name = p_name,
            }));
        }

        private void EmitScore(int total_score)
        {
            socket.Emit("event@on_score", JsonConvert.SerializeObject(new
            {
                score = total_score,
            }));
        }
        #endregion

        #region OnSocket Event
        private void EmitStartGameEvent(string raw_json_string)
        {
            //Do something here
            RoomComponentType roomStruct = JsonUtility.FromJson<RoomComponentType>(raw_json_string);
        }

        private void EmitEndGameEvent()
        {
            //Do something here

            //Register again, if user want to restart next round right away
            EmitJoinRoom(display_name);
        }
        #endregion


        [System.Serializable]
        public struct RoomComponentType
        {
            public string room_id;
            public string host_id;
            public int game_id;
            public long start_time;
            public long end_time;
        }
    }
}
