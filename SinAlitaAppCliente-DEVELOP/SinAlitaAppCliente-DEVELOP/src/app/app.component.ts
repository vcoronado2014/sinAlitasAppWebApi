import { Component, ViewChild } from '@angular/core';
import { Nav, Platform } from 'ionic-angular';
import { StatusBar } from '@ionic-native/status-bar';
import { SplashScreen } from '@ionic-native/splash-screen';

import { HomePage } from '../pages/home/home';
import {AppSettings} from "../../AppSettings";
import { InicioPage } from '../pages/Inicio/inicio';

@Component({
  templateUrl: 'app.html'
})
export class MyApp {
  @ViewChild(Nav) nav: Nav;
  rootPage:any = InicioPage;
  pages: Array<{title: string, component: any}>;
  imgPrincipal;

  constructor(platform: Platform, statusBar: StatusBar, splashScreen: SplashScreen) {
    var paginaUno = {title: 'Inicio', component: InicioPage, visible: true };
    this.imgPrincipal = AppSettings.URL_FOTOS + "img/agua.jpg";


    this.pages = [
      paginaUno
    ]

    platform.ready().then(() => {
      // Okay, so the platform is ready and our plugins are available.
      // Here you can do any higher level native things you might need.
      statusBar.styleDefault();
      splashScreen.hide();
    });
  }
  openPage(page) {
    // Reset the content nav to have just this page
    // we wouldn't want the back button to show in this scenario
    this.nav.setRoot(page.component);
  }
}

