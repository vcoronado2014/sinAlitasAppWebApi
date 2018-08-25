'use strict';

module.exports = function (db, Types) {

  var RequestAttachment = db.define('requestAttachment', {

    name: {
      type: Types.TEXT,
      allowNull: false,

      validate: {
        notEmpty: true
      }
    },

    description: {
      type: Types.TEXT,
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
        RequestAttachment.belongsTo(models.workplace);

        RequestAttachment.belongsTo(models.request);

        RequestAttachment.belongsTo(models.user);

        RequestAttachment.belongsToMany(models.file, {
          through: {
            model: models.fileRequestAttachment,
            unique: false
          }
        });
      }
    }

  });

  return RequestAttachment;

};
