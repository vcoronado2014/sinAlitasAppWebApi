'use strict';

module.exports = function (router) {
  //router.setMaxListeners(0);
  router.get('/', function (req, res, next) {

    /** esto lo comentamos debido a que no podemos traer la fecha del servidor
     * sql server debido a que es muy distinta y el aplicativo trabaja con
     * mucha precisión, por lo tanto se traera la del servidor, que para este 
     * caso no afecta en nada ya que la app no es distribuida
     */
      
      /*


      var config = require('../../config/database');
      var Sequelize = require('sequelize');

      var db = new Sequelize(config.database, config.username, config.password, {
        dialectOptions: config.dialectOptions,
        dialect: config.dialect,
        host: config.host,
        port: config.port,
        logging: false
      });

      //req.setMaxListeners(0);

      db.query("select top 1 getdate() as fecha").then(function(response){
              res.send(response);
              }).error(function(err){
                res.send(new Date());
      });

      */

      res.send(new Date());

});

};