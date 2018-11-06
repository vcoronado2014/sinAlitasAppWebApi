import { Component } from '@angular/core';
import { NavController, NavParams, AlertController, ToastController, ViewController, LoadingController, ModalController  } from 'ionic-angular';

import { GlobalService } from '../../app/Services/GlobalService';

import * as moment from 'moment';
//modales
import { EditarClientePage } from '../../pages/editar-cliente/editar-cliente';
import { CrearPackPage } from '../../pages/crear-pack/crear-pack';

/**
 * Generated class for the ClientesPage page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */

@Component({
  selector: 'page-clientes',
  templateUrl: 'clientes.html',
})
export class ClientesPage {
  clientesArr =[];
  todasLosClientes;
  nombreBuscar;
  encontrados;

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
    this.todasLosClientes=[];
    this.encontrados = 0;
    this.cargarProfesores();
  }
  cargarProfesores() {
    let loader = this.loading.create({
      content: 'Cargando...',
    });
    loader.present().then(() => {
      this.global.postClientes().subscribe(
        data => {
          var datos = data.json();
          if (datos){
            datos.forEach(element => {
              element.NombreCompleto = element.Nombres.trim() + ' ' + element.PrimerApellido.trim() + ' ' + element.SegundoApellido.trim();
            });
          }
          this.clientesArr = datos;
          this.todasLosClientes = this.clientesArr;
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

  ionViewDidLoad() {
    console.log('ionViewDidLoad ClientesPage');
  }
  getClientesNombres() {
    //"Blue Whale".indexOf("Blue")
    if (this.nombreBuscar && this.nombreBuscar.length > 2) {
      let clientesDevolver = [];
      let clientes = this.clientesArr;
      let cliente = clientes.filter(item => item.NombreCompleto == this.nombreBuscar);
      for (let i = 0; i <= this.clientesArr.length; i++) {
        let objComuna = this.clientesArr[i];
        if (objComuna) {
          //if (objComuna.Nombre == this.nombreBuscar) {
          if (objComuna.NombreCompleto.toUpperCase().indexOf(this.nombreBuscar.toUpperCase()) >= 0) {
            this.clientesArr[i].visible = true;
            clientesDevolver.push(objComuna);
          }
        }
      }
      if (clientesDevolver && clientesDevolver.length) {
        this.clientesArr = clientesDevolver;
        this.encontrados = clientesDevolver.length;
      }
    }
    //return this.comunasGeneral;

  } 
  limpiarClientes(){
    //nombre = '';
    this.nombreBuscar = '';
    this.clientesArr = this.todasLosClientes;
    this.encontrados = 0;
  }
  //acciones
  nuevoCliente(){
    //let mensaje = this.presentToast("Levantar pantalla nuevo profesor", "bottom", 2000);
    let modal = this.modalCtrl.create(EditarClientePage, {cliente: null });
    modal.onDidDismiss(data => {
      // Data is your data from the modal
      if (data != undefined){
        this.clientesArr = data.clientes;
        //this.cargarProfesores();
      }
    });
    modal.present();
  }
  nuevoPack(cliente){
    //let mensaje = this.presentToast("Levantar pantalla nuevo profesor", "bottom", 2000);
    let modal = this.modalCtrl.create(CrearPackPage, {cliente: cliente });
    modal.onDidDismiss(data => {
      // Data is your data from the modal
      if (data != undefined){
        //this.clientesArr = data.clientes;
      }
    });
    modal.present();
  }
  editarCliente(cliente){
    //let mensaje = this.presentToast("Levantar pantalla editar profesor " + profesor.Nombres, "bottom", 2000);
    let modal = this.modalCtrl.create(EditarClientePage, {cliente: cliente });
    modal.onDidDismiss(data => {
      // Data is your data from the modal
      if (data != undefined){
        this.clientesArr = data.clientes;
        //this.cargarProfesores();
      }
    });
    modal.present();

  }  


}
