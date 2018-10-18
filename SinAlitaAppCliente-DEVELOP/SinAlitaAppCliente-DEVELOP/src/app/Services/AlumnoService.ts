/**
 * Created by root on 27/10/17.
 */
import { Injectable, Component } from '@angular/core';
import { Http, Headers } from '@angular/http';
import { AppSettings } from '../../../AppSettings'

import 'rxjs/add/operator/map';

@Injectable()
export class AlumnoService{

  constructor(
    private http: Http
  ){

  }

  getAlumnos(clieId){
    let url = AppSettings.URL_API + 'Alumnos';
    let dataGet = { ClieId: clieId };

    let repos = this.http.post(url, dataGet, {
      headers: new Headers({'Content-Type': 'application/json'})
    });
    return repos;
  }

}
