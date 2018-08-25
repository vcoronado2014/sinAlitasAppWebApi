'use strict';

module.exports = function (db, Types) {

  var File = db.define('file', {

    mimetype: {
      type: Types.STRING,
      allowNull: false,

      validate: {
        notEmpty: true
      }
    },

    filename: {
      type: Types.TEXT,
      allowNull: false,

      validate: {
        notEmpty: true
      }
    },

    filesize: {
      type: Types.INTEGER,
      allowNull: false,

      validate: {
        isNumeric: true,
        notEmpty: true,
        isInt: true
      }
    },

    path: {
      type: Types.STRING,
      allowNull: false,

      validate: {
        notEmpty: true
      }
    },

    md5: {
      type: Types.STRING,
      allowNull: false,

      validate: {
        notEmpty: true
      }
    }

  }, {

    classMethods: {
      associate: function associate(models) {
        File.belongsToMany(models.requestAttachment, {
          through: {
            model: models.fileRequestAttachment,
            unique: true
          }
        });
      }
    }

  });

  return File;

};
