'use strict';

module.exports = function (db, Types) {

  var Agreement = db.define('agreement', {

    name: {
      type: Types.TEXT,
      allowNull: false,

      validate: {
        notEmpty: true
      }
    },

    quota: {
      type: Types.INTEGER,
      allowNull: true,

      validate: {
        isNumeric: true,
        notEmpty: true,
        isInt: true
      }
    },

    deletedAt: {
      type: Types.DATE,
      allowNull: true,

      validate: {
        isDate: true
      }
    }

  }, {

    classMethods: {
      associate: function associate(models) {
        Agreement.belongsTo(models.specialty);

        Agreement.belongsTo(models.user, {
          as: 'creator'
        });

        Agreement.belongsToMany(models.workplace, {
          as: 'selectorWorkplaces',
          through: {
            model: models.agreementWorkplace,
            unique: false
          }
        });

        Agreement.belongsToMany(models.workplace, {
          through: {
            model: models.agreementWorkplace,
            unique: false
          }
        });
      }
    }

  });

  return Agreement;

};
