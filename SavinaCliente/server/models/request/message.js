'use strict';

module.exports = function (db, Types) {

  var RequestMessage = db.define('requestMessage', {

    type: {
      type: Types.STRING,
      defaultValue: 'text',

      validate: {
        notEmpty: true
      }
    },

    body: {
      type: Types.TEXT,

      validate: {
        notEmpty: true
      }
    },

    sentAt: {
      type: Types.DATE,
      defaultValue: Types.NOW,
      allowNull: false,

      validate: {
        notEmpty: true,
        isDate: true
      }
    },

    receivedAt: {
      type: Types.DATE,
      allowNull: true,

      validate: {
        isDate: true
      }
    }

  }, {

    classMethods: {
      associate: function associate(models) {
        RequestMessage.belongsTo(models.workplace);

        RequestMessage.belongsTo(models.request);

        RequestMessage.belongsTo(models.user);
      }
    }

  });

  return RequestMessage;

};
