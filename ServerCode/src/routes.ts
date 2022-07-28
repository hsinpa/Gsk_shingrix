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
      await ctx.render('main', {title: "HSINPA"});
    });

    router.get('/rank/:id', async function (ctx:any, next:any) {
        let r = (await mongodb.scoreModel.find_ranking(ctx.params.id));
        ///console.log(r);

        r = r.sort(function (a, b) {
          return a.ranking - b.ranking;
        });

        let return_json = {ranking: -1};
        
        if (r.length > 0)
            return_json.ranking = r[0].ranking;

        ctx.body = JSON.stringify(return_json);
      });
}
  