/**
 * Created by VICTOR CORONADO on 28/10/2017.
 */
import { Injectable, Component } from '@angular/core';
import { Http, Headers } from '@angular/http';
import { AppSettings } from '../../../AppSettings'

import 'rxjs/add/operator/map';
@Injectable()
export class AceptaCondicionesService{

  constructor(
    private http: Http
  ){

  }

  put(clieId){
    let url = AppSettings.URL_API + 'AceptaCondiciones';
    let dataPut = { IdPack: clieId };

    let repos = this.http.put(url, dataPut, {
      headers: new Headers({'Content-Type': 'application/json'})
    });
    return repos;
  }

}
