(function (ng) {
  'use strict';

  ng.module('App').directive('requestAttachments', [
    '$http', '$session', '$routeParams', '$location', '$q', '$notifications', '$room',

    function ($http, $session, $routeParams, $location, $q, $notifications, $room) {

      return {
        templateUrl: '/assets/templates/requests/attachments/list.html',
        restrict: 'E',

        scope: {},

        link: function ($scope) {
          var canceler = $q.defer();

          $scope.attachments = [];
          $scope.fetching = true;
          $scope.errored = false;

          $session.get('activity').name = 'attachments';

          $scope.fetch = function () {
            $scope.fetching = true;
            $scope.errored = false;

            $http.get('/api/requests/attachments/of/' + $routeParams.id, {
              timeout: canceler.promise
            }).then(function success(res) {
              $scope.attachments = res.data;

              /* Update notifications */
              $http.put('/api/notifications/request/attached', {
                requestId: $routeParams.id
              }, {
                timeout: canceler.promise
              }).then(function success() {
                $notifications.requests.ongoing.update();
              });
            }, function error(res) {
              if (res.status > 0) {
                $scope.errored = true;
              }
            }).then(function complete() {
              $scope.fetching = false;
            });
          };

          $scope.view = function (id) {
            $location.search('id', id);
            $location.hash('attachments-view');
          };

          $room.on('attached', function () {
            if ($location.hash() === 'attachments') {
              $scope.fetch();
            }
          });

          $scope.$on('$destroy', function () {
            canceler.resolve();
          });

          $scope.fetch();
        }

      };

    }

  ]);

}(angular));
