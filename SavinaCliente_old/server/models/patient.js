'use strict';

var utils = component('utils');
var rut = component('rut');

module.exports = function (db, Types) {

  var Patient = db.define('patient', {

    firstname: {
      type: Types.TEXT,
      allowNull: false,

      validate: {
        notEmpty: true
      },

      set: function (value) {
        this.setDataValue('firstname', utils.format.asName(value));
      }
    },

    lastname: {
      type: Types.TEXT,
      allowNull: false,

      validate: {
        notEmpty: true
      },

      set: function (value) {
        this.setDataValue('lastname', utils.format.asName(value));
      }
    },

    run: {
      type: Types.STRING,
      allowNull: false,

      validate: {
        isValidRun: rut.validate,
        notEmpty: true
      },
    },

    birth: {
      type: Types.DATE,
      allowNull: false,

      validate: {
        notEmpty: true,
        isDate: true
      }
    },

    city: {
      type: Types.TEXT,
      allowNull: false,

      validate: {
        notEmpty: true
      }
    }

  }, {

    classMethods: {
      associate: function associate(models) {
        Patient.belongsTo(models.gender);
      },

      createOrUpdate: function (conditions, data) {

        return db.model('patient').findOne(conditions)

        .then(function success(patient) {
          if (patient) {
            return patient.update(data);
          }

          return db.model('patient').create(data);

        });
      }
    }
  });

  return Patient;
};
