'use strict';

module.exports = function (db, Types) {

  var UserWorkplace = db.define('userWorkplace', {

    id: {
      type: Types.INTEGER,
      autoIncrement: true,
      primaryKey: true,
      unique: true
    },

    userId: {
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

  return UserWorkplace;

};
