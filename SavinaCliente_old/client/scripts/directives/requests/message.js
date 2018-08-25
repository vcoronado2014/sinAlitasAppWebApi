(function (ng) {
  'use strict';

  ng.module('App').directive('message', [
    '$http', '$routeParams', '$q', '$filter', '$timeout', '$session', '$notifications',

    function ($http, $routeParams, $q, $filter, $timeout, $session) {

      return {
        templateUrl: '/assets/templates/message.html',
        restrict: 'E',

        scope: {
          messages: '=',
          notify: '=',
          index: '='
        },

        link: function link($scope, $element) {
          $scope.message = $scope.messages[$scope.index];
          $scope.errored = false;
          $scope.sending = false;

          /**
           * Message uploaded successfully.
           */
          function success(res) {
            $scope.messages[$scope.index] = $scope.message = res.data;
            $scope.notify();
          }

          /**
           * Message couldn't be uploaded.
           */
          function error() {
            $element.addClass('errored');
            $scope.errored = true;
          }

          /**
           * Request complete.
           */
          function complete() {
            $scope.sending = false;
          }

          /**
           * Sends the message.
           */
          function send() {
            $element.removeClass('errored');
            $scope.errored = false;
            $scope.sending = true;

            $http.post('/api/requests/messages', $scope.message).
            then(success, error).then(complete);
          }

          if ($session.user('id') === $scope.message.userId) {
            $scope.message.own = true;
            $element.addClass('own');
          }

          var previous = $scope.messages[$scope.index - 1];

          if (previous && previous.userId === $scope.message.userId) {
            $element.addClass('continued');
          }

          if (!$scope.message.sentAt) {
            send();
          }

          $scope.send = send;
        }
      };

    }

  ]);

}(angular));
