(function (ng) {
  'use strict';

  ng.module('App').directive('requestAnswersAdd', [
    '$http', '$routeParams', '$location', '$session', '$q', '$notifications',

    function ($http, $routeParams, $location, $session, $q, $notifications) {

      return {
        templateUrl: '/assets/templates/requests/answers/add.html',

        restrict: 'E',

        scope: {
          request: '='
        },

        link: function ($scope) {
          var canceler = $q.defer();

          $scope.specialist = !!$session.user('specialty');
          $scope.submitting = false;
          $scope.fetching = false;
          $scope.errored = false;
          $scope.tab = 0;

          $session.get('activity').name = 'answers';

          /* Base model */
          $scope.answer = {
            requestId: $routeParams.id,
            comment: null,

            diagnosis: {
              therapeuticPlan: null,
              description: null,
              evaluation: null,
              confirmed: false,

              checkupExamsRequired: false,
              checkupSameSpecialist: false,
              checkupComment: null,
              checkupModeId: null,
              checkupDate: null,

              exams: [],

              icds: []
            }
          };

          /**
           * Create the answer.
           */
          $scope.submit = function () {
            $scope.submitting = true;

            if ($scope.specialist) {
              delete $scope.answer.comment;

              if ($scope.answer.diagnosis.confirmed) {
                $scope.answer.diagnosis.description = $scope.request.hypothesis;

                ng.forEach($scope.request.icds.reverse(), function (icd) {
                  $scope.answer.diagnosis.icds.unshift(icd.id);
                });
              }

              $scope.icds.filter();

              if (!$scope.answer.diagnosis.checkupRequired) {
                delete $scope.answer.diagnosis.checkupSameSpecialist;
                delete $scope.answer.diagnosis.checkupExamsRequired;
                delete $scope.answer.diagnosis.checkupComment;
                delete $scope.answer.diagnosis.checkupModeId;
                delete $scope.answer.diagnosis.checkupDate;
              }
            } else {
              delete $scope.answer.diagnosis;
            }

            /** Create the answer on the server */
            $http.post('/api/requests/answers', $scope.answer).then(function success() {
              $session.flash('success', "Tu respuesta ha sido publicada");
              $location.hash('answers');

              $notifications.notify.request.answered($routeParams.id);
            }, function error() {
              $session.flash('danger', "No se ha podido publicar tu respuesta");
            }).then(function complete() {
              $scope.submitting = false;
            });
          };

          /**
           * Fetch diagnoses from the server.
           */
          $scope.icds = {
            list: [],

            add: function ($item) {
              var allowed = $scope.answer.diagnosis.icds.indexOf($item.id) < 0;

              if ($scope.answer.diagnosis.confirmed && allowed) {
                allowed = !$scope.request.icds.some(function (icd) {
                  return icd.id === $item.id;
                });
              }

              if (allowed) {
                $scope.answer.diagnosis.icds.push($item.id);
                $scope.icds.list.push($item);
              }
            },

            remove: function ($index) {
              $scope.answer.diagnosis.icds.splice($index, 1);
              $scope.icds.list.splice($index, 1);
            },

            filter: function () {
              if (!$scope.answer.diagnosis.confirmed) {
                return;
              }

              ng.forEach($scope.request.icds, function (ricd) {
                ng.forEach($scope.icds.list, function (iicd, index) {
                  if (ricd.id === iicd.id) {
                    $scope.icds.remove(index);
                  }
                });
              });
            }
          };

          /**
           * Exams handler.
           */
          $scope.exams = {
            add: function () {
              $scope.answer.diagnosis.exams.push({
                samePlace: $scope.form.exam.samePlace,
                name: $scope.form.exam.name
              });

              $scope.form.exam.samePlace = false;
              $scope.form.exam.name = null;
            },

            remove: function ($index) {
              $scope.answer.diagnosis.exams.splice($index, 1);
            }
          };

          if ($scope.specialist) {
            $scope.fetching = true;

            /* Retrieve required statics */
            $http.get('/api/checkup-modes', {
              timeout: canceler.promise
            }).then(function success(res) {
              if (res.data.length) {
                $scope.answer.diagnosis.checkupModeId = res.data[0].id;
                $scope.checkupModes = res.data;
              }
            }, function error(res) {
              if (res.status > 0) {
                $scope.errored = true;
              }
            }).then(function complete() {
              $scope.fetching = false;
            });

            if ($scope.specialist) {
              $scope.$watch('answer.diagnosis.confirmed', function (confirmed) {
                if (confirmed) {
                  $scope.answer.diagnosis.description = $scope.request.hypothesis;
                } else {
                  $scope.answer.diagnosis.description = '';
                }

                $scope.icds.filter();
              });
            }
          }

          $scope.$on('$destroy', function () {
            canceler.resolve();
          });

        }
      };
    }

  ]);

}(angular));
