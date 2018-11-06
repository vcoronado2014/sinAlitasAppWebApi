import { Component } from '@angular/core';
import { NavController, NavParams, AlertController, ToastController, ViewController, LoadingController, ModalController  } from 'ionic-angular';

import { GlobalService } from '../../app/Services/GlobalService';

import * as moment from 'moment';


/**
 * Generated class for the EditarClientePage page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */

@Component({
  selector: 'page-editar-cliente',
  templateUrl: 'editar-cliente.html',
})
export class EditarClientePage {
  cliente;
  titulo;
  subtitulo;
  nombres;
  primerApellido;
  segundoApellido;
  correo;
  telefono;
  direccion;
  idCliente;
  comunasArr=[];
  region;
  comuna;
  clientes=[];


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
    this.cliente =  navParams.get('cliente');
    if (this.cliente == null){
      this.titulo = "Creado un Cliente";
      this.subtitulo = "Ingresa los datos del Nuevo Cliente."
      this.idCliente = "0";
    }
    else{
      this.titulo = "Estás editando a " + this.cliente.NombreCompleto;
      this.subtitulo = "Modifica los datos del Cliente " + this.cliente.NombreCompleto;
      this.setearCliente(this.cliente);

    }    

  }

  
  cargarRegiones() {
    let loader = this.loading.create({
      content: 'Cargando...',
    });
    loader.present().then(() => {
      this.global.postComunasRegion(this.region).subscribe(
        data => {
          this.comunasArr = data.json();
          
        },
        err => console.error(err),
        () => console.log('get comunas completed')
      );
      loader.dismiss();
    });
  }

  setearCliente(cliente) {
    let loader = this.loading.create({
      content: 'Cargando Profesor...',
    });

    loader.present().then(() => {
      this.idCliente = cliente.Id;
      this.nombres = cliente.Nombres;
      this.primerApellido = cliente.PrimerApellido;
      this.segundoApellido = cliente.SegundoApellido;
      this.telefono = cliente.TelefonosContacto;
      this.correo = cliente.Email;
      this.direccion = cliente.Direccion;
      this.region = cliente.RegId;
      //cargamos las regiones
      this.global.postComunasRegion(cliente.RegId).subscribe(
        data => {
          this.comunasArr = data.json();
          this.comuna = cliente.ComId;
        },
        err => console.error(err),
        () => console.log('get comunas completed')
      );
      loader.dismiss();
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
    console.log('ionViewDidLoad EditarClientePage');
  }

  guardarCliente(){

    let loader = this.loading.create({
      content: 'Cargando Profesor...',
    });

    loader.present().then(() => {
      var nuevo = false;
      var activo = "1";
      if (this.cliente == null) {
        nuevo = true;
      }
      var idCliente = this.idCliente;

      if (!this.nombres  || this.nombres == ""){
        let toast = this.presentToast("Nombres requerido", "bottom", 2000);
        loader.dismiss();
        return;
      }
      if (!this.primerApellido  || this.primerApellido == ""){
        let toast = this.presentToast("Primer apellido requerido", "bottom", 2000);
        loader.dismiss();
        return;
      }
      if (!this.region  || this.region == ""){
        let toast = this.presentToast("Region requerida", "bottom", 2000);
        loader.dismiss();
        return;
      }
      if (!this.comuna  || this.comuna == ""){
        let toast = this.presentToast("Comuna requerida", "bottom", 2000);
        loader.dismiss();
        return;
      }
      if (!this.direccion  || this.direccion == ""){
        let toast = this.presentToast("Dirección requerida", "bottom", 2000);
        loader.dismiss();
        return;
      }
      if (!this.telefono  || this.telefono == ""){
        let toast = this.presentToast("Teléfono requerido", "bottom", 2000);
        loader.dismiss();
        return;
      }
      if (!this.correo  || this.correo == ""){
        let toast = this.presentToast("Correo electrónico requerido", "bottom", 2000);
        loader.dismiss();
        return;
      }


      this.global.putCliente(
        this.idCliente,
        this.nombres,
        this.primerApellido,
        this.segundoApellido,
        this.region,
        this.comuna,
        this.telefono,
        this.correo,
        this.direccion,
        activo
      ).subscribe(
        data => {
          var datos = data.json();
          if (datos){
            datos.forEach(element => {
              element.NombreCompleto = element.Nombres.trim() + ' ' + element.PrimerApellido.trim() + ' ' + element.SegundoApellido.trim();
            });
          }
          this.clientes = datos;
          //this.todasLosClientes = this.clientesArr;
        },
        err => {
          console.error(err);
          let toast = this.presentToast("Error al guardar cliente", "top", 2000);
          loader.dismiss();
        },
        () => {
          console.log('save completed');
          let toast = this.presentToast("Cliente guardado con éxito", "top", 2000);
          //ProfesoresPage.cargarProfesores();
          loader.dismiss();
          //volvemos a la página anterior
          this.viewCtrl.dismiss({ clientes: this.clientes });
        }
      );



    });

  }
}
