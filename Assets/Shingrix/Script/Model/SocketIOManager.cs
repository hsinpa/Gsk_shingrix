using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BestHTTP;
using System;
using BestHTTP.SocketIO3;
using Hsinpa.Ranking;

namespace Hsinpa.WebSocket {
    public class SocketIOManager
    {
        public delegate void OnSocketEventDelegate(string event_id, string rawData);

        private SocketManager _socketManager;
        public Socket socket => (_socketManager == null) ? null : _socketManager.Socket;

        public string socket_id => _socketManager.Handshake.Sid;

        public bool IsConnected => (socket != null && socket.IsOpen);

        public SocketIOManager(Uri uri)
        {
            _socketManager = new SocketManager(uri);
            RegisterSocket(_socketManager);
        }

        private void RegisterSocket(SocketManager manager) {
            manager.Socket.On(TypeStruct.SocketEvent.OnConnect, OnConnectEvent);
        }

        void OnConnectEvent()
        {
            Debug.Log("Original Socket " + socket_id);
        }

        public void Emit(string event_id, string raw_json = "{}") {
            Debug.Log("Emit event_id " + event_id + ",raw_json " + raw_json +", Open " + socket.IsOpen);

            socket.Emit(event_id, raw_json);
        }

        private struct EmitStruct {
            public string event_id;
            public string json;

            public EmitStruct(string p_event, string p_json) {
                this.event_id = p_event;
                this.json = p_json;
            }
        }
    }
}