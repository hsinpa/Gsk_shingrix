import {Schema} from 'mongoose';

const feedback_schema = new Schema({
    name : String,
    feedback: String,
    create_date : {type : Date, default :Date.now},
});

export default feedback_schema;