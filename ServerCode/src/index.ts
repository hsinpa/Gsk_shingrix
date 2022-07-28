import { Server, IncomingMessage, ServerResponse } from 'http'
import {join} from 'path';
import SocketMain from './socket/socket_main';
import MongoDB from './service/MongoDB';

import * as http from 'http';
import * as fs from 'fs';

import * as koa_static from 'koa-static';
import * as Router from 'koa-router';
import {SetRouter} from './routes';

const Koa = require('koa');
const bodyParser = require('koa-bodyparser');

const router = new Router();

const views = require('koa-views');

const app = new Koa();

const dotenv = require('dotenv');
dotenv.config();

const env = process.env;
const socketMain = new SocketMain();

let rootFolder : string = join(__dirname, '..',);

app.use(koa_static(
  join( rootFolder,  '/views')
));

app.use(views(rootFolder + '/views', {
  map: {
    html: 'handlebars'
  }
}));

app.use(bodyParser());
app.use(router.routes());
app.use(router.allowedMethods());

// @ts-ignore
var server = http.createServer( app.callback());

const mongodb = new MongoDB(env, (db: MongoDB) => {
  console.log("Connect to database");
  SetRouter(router, db);

  socketMain.Init(server, db.scoreModel);

  server.listen(3000, 'localhost', function () {
    console.log(`Application worker ${process.pid} started...`);
  });
});

