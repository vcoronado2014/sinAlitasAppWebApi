(function (ng) {
  'use strict';

  var timeout;

  var config = {
    timeout: 6000,
    url: ''
  };

  ng.module('App').factory('$session', [
    '$rootScope', '$timeout',

    function ($rootScope, $timeout) {
      $rootScope.session = {
        activity: {}
      };

      var $session = {
        UPDATED: 'session updated'
      };

      /**
       * Initialize the session.
       *
       * @param {Object} options The options object.
       */
      $session.init = function (options) {
        if (!options.url) {
          throw new Error("The URL cannot be empty");
        }

        config.url = options.url;
      };

      /**
       * Log's a user in.
       *
       * @param {Object} user The user data to store in the session.
       */
      $session.signin = function (data) {
        $rootScope.session.workplace = data.workplace;
        $rootScope.session.user = data.user;

        $rootScope.$broadcast($session.UPDATED);
      };

      /**
       * Logs a user out of the session.
       */
      $session.signout = function () {
        $rootScope.session.workplace = null;
        $rootScope.session.activity = {};
        $rootScope.session.user = null;

        $rootScope.$broadcast($session.UPDATED);
      };

      /**
       * Sets or clears a flash message.
       *
       * @param {String} type The type of the flash message.
       * @param {String} title The title of the flash message.
       * @param {String} message The message body of the flash message.
       */
      $session.flash = function (type, message) {
        $timeout.cancel(timeout);

        if (message) {
          $rootScope.session.flash = {
            type: type || 'default',
            message: message
          };

          $timeout(function () {
            $session.flash();
          }, config.timeout);
        } else {
          $rootScope.session.flash = null;
        }
      };

      /**
       * Obtains a value from the user's session object.
       *
       * @param {String} field The key name to get.
       */
      $session.user = function (field) {
        if (field && $rootScope.session.user) {
          return $rootScope.session.user[field];
        }

        return !!$rootScope.session.user;
      };

      /**
       * Obtains a value from the session object.
       *
       * @param {String} key The key name to get.
       */
      $session.get = function (key) {
        return $rootScope.session[key];
      };

      /**
       * Sets a value on the session object.
       *
       * @param {String} key The key name to set.
       * @param {Mixed} value The value of the key.
       */
      $session.set = function (key, value) {
        $rootScope.session[key] = value;
        $rootScope.$broadcast('session ' + key + ' changed');
      };

      /**
       * Deletes a value from the session object.
       *
       * @param {String} key The key to delete.
       */
      $session.delete = function (key) {
        delete $rootScope.session[key];
        $rootScope.$broadcast('session ' + key + ' changed');
      };

      /**
       * Checks if the current route is allowed to the current user.
       *
       * @param {Object} route The route object to check for.
       * @return {Boolean} Wether the route can be accessed.
       */
      $session.isAllowed = function (route) {
        if (route && route.auth && route.auth.requires) {
          if (ng.isString(route.auth.requires)) {
            return route.auth.requires === this.user('role');
          } else if (ng.isArray(route.auth.requires)) {
            return route.auth.requires.indexOf(this.user('role')) > -1;
          } else {
            return false;
          }
        } else {
          return false;
        }

        /* Allow all by default */
        return true;
      };

      /**
       * Checks if the current route can be accessed after a login.
       *
       * @param {Object} route The route object to check for.
       * @return {Boolean} Wether the route can be accessed after a login.
       */
      $session.canLogin = function (route) {
        return route && route.auth && route.auth.redirect && ng.isString(route.auth.redirect);
      };

      return $session;

    }

  ]).

  run([
    '$rootScope', '$route', '$session', '$location',

    function ($rootScope, $route, $session, $location) {

      /* Session handling */
      $rootScope.$on('$routeChangeStart', function ($event, $next) {
        if (!$next || !$next.$$route) {
          return true;
        }

        var route = $next.$$route;

        $next.$$route.resolve = $next.$$route.resolve || {};

        $next.$$route.resolve.session = [
          '$route', '$session', '$http', '$q',

          function ($route, $session, $http, $q) {
            if (route.auth) {
              var deferred = $q.defer();

              /* Retrieve current session */
              $http.get(config.url).then(function success(res) {
                /* Session exists and user is logged in */
                $session.signin(res.data);
              }, function error() {
                $session.signout();
              }).then(function complete() {
                if ($session.isAllowed(route)) {
                  deferred.resolve();
                } else {
                  deferred.reject();

                  if ($session.canLogin(route)) {
                    $session.set('redirect', $location.path());
                    $location.path(route.auth.redirect);
                    $location.hash(null);
                  } else {
                    $location.path('/forbidden');
                  }
                }
              });

              return deferred.promise;
            } else {
              return true;
            }
          }
        ];
      });
    }

  ]);

}(angular));
