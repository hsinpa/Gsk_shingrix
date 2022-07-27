import { SocketEvent } from "../Utility/Flag/EventFlag";
import SocketListener from "./emit_receive/socket_listener";
import SocketRoom from "./socket_room";
import * as http from 'http';
import { Server } from "socket.io";
import ScoreModel from "../service/ScoreModel";

export default class SocketMain {

    private m_socket_room: SocketRoom;
    private m_socket_listener: SocketListener;

    constructor() {

    }

    public Init(app : http.Server, score_model : ScoreModel) {
        let io = new Server(app,
            {
                cors: {
                    origin: [ "http://localhost:8080", "*"]
                }
              }
        );
        
        this.m_socket_room = new SocketRoom(io, score_model);
        this.m_socket_listener = new SocketListener(this.m_socket_room);
        this.SetBasicEventListener(io);
    }

    private SetBasicEventListener(io: Server) {
        io.on("connection", (socket) => {
            this.m_socket_listener.RegisterEvent(socket);
            console.log(socket.id + " is connect");

            socket.emit(SocketEvent.Connect, JSON.stringify({
                    id : socket.id,
                })
            );

            socket.on("disconnect", () => {
                this.m_socket_room.LeaveRoom(socket.id);
            });
        });
    }
}