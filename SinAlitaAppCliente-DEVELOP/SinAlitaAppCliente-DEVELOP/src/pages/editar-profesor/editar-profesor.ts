import { Component } from '@angular/core';
import { NavController, NavParams, AlertController, ToastController, ViewController, LoadingController, ModalController  } from 'ionic-angular';

import { GlobalService } from '../../app/Services/GlobalService';

import * as moment from 'moment';
import { ProfesoresPage } from '../profesores/profesores';

/**
 * Generated class for the EditarProfesorPage page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */

@Component({
  selector: 'page-editar-profesor',
  templateUrl: 'editar-profesor.html',
})
export class EditarProfesorPage {

  profesor;
  titulo;
  subtitulo;
  //datos del profesor
  rut;
  nombres;
  primerApellido;
  segundoApellido;
  telefonos;
  sexo;
  correo;
  idProfesor;
  profesores;
  //fin datos profesor

  constructor(
    private nav: NavController,
    private alert: AlertController,
    public loading: LoadingController,
    public toastCtrl: ToastController,
    private navParams: NavParams,
    private viewCtrl: ViewController,
    private global: GlobalService,
    private modalCtrl: ModalController,

  ) {
    this.profesor =  navParams.get('profesor');
    if (this.profesor == null){
      this.titulo = "Creado un Profesor";
      this.subtitulo = "Ingresa los datos del Nuevo Profesor."
      this.idProfesor = "0";
    }
    else{
      this.titulo = "Estás editando a " + this.profesor.Nombres;
      this.subtitulo = "Modifica los datos del Profesor " + this.profesor.Nombres;
      this.setearProfesor(this.profesor);

    }

  }
  setearProfesor(profesor) {
    let loader = this.loading.create({
      content: 'Cargando Profesor...',
    });

    loader.present().then(() => {
      this.idProfesor = profesor.Id;
      this.nombres = profesor.Nombres;
      this.rut = profesor.Rut;
      this.primerApellido = profesor.PrimerApellido;
      this.segundoApellido = profesor.SegundoApellido;
      this.sexo = profesor.Sexo;
      this.telefonos = profesor.TelefonosContacto;
      this.correo = profesor.Email;
      loader.dismiss();
    });
  }
  guardarProferor(){

    let loader = this.loading.create({
      content: 'Cargando Profesor...',
    });

    loader.present().then(() => {
      var nuevo = false;
      var activo = "1";
      if (this.profesor == null) {
        nuevo = true;
      }
      var idProfesor = this.idProfesor;
      if (this.rut == ""){
        let toast = this.presentToast("Rut requerido", "bottom", 2000);
        loader.dismiss();
        return;
      }
      if (this.nombres == ""){
        let toast = this.presentToast("Nombres requerido", "bottom", 2000);
        loader.dismiss();
        return;
      }
      if (this.primerApellido == ""){
        let toast = this.presentToast("Primer apellido requerido", "bottom", 2000);
        loader.dismiss();
        return;
      }
      if (this.correo == ""){
        let toast = this.presentToast("Correo electrónico requerido", "bottom", 2000);
        loader.dismiss();
        return;
      }


      this.global.putProfesor(
        this.idProfesor,
        this.rut,
        this.nombres,
        this.primerApellido,
        this.segundoApellido,
        this.telefonos,
        this.sexo,
        this.correo,
        activo
      ).subscribe(
        data => {
          this.profesores = data.json();
        },
        err => {
          console.error(err);
          let toast = this.presentToast("Error al guardar profesor", "top", 2000);
          loader.dismiss();
        },
        () => {
          console.log('save completed');
          let toast = this.presentToast("Profesor guardado con éxito", "top", 2000);
          //ProfesoresPage.cargarProfesores();
          loader.dismiss();
          //volvemos a la página anterior
          this.viewCtrl.dismiss({ profesores: this.profesores });
        }
      );


    });

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
  cancel(){
    this.viewCtrl.dismiss();
  }
  ionViewDidLoad() {
    console.log('ionViewDidLoad EditarProfesorPage');
  }

}
