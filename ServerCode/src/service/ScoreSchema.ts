import {Schema, model} from 'mongoose';

const score_schema = new Schema({
    socket_id :  { type: String, index: true },
    name : String,
    score : Number,
    session: String,
    create_date : {type : Date, default :Date.now},
});

export default score_schema;