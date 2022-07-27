using Hsinpa.Algorithm;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hsinpa.Ranking {
    public class TypeStruct
    {
        public class RankStruct : ISort
        {
            public string name;
            public string id;

            public int Index => _index;

            public int Value => _value;
            private int _value;

            private int _index;

            public void SetIndex(int index)
            {
                _index = index;
            }

            public void SetValue(int value)
            {
                _value = value;
            }
        }

        [System.Serializable]
        public struct UserComponentType
        {
            public string socket_id;
            public string name;
            public int score;
            public int? user_type;
        }

        public class SocketEvent
        {
            public const string OnConnect = "connect";
            public const string UpdateUserInfo = "event@update_user";
            public const string UserJoined = "event@user_joined";
            public const string RefreshUserStatus = "event@refresh_user_status";
            public const string UserLeaved = "event@user_leave";
            public const string CreateRoom = "event@create_room";

            public const string StartGame = "event@on_start_game";
            public const string TerminateGame = "event@on_end_game";

            public const string Reconnect = "event@reconnect";
            public const string Disconnect = "event@disconnect";
        }

        public class URL
        {
            public const string SocketDev = "http://localhost:3000";
            public const string SocketProduction = "event@update_userInfo";
        }

        public class StaticText {
            public const string TotalScore = "每分鐘全場以協助<br><b><size=82><color=#B50003>{0}</color></size></b>人<br>預防帶狀皰疹發生";
        }
    }
}