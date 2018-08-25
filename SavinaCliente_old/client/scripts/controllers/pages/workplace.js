(function (ng) {
  'use strict';

  ng.module('App').controller('Pages:Workplace', [
    '$scope', '$http', '$location', '$session', '$q', '$notifications', '$presence', '$rootScope', '$timeout', 'workplaces',

    function ($scope, $http, $location, $session, $q, $notifications, $presence, $rootScope, $timeout, workplaces) {
      var canceler = $q.defer();

      $scope.workplaces = workplaces.data;
      $scope.setting = false;
      $scope.adding = false;

      function onWorkplaceSet() {
        var type = $session.get('activity').request || 'consultation';
        var activity = $session.get('activity').name;

        switch (activity) {
        case 'waiting':
        case 'ongoing':
        case 'closed':
        case 'sent':
          /* No change */
          break;

        default:
          activity = 'ongoing';
        }

        $location.path('/requests/worktable/' + activity + '/' + type);
      }

      $scope.setWorkplace = function (workplace) {
        $scope.setting = true;

        if ($session.get('workplace') && $session.get('workplace').id === workplace.id) {
          return onWorkplaceSet();
        }

        $http.put('/api/workplaces/active', {
          id: workplace.id
        }, {
          timeout: canceler.promise
        }).then(onWorkplaceSet).catch(function (res) {
          if (res.status > 0) {
            $session.flash('danger', "No se pudo fijar el lugar de trabajo");
          }
        }).finally(function () {
          $scope.setting = false;
        });
      };

      $notifications.update.workplaces(workplaces.data);

      $scope.$on($notifications.WORKPLACES_UPDATED, function onUpdate() {
        console.log($notifications.workplaces);

        $scope.notifications = $notifications.workplaces.list;

        $timeout();
      });

      $scope.$on($notifications.REQUESTS_UPDATED, function () {
        $notifications.update.workplaces(workplaces.data);
      });

      $scope.$on('$destroy', function () {
        canceler.resolve();
      });
    }
  ]);

}(angular));
