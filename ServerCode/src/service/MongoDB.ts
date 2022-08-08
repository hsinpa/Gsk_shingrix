import * as moogoose from 'mongoose';
import ScoreSchema from './ScoreSchema';
import FeedbackSchema from './FeedbackSchema';

import ScoreModel from './ScoreModel';


import {DatabaseTableName} from '../Utility/Flag/EventFlag'
import FeedbackModel from './FeedbackModel';

class MongoDB {
    private config = {};
    private dburi : string;
    private moogoseDB : moogoose.Mongoose;

    private scoreSchema : typeof moogoose.Model;
    private feedbackSchema : typeof moogoose.Model;

    scoreModel : ScoreModel;
    feedbackModel : FeedbackModel;

    constructor(env : NodeJS.ProcessEnv, callback :  (db : MongoDB )=> void) {
        this.config = {
            user : env.DATABASE_USER,
            password : env.DATABASE_PASSWORD,
            database : env.DATABASE_NAME,
            host : env.DATABASE_SERVER
        }

        this.dburi = `mongodb+srv://${env.DATABASE_USER}:${env.DATABASE_PASSWORD}@cluster0.oftawqd.mongodb.net/${env.DATABASE_NAME}?retryWrites=true&w=majority`;        
        this.ConnectToDatabase(callback);
    }

    async ConnectToDatabase(callback : (db : MongoDB ) => void) {
        console.log(this.dburi);
        this.moogoseDB = await moogoose.connect(this.dburi);
        this.RegisterAllSchema();
        this.RegisterAllModel();
        callback(this);
    }

    RegisterAllSchema() {
        this.feedbackSchema = this.moogoseDB.model(DatabaseTableName.Feedback, FeedbackSchema);
        this.scoreSchema = this.moogoseDB.model(DatabaseTableName.Score, ScoreSchema);
    }

    RegisterAllModel() {
        this.scoreModel = new ScoreModel(this.scoreSchema);
        this.feedbackModel = new FeedbackModel(this.feedbackSchema);
    }

}

export default MongoDB;