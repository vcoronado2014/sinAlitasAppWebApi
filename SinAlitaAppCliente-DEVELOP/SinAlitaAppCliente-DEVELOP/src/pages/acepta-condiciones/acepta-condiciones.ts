import { Component } from '@angular/core';
import { NavController, NavParams, AlertController, ToastController, ViewController } from 'ionic-angular';
import { LoadingController } from 'ionic-angular';

import { DetailPackPage } from '../../pages/detail-pack/detail-pack';
import { HomePage } from '../../pages/home/home';

import {AceptaCondicionesService} from '../../app/Services/AceptaCondicionesService';
import {AppSettings} from "../../../AppSettings";

/**
 * Generated class for the AceptaCondicionesPage page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */

@Component({
  selector: 'page-acepta-condiciones',
  templateUrl: 'acepta-condiciones.html',
  providers: [AceptaCondicionesService]
})
export class AceptaCondicionesPage {
  public idPack;
  public codigoCliente;
    public urlFacebook;
  public urlInstagram;
  public urlFun;
  constructor(
    private nav: NavController,
    private alert: AlertController,
    public loading: LoadingController,
    public toastCtrl: ToastController,
    private navParams: NavParams,
    private viewCtrl: ViewController,
    public acc: AceptaCondicionesService
  ) {
    this.urlFacebook = AppSettings.URL_FACEBOOK;
    this.urlInstagram = AppSettings.URL_INSTAGRAM;
    this.urlFun = AppSettings.URL_FUN;
    this.idPack = navParams.get('id');
    this.codigoCliente = navParams.get('codigoCliente');
  }

  ionViewDidLoad() {
    console.log('ionViewDidLoad AceptaCondicionesPage');
  }

  atras(){
    //aca hay que ir a la pagina anterior con push
    //this.viewCtrl.dismiss();
    this.nav.setRoot(HomePage);
  }
  aceptar(){
    //acepta las condiciones del servicio
    this.acc.put(this.idPack).subscribe(
      data => {
        if (data.status == 200) {
          //todo ok
          let mi = this.presentToast('Condiciones aceptadas.', 'top', 5000);
          //this.navCtrl.setRoot(UsuariosPage);
          this.nav.setRoot(HomePage, {idPack: this.idPack, codigoCliente: this.codigoCliente});


        }
        else {
          let mi = this.presentToast('Error al guardar.', 'bottom', 4000);
          this.nav.setRoot(HomePage);
        }
      },
      err => console.error(err),
      () => console.log('ok')
    );

  }
  presentToast = function(mensaje, posicion, duracion) {
    let toast = this.toastCtrl.create({
      message: mensaje,
      duration: duracion,
      position: posicion
    });

    toast.onDidDismiss(() => {
      console.log('Dismissed toast');
    });

    toast.present();
  }
}
