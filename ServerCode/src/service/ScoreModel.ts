import { MongoBulkWriteError } from 'mongodb';
import * as moogoose from 'mongoose';
import { UserComponentType } from '../socket/socket_struct';

interface score_schema_type { name: string; socket_id: string, session: string, score :number, game_id : number }
class ScoreModel {

    _scoreSchema : typeof moogoose.Model;

    constructor(scoreSchema : typeof moogoose.Model) {
        this._scoreSchema =  scoreSchema;
    }

    insert(session_id: string, game_id: number, users: UserComponentType[]) {
        let filter_users = users.filter(x=>x.score > 0);
        let select_users = filter_users.map(x => <score_schema_type>
           {name: x.name, session: session_id, socket_id: x.socket_id, game_id: game_id, score: x.score}
        );
        try {
         this._scoreSchema.insertMany(select_users);
      } catch (e: any) {
         console.log(e);
      } 
    }

    find_ranking(p_user_id: string, p_game_id : number) {
        return this._scoreSchema.aggregate( [
         {
               $setWindowFields: {
                  partitionBy: "$game_id",
                  sortBy: { score: -1 },
                  output: {
                     ranking: {
                        $rank: {}
                     }
                  }
               }
            },
            {$match : {socket_id : p_user_id, game_id : p_game_id}},
         ] )
    }

    find_ranking_in_session(p_user_id: string, p_session: string) {
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
          {$match : {socket_id : p_user_id, session : p_session}},
       ] )
  }
}

export default ScoreModel;