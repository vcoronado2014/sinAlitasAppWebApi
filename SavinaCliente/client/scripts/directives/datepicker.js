(function (window) {
  'use strict';

  var moment = window.moment;
  var ng = window.angular;

  var FUTURE = 'future';
  var PAST = 'past';

  function Datepicker(attrs) {
    this.restrict = attrs.dpRestrict;

    this.attrs = {
      minDay: attrs.dpMinDay,
      maxDay: attrs.dpMaxDay,
      day: attrs.dpDay,

      minMonth: attrs.dpMinMonth,
      maxMonth: attrs.dpMaxMonth,
      month: attrs.dpMonth,

      minYear: attrs.dpMinYear,
      maxYear: attrs.dpMaxYear,
      year: attrs.dpYear
    };

    this.months = moment.months();

    this.init();
  }

  /**
   * Initiallzes the datepicker.
   *
   * @return {Void}
   */
  Datepicker.prototype.init = function init() {
    var today = moment();
    var $this = this;

    $this.date = {
      value: moment(),

      year: {
        min: Number($this.attrs.minYear) || 1970,
        max: Number($this.attrs.maxYear) || today.year(),
        value: Number($this.attrs.year) || today.year(),

        add: function () {
          this.value++;
        },

        substract: function () {
          this.value--;
        }
      },

      month: {
        list: [],
        min: Number($this.attrs.minMonth) || 0,
        max: Number($this.attrs.maxMonth) || 11,
        value: Number($this.attrs.month) || today.month(),

        add: function () {
          this.value++;
        },

        substract: function () {
          this.value--;
        }
      },

      day: {
        list: [],
        min: Number($this.attrs.minDay) || 1,
        max: Number($this.attrs.maxDay) || 31,
        value: Number($this.attrs.day) || today.date(),

        add: function () {
          this.value++;
        },

        substract: function () {
          this.value--;
        }
      }
    };

    if ($this.restrict === FUTURE) {
      $this.date.year.min = today.year();
      $this.date.year.max = false;
    } else if ($this.restrict === PAST) {
      $this.date.year.max = today.year();
      $this.date.year.min = false;
    }

    for (var i = $this.date.month.min; i <= $this.date.month.max; i++) {
      $this.date.month.list.push({
        name: $this.months[i],
        disabled: false,
        value: i
      });
    }

    $this.date.value = new Date($this.date.year.value, $this.date.month.value, $this.date.day.value);
  };

  /**
   * Updates the datepicker.
   *
   * @return {Void}
   */
  Datepicker.prototype.update = function update() {
    var today = moment();
    var $date = this.date;

    for (var i = 0, l = $date.month.list.length; i < l; i++) {
      var month = $date.month.list[i];

      if (this.restrict === FUTURE) {
        month.disabled = $date.year.value <= today.year() && month.value < today.month();

        if (month.disabled && $date.month.value < today.month()) {
          $date.month.value = today.month();
        }
      }

      if (this.restrict === PAST) {
        month.disabled = $date.year.value >= today.year() && month.value > today.month();

        if (month.disabled && $date.month.value > today.month()) {
          $date.month.value = today.month();
        }
      }
    }

    var maxDays = Math.min(moment([$date.year.value, $date.month.value]).daysInMonth(), $date.day.max);

    /* Reset days list */
    $date.day.list = [];

    for (i = $date.day.min, l = (maxDays - $date.day.min) + 1; i <= l; i++) {
      var disabled = false;

      if (this.restrict === FUTURE) {
        disabled = $date.year.value <= today.year() && $date.month.value <= today.month() && i < today.date();

        if (disabled && $date.day.value < today.date()) {
          $date.day.value = today.date();
        }
      }

      if (this.restrict === PAST) {
        disabled = $date.year.value >= today.year() && $date.month.value >= today.month() && i > today.date();

        if (disabled && $date.day.value > today.date()) {
          $date.day.value = today.date();
        }
      }

      $date.day.list.push({
        disabled: disabled,
        value: i
      });
    }

    /* Check for min and max day */
    if ($date.day.value > maxDays) {
      $date.day.value = maxDays;
    } else if ($date.day.value < $date.day.min) {
      $date.day.value = $date.day.min;
    }

    /* Check for max month */
    if ($date.month.value > $date.month.max) {
      $date.month.value = $date.month.max;
    }

    /* Check for max year */
    if (ng.isNumber($date.year.max) && $date.year.value > $date.year.max) {
      $date.year.value = $date.year.max;
    } else if (ng.isNumber($date.year.min) && $date.year.value < $date.year.min) {
      $date.year.value = $date.year.min;
    }

    /* Update self date value */
    $date.value = new Date($date.year.value, $date.month.value, $date.day.value);
  };

  /**
   * Resets the datepicker.
   *
   * @return {Void}
   */
  Datepicker.prototype.reset = function reset() {
    this.init();
    this.update();

    console.log(this);
  };

  /**
   * Returns the number of or an array with the days to offset the first week.
   *
   * @param {Boolean} asArray Returns the offset as an array.
   *
   * @return {Number|Array} The offset.
   */
  Datepicker.prototype.getFirstWeekDayOffset = function getFirstWeekDayOffset(asArray) {
    var offset = moment(this.date.value).startOf('month').weekday();

    if (asArray) {
      return Array.apply(null, Array(offset)).map(Number.call, Number);
    }

    return offset;
  };

  /* Expose useful moment methods */
  Datepicker.prototype.weekdaysShort = moment.weekdaysShort;
  Datepicker.prototype.weekdaysMin = moment.weekdaysMin;
  Datepicker.prototype.weekdays = moment.weekdays;

  ng.module('App').directive('datepicker', [
    '$parse',

    function ($parse) {

      return {
        restrict: 'E',

        templateUrl: function ($element, $attrs) {
          return $attrs.dpTemplate;
        },

        link: function ($scope, $element, $attrs) {
          var $model = $parse($attrs.dpModel);
          var $name = $parse($attrs.dpName);

          /* Set the local variables to the scope's [name] param */
          $name.assign($scope, new Datepicker($attrs));

          /* Watch view value changes */
          $scope.$watchGroup([
            $attrs.dpName + '.date.year.value',
            $attrs.dpName + '.date.month.value',
            $attrs.dpName + '.date.day.value'
          ], function () {
            $name($scope).update();
          });

          /* Watch date value changes */
          $scope.$watch(function () {
            return $name($scope).date.value;
          }, function () {
            $model.assign($scope, $name($scope).date.value);
          });
        }
      };

    }
  ]);

}(window));
