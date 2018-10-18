import { Component } from '@angular/core';
import { NavController, NavParams, AlertController, ToastController, ViewController } from 'ionic-angular';
import { LoadingController } from 'ionic-angular';
import {AppSettings} from "../../../AppSettings";

/**
 * Generated class for the DetailsFichaPage page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */

@Component({
  selector: 'page-details-ficha',
  templateUrl: 'details-ficha.html',
})
export class DetailsFichaPage {
  public fichaAlumno;

  public nombreCompleto;
  public edad;
  public sexo;

  public tieneAsma;
  public tieneProblemasCardiacos;
  public tieneProblemasMotores;

  public cualesProblemasCardiacos;
  public cualesProblemasMotores;
  public dondeAcudir;
  public numeroEmergencia;
  public observacion;
  public otraEnfermedad;
  pet: string = "puppies";
    public urlFacebook;
  public urlInstagram;
  public urlFun;


  constructor(
    private nav: NavController,
    private alert: AlertController,
    public loading: LoadingController,
    public toastCtrl: ToastController,
    private navParams: NavParams,
    private viewCtrl: ViewController
  ) {

    this.fichaAlumno = navParams.get('fichaAlumno');
        this.urlFacebook = AppSettings.URL_FACEBOOK;
    this.urlInstagram = AppSettings.URL_INSTAGRAM;
    this.urlFun = AppSettings.URL_FUN;
    let loader = this.loading.create({
      content: 'Cargando...',
    });

    loader.present().then(() => {
      if (this.fichaAlumno){
        this.nombreCompleto = this.fichaAlumno.NombreCompleto;
        this.edad = this.fichaAlumno.Edad;
        this.sexo = this.fichaAlumno.Sexo;

        if (this.fichaAlumno.TieneAsma == 1){
          this.tieneAsma = true;
        }
        else {
          this.tieneAsma = false;
        }

        if (this.fichaAlumno.TieneProblemasCardiacos == 1){
          this.tieneProblemasCardiacos = true;
          this.cualesProblemasCardiacos = this.fichaAlumno.CualesProblemasCardiacos;
        }
        else {
          this.tieneProblemasCardiacos = false;
          this.cualesProblemasCardiacos = "No tiene";
        }

        if (this.fichaAlumno.TieneProblemasMotores == 1){
          this.tieneProblemasMotores = true;
          this.cualesProblemasMotores = this.fichaAlumno.CualesProblemasMotores;
        }
        else {
          this.tieneProblemasMotores = false;
          this.cualesProblemasMotores = "No tiene";
        }



        this.dondeAcudir = this.fichaAlumno.DondeAcudir;
        this.numeroEmergencia = this.fichaAlumno.NumeroEmergencia;
        this.observacion = this.fichaAlumno.Observacion;
        this.otraEnfermedad = this.fichaAlumno.OtraEnfermedad;

      }
      loader.dismiss();
    });


  }

  close(){
    this.viewCtrl.dismiss();
  }

}
