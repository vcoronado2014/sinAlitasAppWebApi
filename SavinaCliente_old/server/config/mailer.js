'use strict';

var path = require('path');

module.exports = {

  templates: path.normalize(path.join(__basedir, 'views', 'emails')),

  debug: require('debug')('app:component:mailer'),

  sender: 'Savina <mailer@finaldevstudio.com>',

  connection: {
    host: 'box819.bluehost.com',
    name: 'finaldevstudio.com',
    secure: true,
    port: 465,

    auth: {
      user: 'mailer@finaldevstudio.com',
      pass: '%6{TLkTF849f'
    },
  },


};
