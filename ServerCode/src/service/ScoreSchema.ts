import {Schema, model} from 'mongoose';

const score_schema = new Schema({
    socket_id : String,
    username : String,
    score : Number,
    create_date : {type : Date, default :Date.now},
});

export default score_schema;