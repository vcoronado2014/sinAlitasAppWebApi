/**
 * Created by vcoronado on 25-10-2017.
 */
import { Injectable, Component } from '@angular/core';
import { Http, Headers } from '@angular/http';
import { AppSettings } from '../../../AppSettings'

import 'rxjs/add/operator/map';

@Injectable()
export class FichaAlumnoService{

  constructor(
    private http: Http
  ){

  }

  getFichas(idPack){
    let url = AppSettings.URL_API + 'FichaAlumno';
    let dataGet = { Id: idPack };

    let repos = this.http.post(url, dataGet, {
      headers: new Headers({'Content-Type': 'application/json'})
    });
    return repos;
  }
  put(dataPut){
    let url = AppSettings.URL_API + 'FichaAlumno';

    let repos = this.http.put(url, dataPut, {
      headers: new Headers({'Content-Type': 'application/json'})
    });
    return repos;
  }

}
