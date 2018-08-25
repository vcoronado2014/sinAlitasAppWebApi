'use strict';

module.exports = function (db, Types) {

  var IcdRequest = db.define('icdRequest', {

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

    requestId: {
      type: Types.INTEGER,
      primaryKey: false,
      unique: false
    }

  }, {

    timestamps: false

  });

  return IcdRequest;

};
