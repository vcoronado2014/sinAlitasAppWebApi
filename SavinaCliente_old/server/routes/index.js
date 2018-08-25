'use strict';

module.exports = function (router) {

  /**
   * Client routes.
   *
   * This routes render the default layout and Angular takes care of the rest.
   */
  router.get([
    /* Pages */
    '/', '/help', '/workplace', '/create', '/notfound', '/links/:hash',

    /* Users */
    '/users/recover', '/users/reset/:secret', '/users/profile',
    '/users/counterparts',

    /* Requests */
    '/requests/worktable/sent/:type', '/requests/worktable/waiting/:type',
    '/requests/worktable/ongoing/:type', '/requests/worktable/closed/:type',
    '/requests/create/:type', '/requests/:id', '/requests/take/:id',
    '/requests/room/:id', '/requests/attachments/:id'

  ], function (req, res) {

    /* Render the default public layout */
    res.render('index');

  });

};
