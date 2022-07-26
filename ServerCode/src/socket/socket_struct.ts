export interface UserComponentType {
    socket_id : string
    name : string,
    score? : number,
    user_type? : UserStatus
}

export enum UserStatus {
    Host, Participants
 }
 
 
export interface RoomComponentType {
    room_id : string,
    host_id : string,

    start_time : number,
    end_time : number,
}
