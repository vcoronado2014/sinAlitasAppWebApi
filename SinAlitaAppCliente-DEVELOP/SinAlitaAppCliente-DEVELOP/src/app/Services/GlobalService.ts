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
    logout(): void {
        sessionStorage.clear();
        this.persona = null;
        this.nombreUsuario = '';
      this.loggedIn = false;
    }
    isLoggedId(){
      return this.loggedIn;
    }
  
}