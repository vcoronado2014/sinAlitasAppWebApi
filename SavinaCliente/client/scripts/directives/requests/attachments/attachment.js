(function (ng) {
  'use strict';

  ng.module('App').directive('attachment', [
    '$http', '$session', '$q', '$timeout', '$routeParams', '$notifications', 'NOTIFY_TIMEOUT',

    function ($http, $session, $q, $timeout, $routeParams, $notifications, NOTIFY_TIMEOUT) {

      return {
        templateUrl: '/assets/templates/requests/attachments/attachment.html',
        restrict: 'E',

        scope: {
          attachment: '='
        },

        link: function link($scope) {
          var canceler = $q.defer();
          var timeout;

          $scope.attachment.own = $session.user('id') === $scope.attachment.userId;

          if (!$scope.attachment.own && !$scope.attachment.seenAt) {
            timeout = $timeout(function () {
              $http.put('/api/requests/attachments/seen', {
                id: $scope.attachment.id
              }, {
                timeout: canceler.promise
              }).then(function success(res) {
                $scope.attachment.seenAt = res.data.seenAt;
              });
            }, NOTIFY_TIMEOUT);
          }

          $scope.$on('$destroy', function () {
            $timeout.cancel(timeout);
            canceler.resolve();
          });
        }

      };

    }
  ]);

}(angular));
