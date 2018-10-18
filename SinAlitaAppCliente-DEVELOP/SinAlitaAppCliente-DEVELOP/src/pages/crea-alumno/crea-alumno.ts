import { Component } from '@angular/core';
import { NavController, NavParams, AlertController, ToastController, ViewController } from 'ionic-angular';
import { LoadingController } from 'ionic-angular';
import { DetailPackPage } from '../../pages/detail-pack/detail-pack';

import { AlumnoService } from '../../app/Services/AlumnoService';
import { FichaAlumnoService } from '../../app/Services/FichaAlumnoService';
import { AuthService } from '../../app/Services/AuthService';
import {AppSettings} from "../../../AppSettings";

/**
 * Generated class for the CreaAlumnoPage page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */

@Component({
  selector: 'page-crea-alumno',
  templateUrl: 'crea-alumno.html',
})
export class CreaAlumnoPage {
  //aca necesitamos
  //el id del cliente para buscar una lista de alumnos que tenga,
  //mostrarlos en un combo y que lo pueda usar
  alumnosArr =[];
  fichasArr =[];
  public tieneAlumnos;
  public clieId;
  public idPack;
  //variables del formulario
  public idAlumno;
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

  public cantidadAlumnosActual;
  public codigoCliente;

  pet: string = "puppies";

