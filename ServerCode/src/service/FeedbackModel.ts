import { MongoBulkWriteError } from 'mongodb';
import * as moogoose from 'mongoose';
import { UserComponentType } from '../socket/socket_struct';

class FeedbackModel {
    _feedbackSchema : typeof moogoose.Model;

    constructor(feedbackSchema : typeof moogoose.Model) {
        this._feedbackSchema =  feedbackSchema;
    }

    insert(feedback: string, p_name: string) {
        try {
         this._feedbackSchema.create({name: p_name, feedback: feedback});
      } catch (e: any) {
         console.log(e);
      }
    }

    query(query_count: number) {
        return this._feedbackSchema.find ().sort ( { create_date : -1  } ).limit(query_count); 
    }
}

export default FeedbackModel;