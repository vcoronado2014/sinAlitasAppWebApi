import { Component } from '@angular/core';
import { NavController, NavParams, AlertController, ToastController, ViewController, LoadingController, ModalController  } from 'ionic-angular';

import { GlobalService } from '../../app/Services/GlobalService';

import * as moment from 'moment';
//modales
import { EditarProfesorPage } from '../../pages/editar-profesor/editar-profesor';
import { AsociarComunasPage } from '../../pages/asociar-comunas/asociar-comunas';

/**
 * Generated class for the ProfesoresPage page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */

@Component({
  selector: 'page-profesores',
  templateUrl: 'profesores.html',
})
export class ProfesoresPage {
  profesoresArr =[];

  constructor(
    private nav: NavController,
    private alert: AlertController,
    public loading: LoadingController,
    public toastCtrl: ToastController,
    private navParams: NavParams,
    private viewCtrl: ViewController,
    private global: GlobalService,
    private modalCtrl: ModalController
  ) {
    //cargamos los profesores
    this.cargarProfesores();
  }
  cargarProfesores() {
    let loader = this.loading.create({
      content: 'Cargando...',
    });
    loader.present().then(() => {
      this.global.postProfesores().subscribe(
        data => {
          this.profesoresArr = data.json();
        },
        err => console.error(err),
        () => console.log('get alumnos completed')
      );
      loader.dismiss();
    });
  }
  doRefresh(refresher) {
    console.log('Begin async operation', refresher);

    setTimeout(() => {
      this.cargarProfesores();
      refresher.complete();
    }, 2000);
  }

  ionViewDidLoad() {
    console.log('ionViewDidLoad ProfesoresPage');
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
  asociarComunas(profesor){
    let modal = this.modalCtrl.create(AsociarComunasPage, {profesor: profesor });
    modal.present();
  }
  //acciones
  nuevoProfesor(){
    //let mensaje = this.presentToast("Levantar pantalla nuevo profesor", "bottom", 2000);
    let modal = this.modalCtrl.create(EditarProfesorPage, {profesor: null });
    modal.onDidDismiss(data => {
      // Data is your data from the modal
      if (data != undefined){
        this.profesoresArr = data.profesores;
        //this.cargarProfesores();
      }
    });
    modal.present();

  }
  editarProfesor(profesor){
    //let mensaje = this.presentToast("Levantar pantalla editar profesor " + profesor.Nombres, "bottom", 2000);
    let modal = this.modalCtrl.create(EditarProfesorPage, {profesor: profesor });
    modal.onDidDismiss(data => {
      // Data is your data from the modal
      if (data != undefined){
        this.profesoresArr = data.profesores;
        //this.cargarProfesores();
      }
    });
    modal.present();

  }
  desactivarProfesor(profesor){
    
      const confirm = this.alert.create({
        title: 'Desactivar Profesor',
        message: '¿Esta seguro de desactivar al Profesor ' + profesor.Nombres + ' ' + profesor.PrimerApellido + ' ' + profesor.SegundoApellido + '?',
        buttons: [
          {
            text: 'No',
            handler: () => {
              console.log('Disagree clicked');
            }
          },
          {
            text: 'Si',
            handler: () => {
              console.log('Agree clicked');
              //let mensaje = this.presentToast("Desactivar profesor " + profesor.Nombres, "bottom", 2000);
              profesor.Activo = 0;
              profesor.Eliminado = 1;
              this.guardarProferor(profesor, "Profesor desactivado con éxito.")

            }
          }
        ]
      });
      confirm.present();
  }
  activarProfesor(profesor){
    const confirm = this.alert.create({
      title: 'Activar Profesor',
      message: '¿Esta seguro de activar al Profesor ' + profesor.Nombres + ' ' + profesor.PrimerApellido + ' ' + profesor.SegundoApellido + '?',
      buttons: [
        {
          text: 'No',
          handler: () => {
            console.log('Disagree clicked');
          }
        },
        {
          text: 'Si',
          handler: () => {
            console.log('Agree clicked');
            profesor.Activo = 1;
            profesor.Eliminado = 0;
            this.guardarProferor(profesor, "Profesor activado con éxito.")
            //let mensaje = this.presentToast("Activar profesor " + profesor.Nombres, "bottom", 2000);
          }
        }
      ]
    });
    confirm.present();
    

  }
  guardarProferor(profesor, mensaje){

    let loader = this.loading.create({
      content: 'Cargando Profesor...',
    });

    loader.present().then(() => {
      this.global.putProfesor(
        profesor.Id.toString(),
        profesor.Rut,
        profesor.Nombres,
        profesor.PrimerApellido,
        profesor.SegundoApellido,
        profesor.TelefonosContacto,
        profesor.Sexo,
        profesor.Email,
        profesor.Activo.toString()
      ).subscribe(
        data => {
          this.profesoresArr = data.json();
        },
        err => {
          console.error(err);
          let toast = this.presentToast("error al realizar la operación.", "top", 2000);
          loader.dismiss();
        },
        () => {
          console.log('save completed');
          let toast = this.presentToast(mensaje, "top", 2000);
          //ProfesoresPage.cargarProfesores();
          loader.dismiss();
        }
      );


    });

  }

}
