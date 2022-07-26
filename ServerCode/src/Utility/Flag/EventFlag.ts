export const UniversalParameter = {
    RoomName : "room"
}

export const SocketEvent = {
    UpdateUser : "event@update_user",
    StartGame : "event@start_game",
    ForceEndGame : "event@end_game",

    Join : "event@join",
    Disconnect : "event@disconnect",
    Connect : "event@connect",
}

export const OnSocketEvent = {
    StartGame : "event@on_start_game",
    ForceEndGame : "event@on_end_game",

    Score : "event@on_score",
}


export const DatabaseTableName = {
    Score : "score_table"
}

export const DatabaseErrorType = {
    Normal : 0,

    Account : {
        Fail_Login_NoAccount : 1,
        Fail_SignUp_DuplicateAccount : 2,
        Fail_AuthLogin_NotValid : 3
    },

    Friend : {
        Fail_WhatEverTheReason : 1
    }
}