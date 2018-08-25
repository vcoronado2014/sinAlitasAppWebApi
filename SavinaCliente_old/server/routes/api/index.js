'use strict';

module.exports = function (router) {

  /**
   * Get current session's public data.
   */
  router.get('/session', function (req, res) {

    /* Check if there's a user in session */
    if (!req.session.user) {
      return res.status(401).end();
    }

    var session = {};

    session.user = {
      id: req.session.user.id,
      name: req.session.user.name,
      specialty: req.session.user.specialty,
      role: 'user'
    };

    if (req.session.workplace) {
      session.workplace = {
        id: req.session.workplace.id,
        name: req.session.workplace.name
      };

      session.user.role = 'user workplace';
    }

    res.send(session);
  });

  /**
   * Return server time.
   */
  router.get('/time', function (req, res) {
    res.send(Date.now().toString());
  });

};
