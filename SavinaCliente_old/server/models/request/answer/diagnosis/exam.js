'use strict';

module.exports = function (db, Types) {

  var RequestAnswerDiagnosisExam = db.define('requestAnswerDiagnosisExam', {

    name: {
      type: Types.TEXT,
      allowNull: false,

      validate: {
        notEmpty: true
      }
    },

    samePlace: {
      type: Types.BOOLEAN,
      defaultValue: false,
      allowNull: true
    }

  }, {

    classMethods: {
      associate: function associate(models) {
        RequestAnswerDiagnosisExam.belongsTo(models.requestAnswerDiagnosis);
      }
    }

  });

  return RequestAnswerDiagnosisExam;

};
