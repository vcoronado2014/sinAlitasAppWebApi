'use strict';

var is = require('fi-is');

module.exports = function (db, Types) {

  var Workplace = db.define('workplace', {

    name: {
      type: Types.TEXT,
      allowNull: false,

      validate: {
        notEmpty: true
      }
    },

    deis: {
      type: Types.STRING,
      allowNull: false,
      unique: true,

      validate: {
        notEmpty: true
      }
    },

    private: {
      type: Types.BOOLEAN,
      defaultValue: false,
      allowNull: false
    },

    deletedAt: {
      type: Types.DATE,
      allowNull: true,

      validate: {
        isDate: true
      }
    }

  }, {

    classMethods: {
      associate: function associate(models) {
        Workplace.belongsTo(models.user, {
          as: 'creator'
        });

        Workplace.belongsToMany(models.agreement, {
          through: {
            model: models.agreementWorkplace,
            unique: false
          }
        });

        Workplace.belongsToMany(models.user, {
          through: {
            model: models.userWorkplace,
            unique: false
          }
        });
      },

      getCounterparts: function getCounterparts(workplaceId) {
        return db.model('agreement').findAll({
          attributes: ['id'],
          include: [{
            attributes: ['id'],
            model: db.model('workplace'),
            required: true,
            as: 'selectorWorkplaces',
            where: {
              id: workplaceId
            }
          }, {
            model: db.model('workplace'),
            attributes: ['id'],
            required: true
          }]
        }).then(function (agreements) {
          if (!agreements || is.empty(agreements)) {
            return [];
          }

          var counterparts = [];

          for (var idx = 0, wpid, agreement; idx < agreements.length; idx++) {
            agreement = agreements[idx];

            if (agreement.workplaces.length > 1 && agreement.workplaces[0].id === workplaceId) {
              wpid = agreement.workplaces[1].id;
            } else {
              wpid = agreement.workplaces[0].id;
            }

            if (counterparts.indexOf(wpid) < 0) {
              counterparts.push(wpid);
            }
          }

          return counterparts;
        });
      },

      findByDeis: function findByDeis(deis) {
        return db.model('workplace').findOne({
          where: {
            deis: deis
          }
        });
      }
    }

  });

  return Workplace;

};
