'use strict';

module.exports = function (db, Types) {

  var Preference = db.define('preference', {

    emailRequestAttached: {
      type: Types.BOOLEAN,
      defaultValue: true,
      allowNull: true
    },

    emailRequestAnswered: {
      type: Types.BOOLEAN,
      defaultValue: true,
      allowNull: true
    },

    emailRequestReturned: {
      type: Types.BOOLEAN,
      defaultValue: true,
      allowNull: true
    },

    emailRequestMessaged: {
      type: Types.BOOLEAN,
      defaultValue: true,
      allowNull: true
    },

    emailRequestClosed: {
      type: Types.BOOLEAN,
      defaultValue: true,
      allowNull: true
    },

    emailRequestTaken: {
      type: Types.BOOLEAN,
      defaultValue: true,
      allowNull: true
    },

    emailRequestCreated: {
      type: Types.BOOLEAN,
      defaultValue: true,
      allowNull: true
    },

    soundNotifications: {
      type: Types.BOOLEAN,
      defaultValue: true,
      allowNull: true
    }

  }, {

    classMethods: {
      associate: function associate(models) {
        Preference.belongsTo(models.user);
      }
    }

  });

  return Preference;

};
