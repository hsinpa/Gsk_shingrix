import * as moogoose from 'mongoose';
import { UserComponentType } from '../socket/socket_struct';

interface score_schema_type { name: string; _id: string, session: string, score :number }
class ScoreModel {

    _scoreSchema : typeof moogoose.Model;

    constructor(scoreSchema : typeof moogoose.Model) {
        this._scoreSchema =  scoreSchema;
    }

    insert(session_id: string, users: UserComponentType[]) {
        let filter_users = users.filter(x=>x.score > 0);
        let select_users = filter_users.map(x => <score_schema_type>
           {name: x.name, session: session_id, _id: x.socket_id, score:x.score}
        );

        this._scoreSchema.insertMany(select_users);
    }

    find_ranking(user_id: string) {
        return this._scoreSchema.aggregate( [
            {
               $setWindowFields: {
                  partitionBy: "$session",
                  sortBy: { score: -1 },
                  output: {
                     ranking: {
                        $rank: {}
                     }
                  }
               }
            },
            {$match : {_id : (user_id)}},
         ] )
    }
}

export default ScoreModel;