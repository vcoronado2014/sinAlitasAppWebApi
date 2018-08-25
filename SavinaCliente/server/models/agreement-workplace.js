'use strict';

module.exports = function (db, Types) {

  var AgreementWorkplace = db.define('agreementWorkplace', {

    id: {
      type: Types.INTEGER,
      autoIncrement: true,
      primaryKey: true,
      unique: true
    },

    agreementId: {
      type: Types.INTEGER,
      primaryKey: false,
      unique: false
    },

    workplaceId: {
      type: Types.INTEGER,
      primaryKey: false,
      unique: false
    }

  }, {

    timestamps: false

  });

  return AgreementWorkplace;

};
