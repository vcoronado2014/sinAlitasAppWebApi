'use strict';

module.exports = function (db, Types) {

  var IcdRequestAnswerDiagnosis = db.define('icdRequestAnswerDiagnosis', {

    id: {
      type: Types.INTEGER,
      autoIncrement: true,
      primaryKey: true,
      unique: true
    },

    icdId: {
      type: Types.INTEGER,
      primaryKey: false,
      unique: false
    },

    requestAnswerDiagnosisId: {
      type: Types.INTEGER,
      primaryKey: false,
      unique: false
    }

  }, {

    timestamps: false

  });

  return IcdRequestAnswerDiagnosis;

};
