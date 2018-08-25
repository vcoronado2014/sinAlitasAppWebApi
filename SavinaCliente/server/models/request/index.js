'use strict';

module.exports = function (db, Types) {

  var Request = db.define('request', {

    hypothesis: {
      type: Types.TEXT,
      allowNull: false,

      validate: {
        notEmpty: true
      }
    },

    comment: {
      type: Types.TEXT,
      allowNull: true
    },

    closedAt: {
      type: Types.DATE,
      allowNull: true,

      validate: {
        isDate: true
      }
    },

    rayenSicId: {
      type: Types.INTEGER,
      allowNull: true
    }

  }, {

    classMethods: {
      associate: function associate(models) {
        Request.belongsTo(models.specialty);

        Request.belongsTo(models.priority);

        Request.belongsTo(models.patient);

        Request.belongsTo(models.motive);

        Request.belongsTo(models.requestType);

        Request.hasMany(models.requestAttachment);

        Request.hasMany(models.requestAnswer);

        Request.hasMany(models.notification, {
          foreignKey: 'foreignKey',
          constraints: false,
          scope: {
            model: 'request'
          }
        });

        Request.hasOne(models.notification, {
          foreignKey: 'foreignKey',
          as: 'closedNotification',
          constraints: false,
          required: false,
          scope: {
            model: 'request',
            action: 'closed'
          }
        });

        Request.hasOne(models.notification, {
          foreignKey: 'foreignKey',
          as: 'seenNotification',
          constraints: false,
          required: false,
          scope: {
            model: 'request',
            action: 'seen'
          }
        });

        Request.hasOne(models.requestAnswer, {
          as: 'diagnosticAnswer'
        });

        Request.belongsTo(models.user, {
          as: 'creatorUser'
        });

        Request.belongsTo(models.workplace, {
          as: 'creatorWorkplace'
        });

        Request.belongsTo(models.user, {
          as: 'specialistUser'
        });

        Request.belongsTo(models.workplace, {
          as: 'specialistWorkplace'
        });

        Request.belongsToMany(models.icd, {
          through: {
            model: models.icdRequest,
            unique: false
          }
        });

        Request.addScope('diagnosticAnswer', {
          include: [{
            attributes: ['id', 'userId', 'workplaceId', 'requestId', 'seenAt', 'createdAt'],
            model: db.model('requestAnswer'),
            as: 'diagnosticAnswer',
            required: false,
            order: [
              ['createdAt', 'DESC']
            ],
            where: {
              comment: null
            },
            include: [{
              attributes: ['id', 'name'],
              model: db.model('user'),
              include: [{
                model: db.model('specialty')
              }]
            }, {
              model: db.model('workplace')
            }, {
              attributes: ['id', 'requestAnswerId', 'description', 'confirmed'],
              model: db.model('requestAnswerDiagnosis'),
              include: [{
                model: db.model('icd')
              }, {
                model: db.model('checkupMode')
              }, {
                model: db.model('requestAnswerDiagnosisExam')
              }]
            }]
          }]
        });

      }
    },

    getterMethods: {
      sid: function () {
        return ('000000' + this.id).slice(-Math.max(6, String(this.id).length));
      }
    }

  });

  return Request;

};
