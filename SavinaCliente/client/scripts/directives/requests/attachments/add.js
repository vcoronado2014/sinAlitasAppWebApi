(function (ng) {
  'use strict';

  ng.module('App').directive('requestAttachmentsAdd', [
    '$location', '$session', '$routeParams', '$notifications', 'Upload',

    function ($location, $session, $routeParams, $notifications, $upload) {

      return {
        templateUrl: '/assets/templates/requests/attachments/add.html',

        restrict: 'E',

        scope: {},

        link: function ($scope) {
          $scope.submitting = false;
          $scope.files = [];

          $session.get('activity').name = 'attachments';

          /* Disacrd a selected file */
          $scope.discard = function ($index) {
            $scope.files.splice($index, 1);
          };

          /* Upload the attachment and it's files */
          $scope.submit = function () {
            $scope.submitting = true;

            $upload.upload({
              url: '/api/requests/attachments',
              file: $scope.files,
              fields: {
                description: $scope.data.description,
                requestId: $routeParams.id,
                name: $scope.data.name
              }
            }).then(function success() {
              $notifications.notify.request.attached($routeParams.id);
              $session.flash('success', "Adjunto agregado!");
              $location.hash('attachments');
            }, function error() {
              $session.flash('danger', "No se pudo guardar este adjunto");
            }).then(function () {
              $scope.submitting = false;
            });

          };

        }
      };

    }

  ]);

}(angular));
