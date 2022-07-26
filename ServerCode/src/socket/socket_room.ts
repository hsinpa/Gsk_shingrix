import { RoomComponentType, UserComponentType, UserStatus } from "./socket_struct";
import { SocketEvent, UniversalParameter } from "../Utility/Flag/EventFlag";
import { Server, Socket } from "socket.io";
import { Dictionary } from "typescript-collections";

export default class SocketRoom {

    private m_users :Dictionary<string, UserComponentType> = new Dictionary<string, UserComponentType>();

    private m_io : Server;
    private m_room : RoomComponentType;

    constructor(io: Server) {     
        this.m_io = io;
        this.m_room = {room_id: UniversalParameter.RoomName, host_id: "", start_time: 0, end_time: 0}
    }

    StartGame(id: string)  {
        this.SetRoomTimer(Date.now());
        this.m_room.host_id = id;

        this.m_io.in(UniversalParameter.RoomName).emit(SocketEvent.StartGame,
            JSON.stringify(this.m_room)
        );
    }

    EndGame() {
        this.m_io.in(UniversalParameter.RoomName).emit(SocketEvent.ForceEndGame);
    }

    JoinRoom(socket: Socket, id: string, name: string) {
        let new_user =  {socket_id: id, name : name, score: 0, user_type: UserStatus.Participants };
        this.m_users.setValue(id, new_user);
        socket.join(UniversalParameter.RoomName);

        this.EmitUserType(SocketEvent.UpdateUser, new_user);
    }

    UpdateScore(id: string, score: number) {
        let get_user = this.m_users.getValue(id);

        if (get_user != null) {
            get_user.score = score;
            this.m_users.setValue(id, get_user);

            this.EmitUserType(SocketEvent.UpdateUser, get_user);
        }
    }

    LeaveRoom(id: string) {
        let leaveUser = this.m_users.getValue(id);

        this.m_users.remove(id);

        if (id == this.m_room.host_id) {
            this.EndGame();
            this.m_room.host_id = "";
            return;
        }

        this.EmitUserType(SocketEvent.Disconnect, leaveUser);
    }

    private SetRoomTimer(start_time : number) {
        let endExtendMinute = 1;
        let endDateMiliSecond = new Date(start_time + endExtendMinute * 60000);

        this.m_room.start_time = start_time;
        this.m_room.end_time = endDateMiliSecond.getTime();        
    }

    private EmitUserType(event_tag: string, user: UserComponentType) {
        this.m_io.in(UniversalParameter.RoomName).emit(
            event_tag,
            JSON.stringify(user)
        );
    }
}