'use strict';

module.exports = function (db, Types) {

  var FileRequestAttachment = db.define('fileRequestAttachment', {

    id: {
      type: Types.INTEGER,
      autoIncrement: true,
      primaryKey: true,
      unique: true
    },

    fileId: {
      type: Types.INTEGER,
      primaryKey: false,
      unique: true
    },

    requestAttachmentId: {
      type: Types.INTEGER,
      primaryKey: false,
      unique: false
    }

  }, {

    timestamps: false

  });

  return FileRequestAttachment;

};
