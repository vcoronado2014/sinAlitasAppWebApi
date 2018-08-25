'use strict';

module.exports = {

  saveUninitialized: true,

  name: 'SAVINA-SESSION',

  resave: true,

  cookie: {
    secure: false
  },

  store: function (session) {
    var RedisStore = require('connect-redis')(session);

    return new RedisStore({
      host: 'localhost',
      port: 6379,
      db: 0
    });
  }
};
