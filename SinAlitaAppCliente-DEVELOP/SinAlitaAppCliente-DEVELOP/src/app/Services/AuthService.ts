/**
 * Created by vcoronado on 13-07-2017.
 */
import { Injectable, Component } from '@angular/core';
import { Http, Headers } from '@angular/http';
import { AppSettings } from '../../../AppSettings'

import 'rxjs/add/operator/map';

@Injectable()
export class AuthService{
  codigoCliente: string;
  envoltorio: any;
  loggedIn:boolean;

  constructor(
    private http: Http
  ){
    //inicializamos los valores
    this.codigoCliente = '';
    this.loggedIn = false;
    this.envoltorio = null;
  }

  Post(codigoCliente){
    let url = AppSettings.URL_API + 'Envoltorio';
    //let url = 'http://api.asambleas.cl/api/login';

    let iJson = JSON.stringify({Nombre: codigoCliente});

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
            if (retorno.ProductoCodigo.Id > 0) {
              this.envoltorio = retorno;
              this.codigoCliente = codigoCliente;
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
  logout(): void  {
    sessionStorage.clear();

    this.codigoCliente = '';
    this.loggedIn = false;
  }
  isLoggedId(){
    return this.loggedIn;
  }

}
