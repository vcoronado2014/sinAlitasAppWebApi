'use strict';

module.exports = function (db, Types) {

  var Icd = db.define('icd', {

    code: {
      type: Types.STRING,
      allowNull: false,
      unique: true,

      validate: {
        notEmpty: true
      }
    },

    description: {
      type: Types.TEXT,
      allowNull: false,

      validate: {
        notEmpty: true
      }
    }

  }, {

    classMethods: {
      associate: function associate(models) {
        Icd.belongsToMany(models.request, {
          through: {
            model: models.fileRequestAttachment,
            unique: false
          }
        });

        Icd.belongsToMany(models.requestAnswerDiagnosis, {
          through: {
            model: models.icdRequestAnswerDiagnosis,
            unique: false
          }
        });
      }
    },

    timestamps: false

  });

  return Icd;

};
