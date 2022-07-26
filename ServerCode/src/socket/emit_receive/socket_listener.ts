import { UserComponentType } from "../socket_struct";
import SocketRoom from "../socket_room";
import { OnSocketEvent } from "../../Utility/Flag/EventFlag";
import { Server, Socket } from "socket.io";

export default class SocketListener {

    private m_socket_room : SocketRoom;

    constructor(socket_room: SocketRoom) {     
        this.m_socket_room = socket_room;
    }

    RegisterEvent(socket: Socket) {
        let self = this;

        socket.on(OnSocketEvent.StartGame, function (data : string) {
            self.m_socket_room.StartGame(data);
        });

        socket.on(OnSocketEvent.ForceEndGame, function (data : string) {
            self.m_socket_room.EndGame();
        });

        socket.on(OnSocketEvent.Score, function (data : string) {
            let json = JSON.parse(data);
            let score: number = json["score"];
            let socket_id = socket.id;
            
            self.m_socket_room.UpdateScore(socket_id, score);
        });
    }
}