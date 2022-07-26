import * as moogoose from 'mongoose';
import ScoreSchema from './ScoreSchema';
import ScoreModel from './ScoreModel';


import {DatabaseTableName} from '../Utility/Flag/EventFlag'

class MongoDB {
    private config = {};
    private dburi : string;
    private moogoseDB : moogoose.Mongoose;

    private scoreSchema : typeof moogoose.Model;

    private scoreModel : ScoreModel;

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
        this.scoreSchema = this.moogoseDB.model(DatabaseTableName.Score, ScoreSchema);
    }

    RegisterAllModel() {
        this.scoreModel = new ScoreModel(this.scoreSchema);
    }

}

export default MongoDB;