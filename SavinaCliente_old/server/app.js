'use strict';

const PACKAGE = require(__appdir + '/package.json');

/**** Modules *****/
var compression = require('compression');
var expressSession = require('express-session');
var bodyParser = require('body-parser');
var favicon = require('serve-favicon');
var security = require('fi-security');
var fileman = require('fi-fileman');
var sockets = require('fi-sockets');
var routes = require('fi-routes');
var express = require('express');
var logger = require('morgan');
var auth = require('fi-auth');
var https = require('https');
var http = require('http');
var path = require('path');

/**** Components ****/
var notifier = component('notifier');
var session = component('session');
var mailer = component('mailer');
var models = component('models');
var db = component('database');

/**** Application ****/
var app = express();

/**** Configuration ****/
var configs = {
  database: config('database'),
  security: config('security'),
  notifier: config('notifier'),
  fileman: config('fileman'),
  session: config('session'),
  sockets: config('sockets'),
  statics: config('statics'),
  assets: config('assets'),
  errors: config('errors'),
  mailer: config('mailer'),
  models: config('models'),
  routes: config('routes'),
  server: config('server'),
  views: config('views'),
  auth: config('auth')
};

/* Configure the components */
session.configure(expressSession, configs.session);
notifier.configure(configs.notifier);
fileman.configure(configs.fileman);
mailer.configure(configs.mailer);

/**** Setup ****/
app.disable('x-powered-by');

app.set('port', configs.server.port || 0);
app.set('view engine', configs.views.engine);
app.set('views', configs.views.basedir);

/* Set locals */
app.locals.development = process.env.NODE_ENV === 'development';
app.locals.environment = process.env.NODE_ENV;
app.locals.description = PACKAGE.description;
app.locals.basedir = configs.views.basedir;
app.locals.version = PACKAGE.version;
app.locals.title = PACKAGE.title;
app.locals.stage = PACKAGE.stage;
app.locals.name = PACKAGE.name;

/* Ensure secure configuration on production */
if (configs.server.secure && app.get('env') === 'production') {
  configs.session.cookie.secure = true; // Serve secure cookies
  app.set('trust proxy', 1); // Trust first proxy
}

/**** Settings ****/

/* Keep this order:
 *
 * 0.- Favicon (The single most important piece of this app)
 * 1.- Session
 * 2.- Cookie Parser
 * 3.- Body Parser
 * 4.- Multipart Parser
 * 6.- Security [...]
 * 5.- Compression
 * 7.- Anything else...
 */

app.use(logger(app.get('env') === 'production' ? 'tiny' : 'dev'));
app.use(favicon(path.normalize(path.join(__appdir, 'client', 'assets', 'favicon.ico'))));

/**
 * !! PhantomJS temporary fix !!
 *
 * TODO: Implement PDFKit or something that makes sense.
 */
app.use([
  '/api/files/:id', '/api/files/:id/:filename',
  '/api/requests/reports/view/of/:id'
], function (req, res, next) {
  /* Pass the session query param to the header's cookies */
  if (req.query.session) {
    req.headers.cookie = decodeURIComponent(req.query.session);
  }

  next();
});

app.use(configs.assets.route, express.static(configs.assets.path));
app.use(expressSession(session.config()));
app.use(session.config().cookieParser);
app.use(fileman.multiparser());
app.use(fileman.cleaner());
app.use(bodyParser.json());
app.use(bodyParser.urlencoded({
  extended: false
}));
app.use(compression());

/**** Initialization ****/

/* Setup application security */
security(app, configs.security)

/* Set routes auth */
auth(app, configs.auth);

/* Create server instance */
var server;

if (configs.server.secure) {
  server = https.createServer(configs.server, app);
} else {
  server = http.createServer(app);
}

/* Register sockets */
sockets.init(server, configs.sockets);

/* Initialize database connection */
db.initialize(configs.database);

/* Register database models */
models(configs.models, function () {

  /* Register routes */
  routes(app, configs.routes);

  /* Register error handlers */
  configs.errors(app);

  /* Start server */
  server.listen(app.get('port'), function () {
    console.log("\n  %s server listening on port %d".bold, configs.server.secure ? 'HTTPS' : 'HTTP', app.get('port'));
  });

});
