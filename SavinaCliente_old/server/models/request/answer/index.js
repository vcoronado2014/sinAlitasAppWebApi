'use strict';

module.exports = function (db, Types) {

  var RequestAnswer = db.define('requestAnswer', {

    comment: {
      type: Types.TEXT,
      allowNull: true
    },

    seenAt: {
      type: Types.DATE,
      allowNull: true,

      validate: {
        isDate: true
      }
    }

  }, {

    classMethods: {
      associate: function associate(models) {
        RequestAnswer.belongsTo(models.workplace);

        RequestAnswer.belongsTo(models.user);

        RequestAnswer.belongsTo(models.request);

        RequestAnswer.belongsTo(models.request);

        RequestAnswer.hasOne(models.requestAnswerDiagnosis);
      }
    }

  });

  return RequestAnswer;

};
