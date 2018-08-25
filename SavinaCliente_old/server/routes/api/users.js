'use strict';

var bcrypt = require('bcryptjs');
var is = require('fi-is');

var mailer = component('mailer');

module.exports = function users(router, db) {

  var User = db.model('user');
  var Role = db.model('role');
  var Token = db.model('token');

  /**
   * Log a user in.
   */
  router.post('/signin', function signin(req, res, next) {

    /* Signout any previous user */
    delete req.session.user;
    delete req.session.workplace;

    Role.findOne({
      where: {
        slug: 'user'
      }
    }).then(function success(userRole) {
      if (!userRole || is.empty(userRole)) {
        throw new Error("No user role in database!");
      }

      return User.findOne({
        where: {
          email: req.body.email,
          roleId: userRole.id,
          deletedAt: null
        },
        include: [{
          model: db.model('specialty')
        }, {
          model: db.model('role')
        }]
      });
    }).then(function success(user) {
      if (!user || is.empty(user)) {
        return next();
      }

      /* Check if there's a user and compare the passwords */
      bcrypt.compare(req.body.password, user.password, function compared(err, matches) {
        if (err) {
          return next(err);
        }

        if (!matches) {
          return next();
        }

        req.session.user = user;

        res.send({
          specialty: user.specialty,
          name: user.name,
          role: user.role,
          id: user.id
        });
      });
    }).catch(next);

  }, function (req, res) {

    setTimeout(res.sendStatus.bind(res, 401), 1000);

  });

  /**
   * Logs a user out.
   */
  router.get('/signout', function signout(req, res) {

    delete req.session.user;
    delete req.session.workplace;

    res.sendStatus(204);

  });

  /**
   * Recover a user's password.
   */
  router.post('/recover', function recover(req, res, next) {

    User.findOne({
      attributes: ['id', 'email'],
      where: {
        email: req.body.email,
        deletedAt: null
      }
    }).then(function success(user) {
      if (!user || is.empty(user)) {
        return res.sendStatus(400);
      }

      return Token.create({
        userId: user.id
      }).then(function success(token) {
        mailer.send('recover', user.email, "Recuperar clave", {
          url: req.protocol + '://' + req.get('host') + '/users/reset/' + token.secret
        });

        res.sendStatus(204);
      });
    }).catch(next);

  });

  /**
   * Check if token exists and if it does reset user's password.
   */
  router.post('/reset/:secret', function reset(req, res, next) {

    /* Find the token by its secret, if not expired */
    Token.findOne({
      where: {
        secret: req.params.secret
      },
      include: [{
        model: db.model('user'),
        required: true,
        where: {
          deletedAt: null
        }
      }]
    }).then(function success(token) {
      if (!token || is.empty(token)) {
        return res.sendStatus(400);
      }

      if (!token.user || is.empty(token.user)) {
        return res.sendStatus(412);
      }

      if (token.hasExpired()) {
        return token.destroy().then(function success() {
          res.sendStatus(410);
        });
      }

      token.user.set('password', req.body.password);

      return token.user.save().then(function (user) {
        if (!user || is.empty(user)) {
          return res.sendStatus(400);
        }

        return token.destroy().then(function success() {
          res.sendStatus(204);
        });
      });
    }).catch(next);

  });

  /**
   * Obtains the current user's information.
   */
  router.get('/me', function me(req, res, next) {

    User.findOne({
      attributes: {
        exclude: ['roleId', 'password', 'creatorId']
      },
      where: {
        id: req.session.user.id,
        deletedAt: null
      },
      include: [{
        model: db.model('specialty'),
        required: false
      }]
    }).then(function success(user) {
      if (!user) {
        return res.status(204).send({});
      }

      res.send(user);
    }).catch(next);

  });

  /**
   *
   */
  router.get('/counterparts', function (req, res, next) {

    if (!req.query.ids) {
      return res.status(204).send([]);
    }

    if (is.array(req.query.ids)) {
      req.query.ids.forEach(function (id, idx) {
        req.query.ids[idx] = Number(id);
      });
    }

    if (is.string(req.query.ids) || is.number(req.query.ids)) {
      req.query.ids = Number(req.query.ids);
    }

    var query = {
      attributes: ['id', 'name'],
      where: {
        id: req.query.ids
      }
    };

    if (req.session.user.specialtyId) {
      query.where.specialtyId = null;
    } else {
      query.where.specialtyId = {
        $not: null
      };

      query.include = [{
        model: db.model('specialty'),
        required: true
      }];
    }

    User.findAll(query).then(function (users) {
      if (!users || is.empty(users)) {
        return res.status(204).send([]);
      }

      res.send(users);
    }).catch(next);

  });

};
