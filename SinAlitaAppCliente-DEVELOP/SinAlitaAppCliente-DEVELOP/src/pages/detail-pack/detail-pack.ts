import { Component, ViewChild  } from '@angular/core';
import { List } from 'ionic-angular';
import { NavController, NavParams, AlertController, ToastController } from 'ionic-angular';
import { LoadingController } from 'ionic-angular';
import { SplashScreen } from '@ionic-native/splash-screen';
import { DetailAgendaPage } from '../../pages/detail-agenda/detail-agenda';
import { HomePage } from '../../pages/home/home';
import { FichaAlumnoPage } from '../../pages/ficha-alumno/ficha-alumno';
import { AceptaCondicionesPage } from '../../pages/acepta-condiciones/acepta-condiciones';
import { CreaAlumnoPage } from '../../pages/crea-alumno/crea-alumno';

import * as moment from 'moment';
import {AppSettings} from "../../../AppSettings";

/**
 * Generated class for the DetailPackPage page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */

@Component({
  selector: 'page-detail-pack',
  templateUrl: 'detail-pack.html'
})
export class DetailPackPage {
  @ViewChild('myList', {read: List}) list: List;
  //@ViewChild('item') slidingItem: ItemSliding;

  public idElemento;
  public envoltorio;
  public clasesAgendadas;
  public nombreProfesor;
  public emailProfesor;
  public sexoProfesor;
  public fonoProfesor;
  public fotoProfesor;
  public nombreCliente;
  public direccionCliente;
  public comunaCliente;
  public telefonosCliente;
  public fechaPack;
  public infoProfesor;
  public cantidadAlumnos;
  public fichaAlumnos;
  public idPack;
  public clieId;
  public codigoCliente;
  //para controlar
  public puedeVerAgenda;
  public tieneAlerta;
  public mensajeAlerta;
  public alumnosCreados;
  public cuposCreados;
  public arrCupos;
  public urlFacebook;
  public urlInstagram;
  public urlFun;
  constructor(
    private nav: NavController,
    private alert: AlertController,
    public loading: LoadingController,
    public toastCtrl: ToastController,
    private navParams: NavParams,
    private splashScreen: SplashScreen
  ) {
      this.idElemento = navParams.get('id');
      this.envoltorio = navParams.get('envoltorio');
      this.clasesAgendadas = 0;
      this.cuposCreados = 0;

      this.nombreProfesor = '';
      this.emailProfesor = '';
      this.sexoProfesor = '';
      this.fonoProfesor = '';
      this.fotoProfesor = '';
      //cliente
      this.nombreCliente= '';
      this.direccionCliente= '';
      this.comunaCliente= '';
      this.telefonosCliente= '';
      this.infoProfesor='';

      this.fechaPack = '';
      this.fichaAlumnos= [];
      this.idPack = 0;
      this.clieId = 0;
      this.codigoCliente="";

      this.puedeVerAgenda = false;
      this.tieneAlerta = false;
      this.alumnosCreados = 0;
      this.mensajeAlerta="";

      this.arrCupos = [];


    //this.splashScreen.show();

    let loader = this.loading.create({
      content: 'Cargando...',
    });

    loader.present().then(() => {
          this.urlFacebook = AppSettings.URL_FACEBOOK;
    this.urlInstagram = AppSettings.URL_INSTAGRAM;
    this.urlFun = AppSettings.URL_FUN;

      //aca las llamadas ajax
      //se deben profesar ciertos parametros
      //1. si tiene o no acepta condiciones, si tiene todo bien de lo contrario se va a la pagina de acepta condiciones
      if (this.envoltorio){
        //el identificador del pack
        this.idPack = this.envoltorio.ProductoCodigo.Id;
        this.codigoCliente = this.envoltorio.ProductoCodigo.CodigoCliente;
        //tiene acepta condiciones
        if (this.envoltorio.TieneAceptaCondiciones){
          this.cantidadAlumnos = this.envoltorio.ProductoCodigo.CantidadAlumnos;
          this.clasesAgendadas = this.envoltorio.ProductoCodigo.CantidadClases;
          //ahora si tiene profesor
          if (this.envoltorio.Profesor.Id > 0){
            this.nombreProfesor = this.envoltorio.Profesor.Nombres + ' ' +  this.envoltorio.Profesor.PrimerApellido + ' ' + this.envoltorio.Profesor.SegundoApellido;
            this.emailProfesor = this.envoltorio.Profesor.Email;
            this.sexoProfesor = this.envoltorio.Profesor.Sexo;
            this.fonoProfesor = this.envoltorio.Profesor.TelefonosContacto;
            this.infoProfesor =  this.emailProfesor + ', ' + this.fonoProfesor;
            if (this.envoltorio.Profesor.Fotografia == ''){
              this.fotoProfesor = AppSettings.URL_FOTOS + "img/no_foto.png";
            }
            else {
              this.fotoProfesor = AppSettings.URL_FOTOS + "img/" + this.envoltorio.Profesor.Fotografia;
            }

          }
          else {
            //no hay profesor asignado aun
            this.nombreProfesor = 'No hay profesor asignado';
            this.fotoProfesor = AppSettings.URL_FOTOS + "img/no_foto.png";
            this.infoProfesor = '';
          }
          if (this.envoltorio.Cupos){
            if (this.envoltorio.Cupos.length > 0){
              this.cuposCreados = this.envoltorio.Cupos.length;
              this.puedeVerAgenda = true;
            }
          }
          //cliente
          if (this.envoltorio.Cliente.Id > 0){
            this.clieId = this.envoltorio.Cliente.Id;
            this.nombreCliente = this.envoltorio.Cliente.Nombres + ' ' +  this.envoltorio.Cliente.PrimerApellido + ' ' + this.envoltorio.Cliente.SegundoApellido;
            this.direccionCliente = this.envoltorio.Cliente.Direccion;
            this.comunaCliente = this.envoltorio.ComunaCliente.Nombre;
            this.telefonosCliente = this.envoltorio.Cliente.TelefonosContacto;
          }
          this.codigoCliente = this.envoltorio.ProductoCodigo.CodigoCliente;
          //fecha creacion
          let fecha = moment(this.envoltorio.ProductoCodigo.FechaCreacion).format('DD-MM-YYYY HH:MM');
          this.fechaPack = fecha;

        }
        else{
          //enviar a la pagina de acpeta condiciones con nav.push
          this.goToAceptaCondiciones(this.idPack);

        }
        //ficha Alumnos
        if (this.envoltorio.FichaAlumnos){
          this.fichaAlumnos = this.envoltorio.FichaAlumnos;
          if (this.fichaAlumnos.length > 0){
            this.alumnosCreados = this.fichaAlumnos.length;
          }
        }
        //controles
        //puede ver agenda cuando la cantidad de alumnos creados sea igual a la programada
        if (this.alumnosCreados != this.cantidadAlumnos){
          this.puedeVerAgenda = false;
          this.tieneAlerta = true;
          var dif = this.cantidadAlumnos - this.alumnosCreados;
          var sms = "No puede ver su agenda hasta que cree la ficha de " + dif.toString() + " alumnos.";
          this.mensajeAlerta = sms;
        }
        if (this.cuposCreados == 0){
          this.puedeVerAgenda = false;
        }
        if (this.envoltorio.Cupos){
          this.arrCupos = this.envoltorio.Cupos;
        }

      }
      else {
        //redirect a home
        nav.setRoot(HomePage);
      }

    //this.splashScreen.hide();

      loader.dismiss();
    });
  }
  goToAceptaCondiciones(id){
    //this.nav.push(AceptaCondicionesPage, {id: id });
    this.nav.setRoot(AceptaCondicionesPage, {id: id, codigoCliente: this.codigoCliente });
  }
  goToDetails(id){
    this.nav.push(DetailAgendaPage, {id: id, cupos: this.arrCupos, profesor: this.envoltorio.Profesor, codigoPack: this.codigoCliente });
  }
  goToCrearAlumno(){
    this.nav.push(CreaAlumnoPage, {idPack: this.idPack, clieId: this.clieId, codigoCliente: this.codigoCliente});
  }
  goToFichaAlumno(){
    this.nav.push(FichaAlumnoPage, {fichaAlumnos: this.fichaAlumnos, cantidadAlumnos: this.cantidadAlumnos, idPack: this.idPack, clieId: this.clieId });
    //acá cerramos el slide
    //this.list.closeSlidingItems();
  }
  close(){
    this.nav.setRoot(HomePage);
  }

}
