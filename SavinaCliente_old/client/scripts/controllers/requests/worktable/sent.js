(function (ng) {
  'use strict';

  ng.module('App').controller('Requests:Worktable:Sent', [
    '$scope', '$http', '$session', '$q', '$routeParams', '$moment',

    function ($scope, $http, $session, $q, $routeParams, $moment) {
      var canceler = $q.defer();

      $scope.filterFormVisible = false;

      $scope.reset = function () {
        $scope.params = {
          dateMinEnabled: false,
          dateMaxEnabled: false,
          order: 'createdAt',
          keywords: null,
          dateMin: null,
          dateMax: null
        };

        $scope.fetch();
      };

      $scope.setOrder = function (value) {
        if ($scope.params.order && $scope.params.order.charAt(0) !== '-') {
          $scope.params.order = $scope.params.order = '-' + value;
        } else {
          $scope.params.order = value;
        }

        $scope.fetch();
      };

      $scope.filter = function (filter, value) {
        $scope.params[filter] = value;
        $scope.fetch();
      };

      $scope.abort = function () {
        canceler.resolve();
        canceler = $q.defer();
      };

      $scope.fetch = function () {
        $scope.fetching = true;
        $scope.abort();

        var params = {};

        if ($scope.params.dateMinEnabled) {
          params.dateMin = $scope.params.dateMin;
        }

        if ($scope.params.dateMaxEnabled) {
          params.dateMax = $moment($scope.params.dateMax).add(1, 'day').subtract(1, 'second').toDate();
        }

        if ($scope.params.order) {
          params.order = $scope.params.order;
        }

        if ($scope.params.keywords) {
          params.keywords = $scope.params.keywords;
        }

        $http.get('/api/requests/worktable/sent/' + $routeParams.type, {
          timeout: canceler.promise,
          params: params
        }).then(function success(res) {
          //agregado por coro
          $http.get('/api/date', {
            timeout: 9000
          }).then(function success(resDate) {
            if (resDate.status === 200) {
              //procesemos la resdata para que lleve la fecha servidor
              for (var i in res.data)
              {
                res.data[i].fechaServidor = $moment(resDate.data);
              }

              ng.forEach(res.data, function (request) {
                request.seenAt = true;
              });

              $scope.requests = res.data;
            }
          }, function error(data, status) {


          }).then(function complete() {

          });


        }, function error(res) {
          if (res.status > 0) {
            $session.flash('danger', "No pudimos obtener tus solicitudes enviadas");
          }
        }).then(function complete() {
          $scope.fetching = false;
        });
      };

      $session.set('activity', {
        request: $routeParams.type,
        name: 'sent'
      });

      $scope.$on('$destroy', function () {
        canceler.resolve();
      });

      $scope.reset();
    }

  ]);

}(angular));