  public idAlumnoEditar;
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
    public alumnos: AlumnoService,
    public auth: AuthService,
    public ficha: FichaAlumnoService
  ) {
      this.cantidadAlumnosActual = 0;
      this.clieId = navParams.get('clieId');
      this.idPack = navParams.get('idPack');
      this.codigoCliente= navParams.get('codigoCliente');
      this.tieneAlumnos = false;
      this.idAlumnoEditar = 0;
          this.urlFacebook = AppSettings.URL_FACEBOOK;
    this.urlInstagram = AppSettings.URL_INSTAGRAM;
    this.urlFun = AppSettings.URL_FUN;

      let loader = this.loading.create({
        content: 'Cargando...',
      });

      loader.present().then(() => {
        this.alumnos.getAlumnos(this.clieId).subscribe(
          data => {
            this.alumnosArr = data.json();
            if (this.alumnosArr.length > 0){
              this.tieneAlumnos = true;
            }


          },
          err => console.error(err),
          () => console.log('get alumnos completed')
        );

        this.ficha.getFichas(this.idPack).subscribe(
          dataAl => {
            this.fichasArr = dataAl.json();
            if (this.fichasArr){
              this.cantidadAlumnosActual = this.fichasArr.length;
            }



          },
          err => console.error(err),
          () => console.log('get fichas completed')
        );



        loader.dismiss();
      });

  }
  guardar(){
    //se envian los elementos a guardar, luego se asigna al arreglo de fichas
    //el retorno del guardado, se asigna a la variable contador la cantidad de alumnos
    //se limpia el formulario
      if (this.validar()){
        //todo ok.
        //let mi = this.presentToast('correcto', 'bottom', 4000);
        //el IdAlumnoEditar es el elemento de nuevo o antiguo
        var intTieneProblemasCardiacos = "0";
        var intTieneProblemasMotores= "0";
        var intTieneAsma = "0";

        if (this.tieneAsma)
          intTieneAsma = "1";
        if (this.tieneProblemasCardiacos)
          intTieneProblemasCardiacos = "1";
        if (this.tieneProblemasMotores)
          intTieneProblemasMotores = "1";

        var entidad = {
          IdAlumno: this.idAlumnoEditar.toString(),
          IdPack: this.idPack.toString(),
          NombreCompleto: this.nombreCompleto,
          Edad: this.edad.toString(),
          Sexo: this.sexo,
          TieneProblemasMotores: intTieneProblemasMotores,
          CualesProblemasMotores: this.cualesProblemasMotores,
          TieneProblemasCardiacos: intTieneProblemasCardiacos,
          CualesProblemasCardiacos: this.cualesProblemasCardiacos,
          TieneAsma: intTieneAsma,
          OtraEnfermedad: this.otraEnfermedad,
          NumeroEmergencia: this.numeroEmergencia,
          DondeAcudir: this.dondeAcudir,
          Observacion: this.observacion
        };

        //la entidad esta lista
        //ahora a enviar los elementos
        this.ficha.put(entidad).subscribe(
          data => {
            if (data.status == 200) {
              //todo ok
              let mi = this.presentToast('Registro Guardado con éxito.', 'top', 5000);
              this.fichasArr = data.json();
              if (this.fichasArr){
                this.cantidadAlumnosActual = this.fichasArr.length;
              }
              this.alumnos.getAlumnos(this.clieId).subscribe(
                data => {
                  this.alumnosArr = data.json();
                  if (this.alumnosArr.length > 0){
                    this.tieneAlumnos = true;
                  }


                },
                err => console.error(err),
                () => console.log('get alumnos completed')
              );
              this.limpiar();

            }
            else {
              let mi = this.presentToast('Error al guardar.', 'bottom', 4000);
              //redireccionar a la anterior
              //this.nav.setRoot(HomePage);
            }
          },
          err => console.error(err),
          () => console.log('ok')
        );



      }
  }

  close(){
    this.auth.Post(this.codigoCliente).subscribe(
      data => {
        this.nav.push(DetailPackPage, {id: this.codigoCliente, envoltorio: this.auth.envoltorio });
      },
      err => console.error(err),
      () => console.log('ok')
    );
    //aca hay que ir a la pagina anterior con push
    //this.viewCtrl.dismiss();
/*    this.auth.Post(this.codigoCliente)
      .subscribe(
        rs => = rs,
        er => {
          //console.log(error)
          //let mi = this.presentToast('No existe información', 'bottom', 4000);

        },
        () => {
            this.nav.push(DetailPackPage, {id: this.codigoCliente, envoltorio: this.auth.envoltorio })
              .then(data => console.log(data),
                error => {
                  //console.log(error)
                  let mi = this.presentToast(error, 'bottom', 4000);
                }
              );
          }

        }
      )*/


  }
  validar(){
    if (this.nombreCompleto == undefined){
      let mi = this.presentToast('Nombre es requerido', 'bottom', 4000);
      return false;
    }
    if (this.edad == undefined){
      let mi = this.presentToast('Edad es requerida', 'bottom', 4000);
      return false;
    }
    if (this.sexo == undefined){
      let mi = this.presentToast('Sexo es requerido', 'bottom', 4000);
      return false;
    }
    if (this.dondeAcudir == undefined){
      let mi = this.presentToast('Donde acudir es requerido', 'bottom', 4000);
      return false;
    }
    if (this.numeroEmergencia == undefined){
      let mi = this.presentToast('Número teléfono es requerido', 'bottom', 4000);
      return false;
    }
    return true;
  }
  usar(item){
    if (item != undefined){
      //buscar
      //this.alumnosArr.find(this.encontrarAlumno(alumno, item));
      var alumno = this.encontrarAlumno(item);
      if (alumno){
        this.idAlumno = alumno.Id;

        this.nombreCompleto = alumno.NombreCompleto;
        this.edad = alumno.Edad;
        this.sexo = alumno.Sexo;
        if (alumno.TieneAsma == 1)
          this.tieneAsma = true;
        else
          this.tieneAsma = false;
        if (alumno.TieneProblemasCardiacos == 1)
          this.tieneProblemasCardiacos = true;
        else
          this.tieneProblemasCardiacos = false;
        if (alumno.TieneProblemasMotores == 1)
          this.tieneProblemasMotores = true;
        else
          this.tieneProblemasMotores = false;

        this.cualesProblemasCardiacos = alumno.CualesProblemasCardiacos;
        this.cualesProblemasMotores = alumno.CualesProblemasMotores;


        this.dondeAcudir = alumno.DondeAcudir;
        this.numeroEmergencia = alumno.NumeroEmergencia;
        this.observacion = alumno.Observacion;
        this.otraEnfermedad = alumno.OtraEnfermedad;

      }
    }
    else {
      //mensaje
      let mi = this.presentToast('Debe seleccionar un alumno', 'bottom', 4000);
    }

  }
  limpiar(){
    this.idAlumno = 0;
    this.idAlumnoEditar = 0;

    this.nombreCompleto = "";
    this.edad="";
    this.sexo="";
    this.tieneAsma = false;
    this.tieneProblemasCardiacos = false;
    this.tieneProblemasMotores = false;

    this.cualesProblemasCardiacos = "";
    this.cualesProblemasMotores = "";


    this.dondeAcudir = "";
    this.numeroEmergencia = "";
    this.observacion = "";
    this.otraEnfermedad = "";

  }
  encontrarAlumno(id) {

    var alumno;
    if (this.alumnosArr){
      for (var i in this.alumnosArr){
        if (this.alumnosArr[i].Id == id){
          alumno = this.alumnosArr[i];
        }
      }
    }

    return alumno;
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
