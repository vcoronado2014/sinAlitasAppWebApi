import { Component, ViewChild } from '@angular/core';
import { NavController, NavParams, AlertController, ToastController, ModalController, List } from 'ionic-angular';
import { LoadingController } from 'ionic-angular';
import { DetailAgendaPage } from '../../pages/detail-agenda/detail-agenda';
import { HomePage } from '../../pages/home/home';
import { DetailsFichaPage } from '../../pages/details-ficha/details-ficha';
import { CreaAlumnoPage } from '../../pages/crea-alumno/crea-alumno';

import { FichaAlumnoService } from '../../app/Services/FichaAlumnoService';
import {AppSettings} from "../../../AppSettings";

import * as moment from 'moment';

/**
 * Generated class for the FichaAlumnoPage page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */


@Component({
  selector: 'page-ficha-alumno',
  templateUrl: 'ficha-alumno.html',
})
export class FichaAlumnoPage {

  @ViewChild('myList', {read: List}) list: List;

  public fichaAlumnos;
  public cantidadAlumnos;
  public idPack;
  public cantidadAlumnosBd;
  public botonAgregar: boolean;
  public clieId;
    public urlFacebook;
  public urlInstagram;
  public urlFun;

  constructor(
    private nav: NavController,
    private alert: AlertController,
    public loading: LoadingController,
    public toastCtrl: ToastController,
    private navParams: NavParams,
    public modalCtrl: ModalController,
    public ficha: FichaAlumnoService
  ) {

    //aca hay cosas importantes
    //1. si el idPack es mayor a cero, existe.
    //2. si la ficha de alumnos viene vacia se debería consultar mediante el servicio la ficha de alumnos envoltorio

    //this.fichaAlumnos = navParams.get('fichaAlumnos');
        this.urlFacebook = AppSettings.URL_FACEBOOK;
    this.urlInstagram = AppSettings.URL_INSTAGRAM;
    this.urlFun = AppSettings.URL_FUN;
    this.cantidadAlumnos = navParams.get('cantidadAlumnos');
    this.idPack = navParams.get("idPack");
    this.clieId = navParams.get("clieId");
    this.botonAgregar= false;

    let loader = this.loading.create({
      content: 'Cargando...',
    });

    loader.present().then(() => {
      this.ficha.getFichas(this.idPack).subscribe(
        data => {
          this.fichaAlumnos = data.json();
          //aca se debe o no mostrar el boton para agregar mas niños
          if (this.fichaAlumnos){
            this.cantidadAlumnosBd = this.fichaAlumnos.length;

          }
          //mostramos el boton agregar
          if (this.cantidadAlumnosBd != this.cantidadAlumnos){
            //lo vamos a dejar siempre invisible
            this.botonAgregar = false;
          }

        },
        err => console.error(err),
        () => console.log('get ficha completed')
      );



      loader.dismiss();
    });



  }
  presentModal(item) {
    let modal = this.modalCtrl.create(DetailsFichaPage, {fichaAlumno: item });
    modal.present();
    //this.list.closeSlidingItems();
  }
  presentModalNuevo() {
    let modal = this.modalCtrl.create(CreaAlumnoPage, {idPack: this.idPack, clieId: this.clieId });
    modal.present();
  }




}
