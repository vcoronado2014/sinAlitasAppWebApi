'use strict';

module.exports = function (db, Types) {

  var Notification = db.define('notification', {

    foreignKey: {
      type: Types.INTEGER,
      allowNull: false,

      validate: {
        isInt: true
      }
    },

    action: {
      type: Types.STRING,
      allowNull: false,

      validate: {
        notEmpty: true
      }
    },

    model: {
      type: Types.STRING,
      allowNull: false,

      validate: {
        notEmpty: true
      }
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
        Notification.belongsTo(models.user, {
          as: 'sender'
        });

        Notification.belongsTo(models.user, {
          as: 'receiver'
        });

        Notification.belongsTo(models.request, {
          foreignKey: 'foreignKey',
          constraints: false
        });

        Notification.belongsTo(models.workplace);
      }
    }

  });

  return Notification;

};
