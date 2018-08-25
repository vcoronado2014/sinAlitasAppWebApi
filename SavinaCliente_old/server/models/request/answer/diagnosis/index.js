'use strict';

module.exports = function (db, Types) {

  var RequestAnswerDiagnosis = db.define('requestAnswerDiagnosis', {

    evaluation: {
      type: Types.TEXT,
      allowNull: false,

      validate: {
        notEmpty: true
      }
    },

    confirmed: {
      type: Types.BOOLEAN,
      defaultValue: false,
      allowNull: true
    },

    description: {
      type: Types.TEXT,
      allowNull: false,

      validate: {
        notEmpty: true
      }
    },

    therapeuticPlan: {
      type: Types.TEXT,
      allowNull: true
    },

    checkupDate: {
      type: Types.DATE,
      allowNull: true,

      validate: {
        isDate: true
      }
    },

    checkupExamsRequired: {
      type: Types.BOOLEAN,
      allowNull: true
    },

    checkupSameSpecialist: {
      type: Types.BOOLEAN,
      allowNull: true
    },

    checkupComment: {
      type: Types.TEXT,
      allowNull: true
    }

  }, {

    classMethods: {
      associate: function associate(models) {
        RequestAnswerDiagnosis.hasMany(models.requestAnswerDiagnosisExam);

        RequestAnswerDiagnosis.belongsTo(models.checkupMode);

        RequestAnswerDiagnosis.belongsTo(models.requestAnswer);

        RequestAnswerDiagnosis.belongsToMany(models.icd, {
          through: {
            model: models.icdRequestAnswerDiagnosis,
            unique: false
          }
        });
      }
    }

  });

  return RequestAnswerDiagnosis;

};
