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

        private Dictionary<string, TypeStruct.RankStruct> _rankStructsDict = new Dictionary<string, TypeStruct.RankStruct>();

        public List<TypeStruct.RankStruct> SortedList => _rankStructsDict.Values.ToList().OrderByDescending(x => x.Value).ToList();

        public RankModel(SocketIOManager socketIOManager) {
            _socketIOManager = socketIOManager;

            _socketIOManager.socket.On<string>(TypeStruct.SocketEvent.UpdateUserInfo, OnUpdateUserSocketEvent);
        }

        private void OnUpdateUserSocketEvent(string json_string) {
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

        public void Dispose() {
            _rankStructsDict.Clear();
        }

    }
}
