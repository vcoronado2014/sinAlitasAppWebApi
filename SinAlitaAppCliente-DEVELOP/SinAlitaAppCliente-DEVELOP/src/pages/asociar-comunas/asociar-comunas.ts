import { Component } from '@angular/core';
import { NavController, NavParams, AlertController, ToastController, ViewController, LoadingController, ModalController  } from 'ionic-angular';

import { GlobalService } from '../../app/Services/GlobalService';

import * as moment from 'moment';
import { THIS_EXPR } from '@angular/compiler/src/output/output_ast';

/**
 * Generated class for the AsociarComunasPage page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */

@Component({
  selector: 'page-asociar-comunas',
  templateUrl: 'asociar-comunas.html',
})
export class AsociarComunasPage {
  profesor;
  titulo;
  subtitulo;
  comunasArr;
  comunasProfesorArr;
  comunasGeneral;
  selectedArray :any = [];
  seleccionadas;
  todasLasComunas;
  nombreBuscar;
  encontrados;

  constructor(    private nav: NavController,
    private alert: AlertController,
    public loading: LoadingController,
    public toastCtrl: ToastController,
    private navParams: NavParams,
    private viewCtrl: ViewController,
    private global: GlobalService,
    private modalCtrl: ModalController, ) {
    this.profesor = navParams.get('profesor');

      this.seleccionadas = 0;
      this.titulo = "Agregar a " + this.profesor.Nombres;
      this.subtitulo = "Seleccione las comunas disponibles y luego presione el botón Guardar.";
      this.comunasArr = [];
      this.comunasProfesorArr=[];
      this.comunasGeneral = [];
      this.todasLasComunas=[];
      this.encontrados = 0;
      
      this.cargarComunas(this.profesor);

  }
  limpiarComunas(){
    //nombre = '';
    this.nombreBuscar = '';
    this.comunasGeneral = this.todasLasComunas;
    this.encontrados = 0;
  }

  cargarComunas(profesor) {
    let loader = this.loading.create({
      content: 'Cargando...',
    });
    loader.present().then(() => {
      this.global.postComunas(profesor).subscribe(
        data => {
          var datos = data.json();
          if (datos.ComunasDisponibles){
            datos.ComunasDisponibles.forEach(element => {
              var entidad = {
                Nombre: element.Nombre,
                Id: element.Id,
                check: false,
                visible: true
              };
              //this.comunasArr.push(entidad);
              this.comunasGeneral.push(entidad);

            });
          }
          if (datos.ComunasProfesor){
            datos.ComunasProfesor.forEach(element => {
              var entidad = {
                Nombre: element.Nombre,
                Id: element.Id,
                check: true,
                visible: true
              };
              //this.comunasProfesorArr.push(entidad);
              this.comunasGeneral.push(entidad);
              this.selectedArray.push(entidad);
            });
          }
          //this.selectedArray = this.comunasProfesorArr;
          this.seleccionadas = this.selectedArray.length; 
          //this.comunasArr = datos.ComunasDisponibles;
          //this.comunasProfesorArr = datos.ComunasProfesor;
          this.todasLasComunas = this.comunasGeneral;
        },
        err => console.error(err),
        () => console.log('get comunas completed')
      );
      loader.dismiss();
    });
  }
  ionViewDidLoad() {
    console.log('ionViewDidLoad AsociarComunasPage');
  }
  cancel(){
    this.viewCtrl.dismiss();
  }
  selectMember(data) {
    if (data.check == true) {
      this.selectedArray.push(data);
    } else {
      let newArray = this.selectedArray.filter(function (el) {
        return el.Id !== data.Id;
      });
      this.selectedArray = newArray;
    }
    console.log(this.selectedArray);
    this.seleccionadas = this.selectedArray.length;
  }
  guardar(){

    let loader = this.loading.create({
      content: 'Guardando Profesor...',
    });

    loader.present().then(() => {
      var nuevo = false;
      var activo = "1";
      if (this.profesor == null) {
        nuevo = true;
      }
      var idProfesor = this.profesor.Id.toString();
      var ids = this.obtenerIdsString();
      if (ids == ""){
        let toast = this.presentToast("Debes seleccionar comunas a agregar", "bottom", 2000);
        loader.dismiss();
        return;
      }



      this.global.putProfesorComunas(
        idProfesor,
        ids
        
      ).subscribe(
        data => {
         //this.profesores = data.json();
         //nueva data
        },
        err => {
          console.error(err);
          let toast = this.presentToast("Error al guardar comunas del profesor", "top", 2000);
          loader.dismiss();
        },
        () => {
          console.log('save completed');
          let toast = this.presentToast("Comunas del Profesor guardadas con éxito", "top", 2000);
          //ProfesoresPage.cargarProfesores();
          loader.dismiss();
          //volvemos a la página anterior
          this.viewCtrl.dismiss();
        }
      );


    });
    /*
        console.log(this.profesor);
        console.log(this.selectedArray);
        console.log(this.obtenerIdsString());
        let mensaje = this.presentToast("Guardar las comunas del profesor", "bottom", 2000);
    */
  }
  obtenerIdsString(){
    let ids= '';
    if (this.selectedArray.length > 0){
      for (var i=0; i <= this.selectedArray.length; i++){
        var obj = this.selectedArray[i];
        if (obj != undefined){
          ids = ids + ',' + obj.Id.toString();
        }
      }
    }
    return ids;
  }
  getComunasNombres() {
    //"Blue Whale".indexOf("Blue")
    if (this.nombreBuscar && this.nombreBuscar.length > 2) {
      let comunasDevolver = [];
      let comunas = this.comunasGeneral;
      let comuna = comunas.filter(item => item.Nombre == this.nombreBuscar);
      for (let i = 0; i <= this.comunasGeneral.length; i++) {
        let objComuna = this.comunasGeneral[i];
        if (objComuna) {
          //if (objComuna.Nombre == this.nombreBuscar) {
          if (objComuna.Nombre.toUpperCase().indexOf(this.nombreBuscar.toUpperCase()) >= 0) {
            this.comunasGeneral[i].visible = true;
            comunasDevolver.push(objComuna);
          }
        }
      }
      if (comunasDevolver && comunasDevolver.length) {
        this.comunasGeneral = comunasDevolver;
        this.encontrados = comunasDevolver.length;
      }
    }
    //return this.comunasGeneral;

  } 
/*
  getComunasNombres(nombre) {
    if (nombre == '') {
      this.limpiarComunas();
    }
    else {
      let comunasDevolver = [];
      let comunas = this.comunasGeneral;
      let comuna = comunas.filter(item => item.Nombre == nombre);
      for (let i = 0; i <= this.comunasGeneral.length; i++) {
        let objComuna = this.comunasGeneral[i];
        if (objComuna) {
          if (objComuna.Nombre == nombre) {
            this.comunasGeneral[i].visible = true;
            comunasDevolver.push(objComuna);
          }
        }
      }
      if (comunasDevolver && comunasDevolver.length) {
        this.comunasGeneral = comunasDevolver;
      }
    }
    //return this.comunasGeneral;

  } 
*/
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

  /*
  checkAll(){
    for(let i =0; i <= this.selectedArray.length; i++) {
      this.selectedArray[i].checked = true;
    }
   console.log(this.selectedArray);
   this.seleccionadas = this.selectedArray.length;
  }
  */

}
