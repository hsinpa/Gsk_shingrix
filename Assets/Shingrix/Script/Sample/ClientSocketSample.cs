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
        private const string domain = "http://34.170.230.188:81";

        public ClientSocketSample()
        {
            display_name = System.Guid.NewGuid().ToString().Substring(0, 8);

            Uri uri = new Uri(domain);
            _socketManager = new SocketManager(uri);
            
            socket.On("connect", () =>
            {
                EmitJoinRoom(display_name);
            });

            socket.On("event@start_game", EmitStartGameEvent);
            socket.On("event@end_game", EmitEndGameEvent);
        }

        #region RESTFUL
        /// <summary>
        /// If -1 => socket id is not exist
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetRanking() {
            var request = new HTTPRequest(new Uri(domain + "/rank/"+ socket_id));

            var jsonstr = await request.GetAsStringAsync();
            var json = SimpleJSON.JSON.Parse(jsonstr);
            return json["rank"].AsInt;
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
        private void EmitStartGameEvent()
        {
            //Do something here
        }

        private void EmitEndGameEvent()
        {
            //Do something here

            //Register again, if user want to restart next round right away
            EmitJoinRoom(display_name);
        }
        #endregion

    }
}
