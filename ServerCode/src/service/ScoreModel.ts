import { MongoBulkWriteError } from 'mongodb';
import * as moogoose from 'mongoose';
import { UserComponentType } from '../socket/socket_struct';

interface score_schema_type { name: string; socket_id: string, session: string, score :number }
class ScoreModel {

    _scoreSchema : typeof moogoose.Model;

    constructor(scoreSchema : typeof moogoose.Model) {
        this._scoreSchema =  scoreSchema;
    }

    insert(session_id: string, users: UserComponentType[]) {
        let filter_users = users.filter(x=>x.score > 0);
        let select_users = filter_users.map(x => <score_schema_type>
           {name: x.name, session: session_id, socket_id: x.socket_id, score:x.score}
        );
        try {
         this._scoreSchema.insertMany(select_users);
      } catch (e: any) {
         console.log(e);
      } 
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
            {$match : {socket_id : user_id}},
         ] )
    }
}

export default ScoreModel;