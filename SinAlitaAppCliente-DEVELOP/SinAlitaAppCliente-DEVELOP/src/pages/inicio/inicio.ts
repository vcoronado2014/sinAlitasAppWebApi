import { Component } from '@angular/core';
import {NavController, Toast, NavParams} from 'ionic-angular';
import { ToastController } from 'ionic-angular';
import { DetailPackPage } from '../../pages/detail-pack/detail-pack';
import { HomePage } from '../../pages/home/home';
import { LoginPage } from '../../pages/login/login';
//para redes sociales
import { AppAvailability } from '@ionic-native/app-availability';
import { Platform } from 'ionic-angular';

import { AuthService } from '../../app/Services/AuthService';
import {AppSettings} from "../../../AppSettings";

/**
 * Generated class for the InicioPage page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */

@Component({
  selector: 'page-inicio',
  templateUrl: 'inicio.html',
  providers: [AuthService, AppAvailability]
})
export class InicioPage {
  isLogged: boolean;
  envoltorio: any;
  icono;
  //para las redes sociales
  public scheme: String;
  public isApp;
  public urlFacebook;
  public urlInstagram;
  public urlFun;

  constructor(public navCtrl: NavController,
    private navParams: NavParams,
    public auth: AuthService,
    public toastCtrl: ToastController,
    public appAvailability: AppAvailability,
    public platform: Platform) {
      this.icono = AppSettings.URL_FOTOS + 'img/sinalitas.png';
      this.urlFacebook = AppSettings.URL_FACEBOOK;
      this.urlInstagram = AppSettings.URL_INSTAGRAM;
      this.urlFun = AppSettings.URL_FUN;
      /*
      if (this.navParams.get('codigoCliente') != null) {
        this.codigoCliente = this.navParams.get('codigoCliente');
      }
      */

  }

  ionViewDidLoad() {
    console.log('ionViewDidLoad InicioPage');
  }
  abrirCliente(){
    this.navCtrl.setRoot(HomePage);
  }
  abrirLogin(){
    this.navCtrl.setRoot(LoginPage);
  }

}
