(function (ng) {
  'use strict';

  ng.module('App').directive('answer', [
    '$http', '$session', '$q', '$timeout', '$routeParams', 'NOTIFY_TIMEOUT',

    function ($http, $session, $q, $timeout, $routeParams, NOTIFY_TIMEOUT) {

      return {
        templateUrl: '/assets/templates/requests/answers/answer.html',
        restrict: 'E',

        scope: {
          answer: '='
        },

        link: function ($scope, $element) {
          var $scroller = $element.closest('.scroll-y');
          var canceler = $q.defer();
          var timeout;

          function isVisible() {
            var visible = $element.offset().top + Math.round($element.height() * 0.1) <= $(window).height();

            console.log("Answer %s is %s!", $scope.answer.id, visible ? "visible" : "not visible");

            return visible;
          }

          /**
           * Mark answer as seen.
           *
           * Makes the request to mark the answer as seen and cancels scroll
           * events and timeouts.
           */
          function markAsSeen() {
            if (isVisible()) {
              $http.put('/api/requests/answers/seen', {
                requestId: $routeParams.id,
                id: $scope.answer.id
              }, {
                timeout: canceler.promise
              }).then(function success(res) {
                $scope.answer.seenAt = res.data.seenAt;
              });

              $scroller.off('scroll', onscroll);

              stopTimeout();
            }
          }

          function stopTimeout() {
            $timeout.cancel(timeout);
            timeout = null;
          }

          function startTimeout() {
            timeout = $timeout(markAsSeen, NOTIFY_TIMEOUT);
          }

          /**
           * Scroll listener.
           *
           * Checks if at least 10% of the answer is on screen after starting
           * the timeout to mark it as seen.
           */
          function onscroll() {
            if (isVisible()) {
              if (!timeout) {
                startTimeout();
              }
            } else {
              if (timeout) {
                stopTimeout();
              }
            }
          }

          /* Listen for scroll events if answer is unseen and does not belongs to the user */
          if ($scope.answer.userId !== $session.user('id') && !$scope.answer.seenAt) {
            if (isVisible()) {
              startTimeout();
            }

            $scroller.on('scroll', onscroll);
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
