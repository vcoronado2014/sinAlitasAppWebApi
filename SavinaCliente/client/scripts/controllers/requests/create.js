(function (ng) {
  'use strict';

  ng.module('App').controller('Requests:Create', [
    '$scope', '$location', '$http', '$moment', 'Upload', '$mimeicon', '$session', '$q', '$routeParams', '$notifications', 'related',

    function ($scope, $location, $http, $moment, $upload, $mimeicon, $session, $q, $routeParams, $notifications, related) {
      var canceler = $q.defer();

      $scope.requestTypes = related.requestTypes.data;
      $scope.specialties = related.specialties.data;
      $scope.priorities = related.priorities.data;
      $scope.motives = related.motives.data;
      $scope.genders = related.genders.data;

      $scope.submitting = false;

      $session.get('activity').name = 'create';

      //modificado por coro
      $http.get('/api/date', {
            timeout: 9000
          }).then(function success(resDate) {
            if (resDate.status === 200) {
              $scope.fechaHoraServidor = resDate.data;
            }
          }, function error(data, status) {


          }).then(function complete() {

          });

      /* Mimic the model */
      $scope.data = {
        patient: {
          genderId: $scope.genders[0].id,
          firstname: null,
          lastname: null,
          birth: null,
          city: null,
          run: null
        },

        specialtyId: $scope.specialties[0].id,
        priorityId: $scope.priorities[0].id,
        motiveId: $scope.motives[0].id,
        hypothesis: null,
        comment: null,
        icds: []
      };

      ng.forEach($scope.requestTypes, function (rt) {
        if (rt.slug === $routeParams.type) {
          $scope.data.requestTypeId = rt.id;
          $scope.requestType = rt;
        }
      });

      /* Locked fields */
      $scope.locked = {
        patient: {
          firstname: false,
          lastname: false,
          gender: false,
          birth: false,
          city: false
        }
      };

      /**
       * Tab functionality.
       */
      $scope.tab = {
        count: 0,
        curr: 0,

        set: function set(num) {
          if (num < 0) {
            this.curr = 0;
          } else if (num > this.count) {
            this.curr = this.count;
          } else {
            this.curr = num;
          }
        },

        prev: function prev() {
          this.set(this.curr - 1);
        },

        next: function next() {
          this.set(this.curr + 1);
        },
      };

      /**
       * Files functionality.
       */
      $scope.files = {
        list: [],

        remove: function ($index) {
          this.list.splice($index, 1);
        },
      };

      /**
       * Patient functionality.
       */
      $scope.patient = {
        fetching: false,
        fetched: false,
        found: false,

        /** Clear the patient's form and data */
        clear: function clear(run) {
          this.fetching = false;
          this.fetched = false;
          this.found = false;

          if (run) {
            $scope.data.patient.run = null;
          }
          //agregado por coronado
          if ($scope.data.patient.id != null)
          {
            $scope.data.patient.id = 0;
          }


          $scope.locked.patient.firstname = false;
          $scope.locked.patient.lastname = false;
          $scope.locked.patient.gender = false;
          $scope.locked.patient.birth = false;
          $scope.locked.patient.city = false;

          $scope.data.patient.genderId = $scope.genders[0].id;
          $scope.data.patient.firstname = null;
          $scope.data.patient.lastname = null;
          $scope.data.patient.birth = null;
          $scope.data.patient.city = null;

          $scope.datepicker.reset();
        },

        /** Fetch patient's information */
        fetch: function fetch($event) {
          var self = this;

          if ($event) {
            if ($event.keyCode !== 13) {
              return;
            }
          }

          /* Reset variable's states before fetching except run */
          self.clear();
          /* Indicate that we are fetching patient data */
          self.fetching = true;

          /** Get patient by RUN */
          $http.get('/api/patients/by/run/' + $scope.data.patient.run, {
            timeout: canceler.promise
          }).then(function success(res) {
            if (res.status === 204) {
              self.found = false;
              return;
            }

            self.found = true;

            var bdate = new Date(res.data.birth);

            $scope.data.patient.id = res.data.id;
            $scope.data.patientId = res.data.id;

            /* Assign model data */
            $scope.data.patient.run = res.data.run || $scope.data.patient.run;

            $scope.data.patient.genderId = res.data.gender.id;
            $scope.locked.patient.genderId = !!res.data.gender.id;

            $scope.data.patient.birth = bdate;
            $scope.locked.patient.birth = !!res.data.birth;

            $scope.data.patient.city = res.data.city;
            $scope.locked.patient.city = !!res.data.city;

            $scope.data.patient.firstname = res.data.firstname;
            $scope.locked.patient.firstname = !!res.data.firstname;

            $scope.data.patient.lastname = res.data.lastname;
            $scope.locked.patient.lastname = !!res.data.lastname;

            /* Update datepicker */
            $scope.datepicker.date.year.value = bdate.getUTCFullYear() || 1980;
            $scope.datepicker.date.month.value = bdate.getUTCMonth() || 0;
            $scope.datepicker.date.day.value = bdate.getUTCDate() || 1;
          }, function error(data, status) {
            if (status > 0) {
              $session.flash('danger', "No se pudo obtener el paciente!");
            }
          }).then(function complete() {
            self.fetching = false;
            self.fetched = true;
          });
        }
      };

      /**
       * Attachments functionality.
       */
      $scope.attachments = {
        list: [],

        description: null,
        name: null,

        /**
         * Clears the attachments form.
         */
        clear: function () {
          this.description = null;
          this.name = null;
          $scope.files.list = [];
        },

        /**
         * Adds an attachment to the list.
         */
        add: function () {
          this.list.push({
            description: this.description,
            files: $scope.files.list,
            name: this.name
          });

          this.clear();
        },

        /**
         * Deletes an attachment from the list.
         */
        remove: function ($index) {
          this.list.splice($index, 1);
        },

        /**
         * Uploads an attachment.
         */
        upload: function (attachment, request, done) {
          $upload.upload({
            url: '/api/requests/attachments',
            file: attachment.files,
            fields: {
              description: attachment.description,
              requestId: request.id,
              name: attachment.name
            }
          }).then(function success() {
            done(null);
          }, function error() {
            done(new Error('Upload error'));
          });
        }
      };

      /**
       * Diagnoses functionality.
       */
      $scope.icds = {
        list: [],

        add: function add(item) {
          if (this.list.indexOf(item) < 0) {
            this.list.push(item);
          }

          if ($scope.data.icds.indexOf(item.id) < 0) {
            $scope.data.icds.push(item.id);
          }
        },

        del: function del($index) {
          $scope.data.icds.splice($index, 1);
          this.list.splice($index, 1);
        }
      };

      /**
       * Submits the form to create the Consultation Request.
       */
      $scope.submit = function submit() {
        $scope.submitting = true;

        var request;

        function complete() {
          $session.flash('success', "Solicitud publicada exitosamente");
          $location.path('/requests/worktable/sent/' + $routeParams.type);
          $scope.submitting = false;

          $notifications.notify.request.new(request.id);
        }

        function error() {
          $session.flash('danger', "No se ha podido publicar la solicitud");
          $scope.submitting = false;
        }

        /** Create a new Consultation Request */
        $http.post('/api/requests', $scope.data, {
          timeout: canceler.promise
        }).then(function success(res) {
          request = res.data;
          /* Call success if there are no attachments */
          if (!$scope.attachments.list.length) {
            return complete();
          }

          var uploaded = 0;

          /* Upload attachments */
          ng.forEach($scope.attachments.list, function (attachment) {
            $scope.attachments.upload(attachment, request, function (err) {
              if (err) {
                return error();
              }

              if (++uploaded === $scope.attachments.list.length) {
                complete();
              }
            });
          });
        }, error);
      };

      $scope.$on('$destroy', function () {
        canceler.resolve();
      });
    }
  ]);

}(angular));
