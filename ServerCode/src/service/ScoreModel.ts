import * as moogoose from 'mongoose';
import { UserComponentType } from '../socket/socket_struct';

class ScoreModel {

    _scoreSchema : typeof moogoose.Model
    constructor(scoreSchema : typeof moogoose.Model) {
        this._scoreSchema =  scoreSchema;
    }

    insert(session_id: string, users: UserComponentType[]) {

    }

    find_ranking() {

    }
    
}

export default ScoreModel;