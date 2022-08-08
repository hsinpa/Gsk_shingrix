import {Schema} from 'mongoose';

const score_schema = new Schema({
    socket_id :  { type: String, index: true },
    name : String,
    score : Number,
    game_id: Number,
    session: String,
    create_date : {type : Date, default :Date.UTC},
});

export default score_schema;