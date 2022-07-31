using Hsinpa.Ranking;
using Hsinpa.WebSocket;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

namespace Hsinpa.Model
{
    public class RankModel
    {
        SocketIOManager _socketIOManager;

        public System.Action<List<TypeStruct.RankStruct>> OnDataUpdateEvent;
        public System.Action<int> OnPlayerCountEvent;

        private Dictionary<string, TypeStruct.RankStruct> _rankStructsDict = new Dictionary<string, TypeStruct.RankStruct>();

        public List<TypeStruct.RankStruct> SortedList => _rankStructsDict.Values.ToList().OrderByDescending(x => x.Value).ToList();
        public bool Stop_receive_net_message = false;

        public RankModel(SocketIOManager socketIOManager) {
            _socketIOManager = socketIOManager;

            _socketIOManager.socket.On<string>(TypeStruct.SocketEvent.UpdateUserInfo, OnUpdateUserSocketEvent);

            _socketIOManager.socket.On<string>(TypeStruct.SocketEvent.Disconnect, OnPlayerCountUpdate);
            _socketIOManager.socket.On<string>(TypeStruct.SocketEvent.UserJoined, OnPlayerCountUpdate);
            _socketIOManager.socket.On<string>(TypeStruct.SocketEvent.UserCountSync, OnPlayerCountUpdate);
        }

        private void OnUpdateUserSocketEvent(string json_string) {
            if (Stop_receive_net_message) return;

            Debug.Log(json_string);

            TypeStruct.UserComponentType userComponentType = JsonUtility.FromJson<TypeStruct.UserComponentType>(json_string);

            if (_rankStructsDict.TryGetValue(userComponentType.socket_id, out var rankStruct))
            {
                rankStruct.SetValue(userComponentType.score);
            }
            else {
                TypeStruct.RankStruct newRankItem = new TypeStruct.RankStruct();
                newRankItem.name = userComponentType.name;
                newRankItem.SetValue(userComponentType.score);
                newRankItem.id = userComponentType.socket_id;

                _rankStructsDict.Add(userComponentType.socket_id, newRankItem);
            }

            var sortedScores = _rankStructsDict.Values.ToList().OrderByDescending(x => x.Value).ToList();

            if (OnDataUpdateEvent != null)
                OnDataUpdateEvent(sortedScores);
        }


        private void OnPlayerCountUpdate(string json_string)
        {
            var json = SimpleJSON.JSON.Parse(json_string);

            int p_count = Hsinpa.Utility.UtilityFunc.SafeOperation<int>(json, 0, (x) => x["count"].AsInt);

            if (OnPlayerCountEvent != null) OnPlayerCountEvent(p_count);
        }

        public void Dispose() {
            Debug.Log("Dispose");
            _rankStructsDict.Clear();
        }

    }
}
