import { Injectable, Component } from '@angular/core';
import { Http, Headers } from '@angular/http';
import { AppSettings } from '../../../AppSettings'

import 'rxjs/add/operator/map';
import { THIS_EXPR } from '@angular/compiler/src/output/output_ast';
@Injectable()
export class GlobalService{
    nombreUsuario: string;
    envoltorio: any;
    loggedIn:boolean;

    persona: any;
  
    constructor(
      private http: Http
    ){
      //inicializamos los valores
      this.nombreUsuario = '';
      this.persona = null;
      this.loggedIn = false;
      this.envoltorio = null;
    }
  
    Post(usuario, clave){
      let url = AppSettings.URL_API + 'Login';
      //let url = 'http://api.asambleas.cl/api/login';
  
      let iJson = JSON.stringify({Usuario: usuario, Clave: clave});
  
      return this.http.post(url, iJson, {
        headers: new Headers({'Content-Type': 'application/json'})
      })
        .map(res => res.text())
        .map(res => {
            if (res == "error" || res == "nofound"){
              this.loggedIn = false;
            } else {
              //localStorage.setItem('user_id', res.AutentificacionUsuario.Id);
              //this.username = res.AutentificacionUsuario.NombreUsuario;
              //vamos a dividir el retorno
              let retorno = JSON.parse(res);
              if (retorno.Id > 0) {
                localStorage.setItem('PERSONA', retorno);
                this.envoltorio = retorno;
                this.persona = retorno;
                this.nombreUsuario = usuario;
                localStorage.setItem('USUARIO', this.nombreUsuario);
                this.loggedIn = true;
              }
              else {
                this.loggedIn = false;
              }
            }
            return this.loggedIn;
          }
        );
  
    }
    postProfesores(){
      var usuario = localStorage.getItem("USUARIO");
      let url = AppSettings.URL_API + 'Profesores';
      let dataGet = { Usuario: usuario };
  
      let repos = this.http.post(url, dataGet, {
        headers: new Headers({'Content-Type': 'application/json'})
      });
      return repos;
    }
    postClientes(){
      var usuario = localStorage.getItem("USUARIO");
      let url = AppSettings.URL_API + 'Clientes';
      let dataGet = { Usuario: usuario };
  
      let repos = this.http.post(url, dataGet, {
        headers: new Headers({'Content-Type': 'application/json'})
      });
      return repos;
    }
    postComunas(profesor){
      var usuario = localStorage.getItem("USUARIO");
      let url = AppSettings.URL_API + 'Comunas';
      let dataGet = { IdProfesor: profesor.Id.toString() };
  
      let repos = this.http.post(url, dataGet, {
        headers: new Headers({'Content-Type': 'application/json'})
      });
      return repos;
    }
    putProfesor(idProfesor, rut, nombres, primerApellido, segundoApellido, telefonos, sexo, correo, activo){
      var usuario = localStorage.getItem("USUARIO");
      let url = AppSettings.URL_API + 'Profesores';
      let dataGet = { 
        IdProfesor: idProfesor,
        Rut: rut,
        Nombres: nombres,
        PrimerApellido: primerApellido,
        SegundoApellido: segundoApellido,
        Telefonos: telefonos,
        Sexo: sexo,
        Correo: correo,
        Activo: activo 
      };
  
      let repos = this.http.put(url, dataGet, {
        headers: new Headers({'Content-Type': 'application/json'})
      });
      return repos;
    }
    putCliente(idCliente, nombres, primerApellido, segundoApellido, regId, comId, telefonos, correo, direccion, activo){
      var usuario = localStorage.getItem("USUARIO");
      let url = AppSettings.URL_API + 'Cliente';
      let dataGet = { 
        IdCliente: idCliente,
        Nombres: nombres,
        PrimerApellido: primerApellido,
        SegundoApellido: segundoApellido,
        Telefonos: telefonos,
        RegId: regId,
        ComId: comId,
        Direccion: direccion,
        Correo: correo,
        Activo: activo 
      };
  
      let repos = this.http.put(url, dataGet, {
        headers: new Headers({'Content-Type': 'application/json'})
      });
      return repos;
    }
    putProfesorComunas(idProfesor, idsComunas){
      var usuario = localStorage.getItem("USUARIO");
      let url = AppSettings.URL_API + 'Comunas';
      let dataGet = { 
        IdProfesor: idProfesor,
        Comunas: idsComunas 
      };
  
      let repos = this.http.put(url, dataGet, {
        headers: new Headers({'Content-Type': 'application/json'})
      });
      return repos;
    }
    postComunasRegion(idRegion){
      let url = AppSettings.URL_API + 'Territorio';
      let dataGet = { IdRegion: idRegion.toString() };
  
      let repos = this.http.post(url, dataGet, {
        headers: new Headers({'Content-Type': 'application/json'})
      });
      return repos;
    }    
    logout(): void {
        sessionStorage.clear();
        this.persona = null;
        this.nombreUsuario = '';
      this.loggedIn = false;
    }
    isLoggedId(){
      return this.loggedIn;
    }
    postProductos(){
      var usuario = localStorage.getItem("USUARIO");
      let url = AppSettings.URL_API + 'Productos';
      let dataGet = { Usuario: usuario };
  
      let repos = this.http.post(url, dataGet, {
        headers: new Headers({'Content-Type': 'application/json'})
      });
      return repos;
    }
  
}