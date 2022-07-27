import * as moogoose from 'mongoose';
import { UserComponentType } from '../socket/socket_struct';

interface score_schema_type { name: string; socket_id: string, session: string, score :number }
class ScoreModel {

    _scoreSchema : typeof moogoose.Model;

    constructor(scoreSchema : typeof moogoose.Model) {
        this._scoreSchema =  scoreSchema;
    }

    insert(session_id: string, users: UserComponentType[]) {
        const arr =[{"value":"abc","checked":true},{"value":"xyz","checked":false},{"value":"lmn","checked":true}];

        let filter_users = users.filter(x=>x.score > 0);
        let select_users = filter_users.map(x => <score_schema_type>
           {name: x.name, session: session_id, socket_id: x.socket_id, score:x.score}
        );

        this._scoreSchema.insertMany(select_users);
    }

    find_ranking() {

    }
}

export default ScoreModel;