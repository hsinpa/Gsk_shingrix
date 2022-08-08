import Router = require("koa-router");
import MongoDB from "./service/MongoDB";

export function SetRouter(router : Router, mongodb:MongoDB) {
    router.use(async (ctx : any, next : any) => {
      //I want to check User Authentication here
      await next();
    });
  
    router.get('/', async function (ctx:any, next:any) {
      ctx.state = {
        title: 'HSINPA'
      };
      await ctx.render('index', {title: "HSINPA"});
    });

    router.get('/feedback', async function (ctx:any, next:any) {
      let r = (await mongodb.feedbackModel.query(ctx.params.count));

      ctx.state = {
        feedback: r
      };
      await ctx.render('feedback', {feedback: r});
    });

    router.get('/rank/:id/:game_id', async function (ctx:any, next:any) {
        let r = (await mongodb.scoreModel.find_ranking(ctx.params.id, parseInt(ctx.params.game_id)));
        console.log(r);
        r = r.sort(function (a, b) {
          return a.ranking - b.ranking;
        });

        let return_json = {ranking: -1};
        
        if (r.length > 0)
            return_json.ranking = r[0].ranking;

        ctx.body = JSON.stringify(return_json);
      });

    router.post('/report_feedback', async function (ctx:any, next:any) {
      if ("feedback" in ctx.request.body && "name" in ctx.request.body) {
        mongodb.feedbackModel.insert(ctx.request.body.feedback, ctx.request.body.name);
        ctx.body = "OK";
        return;
      }
      ctx.body = "Failed";
    });

    router.get('/review_feedback/:count', async function (ctx:any, next:any) {
      let r = (await mongodb.feedbackModel.query(ctx.params.count));

      ctx.body = JSON.stringify(r);;
    });
}