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


        public RankModel(SocketIOManager socketIOManager) {
            _socketIOManager = socketIOManager;

            _socketIOManager.socket.On<TypeStruct.UserComponentType>(TypeStruct.SocketEvent.UpdateUserInfo, OnUpdateUserSocketEvent);
        }

        private void OnUpdateUserSocketEvent(TypeStruct.UserComponentType userComponentType) {

            if (_rankStructsDict.TryGetValue(userComponentType.socket_id, out var rankStruct))
            {
                rankStruct.SetValue(rankStruct.Value);
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

    }
}
