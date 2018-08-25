'use strict';

var is = require('fi-is');

var USER = 'user';
var USER_WORKPLACE = 'user-workplace';

module.exports = {

  debug: require('debug')('app:auth'),

  authorizer: function (req) {
    if (is.all.object(req.session.user, req.session.workplace)) {
      return USER_WORKPLACE;
    } else if (is.object(req.session.user)) {
      return USER;
    }

    return null;
  },

  routes: [{
    allows: [USER, USER_WORKPLACE],
    path: [
      '/api/workplaces*', '/api/notifications*', '/api/files*', '/api/users/me',
      '/api/preferences*'
    ]
  }, {
    allows: USER_WORKPLACE,
    path: [
      '/api/patients*', '/api/icds*', '/api/messages*', '/api/requests*'
    ]
  }]

};
