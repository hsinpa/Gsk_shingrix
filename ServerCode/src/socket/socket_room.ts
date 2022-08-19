import { RoomComponentType, UserComponentType, UserStatus } from "./socket_struct";
import { SocketEvent, UniversalParameter } from "../Utility/Flag/EventFlag";
import { Server, Socket } from "socket.io";
import { Dictionary } from "typescript-collections";
import {GenerateRandomString} from '../Utility/GeneralMethod';
import ScoreModel from "../service/ScoreModel";

export default class SocketRoom {

    private m_users :Dictionary<string, UserComponentType> = new Dictionary<string, UserComponentType>();

    private m_session_id: string;
    private m_io : Server;
    private m_room : RoomComponentType;
    private m_score_model : ScoreModel;

    private IsGameStart() {
        return this.m_room.start_time > 0 && this.m_room.end_time > 0;
    }

    public get user_count() {
        return this.m_users.size();
    } 

    constructor(io: Server, score_model : ScoreModel) {     
        this.m_io = io;
        this.m_score_model = score_model;
        this.m_room = {room_id: UniversalParameter.RoomName, host_id: "", session_id: "", game_id: -1, start_time: 0, end_time: 0}
    }

    StartGame(id: string, game_id : number)  {
        this.m_session_id = GenerateRandomString(8);
        this.SetRoomTimer(Date.now());
        this.m_room.host_id = id;
        this.m_room.game_id = game_id;
        this.m_room.session_id = this.m_session_id;
        this.m_io.in(UniversalParameter.RoomName).emit(SocketEvent.StartGame, JSON.stringify(this.m_room));
    }

    EndGame() {
        this.m_io.in(UniversalParameter.RoomName).emit(SocketEvent.ForceEndGame);
        this.m_score_model.insert(this.m_session_id, this.m_room.game_id, this.m_users.values());
        this.m_users.clear();
        this.m_room.end_time = 0;
        this.m_room.start_time = 0;
    }

    JoinRoom(socket: Socket, id: string, name: string) {
        let new_user =  {socket_id: id, name : name, score: 0, user_type: UserStatus.Participants };
        this.m_users.setValue(id, new_user);
        socket.join(UniversalParameter.RoomName);

        this.EmitUserType(SocketEvent.UpdateUser, new_user);

        if (this.IsGameStart())
            socket.emit(SocketEvent.StartGame, JSON.stringify(this.m_room));
        
        this.m_io.emit(
            SocketEvent.Join,
            JSON.stringify({id : id, count: this.m_users.size()}
        ));
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

        this.m_io.in(UniversalParameter.RoomName).emit(
            SocketEvent.Disconnect,
            JSON.stringify({id : id, count: this.m_users.size()}
        ));
    }

    private SetRoomTimer(start_time : number) {
        let endExtendMinute = 1;
        let endDateMiliSecond = new Date(start_time + endExtendMinute * 64000);

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