import {Component, OnInit} from '@angular/core';
import {AuthService} from "./Services/AuthService";

import {LoginProvjera} from "./Services/LoginProvjera";


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  static lightMode:boolean = true;
  constructor(protected auth:AuthService, protected login: LoginProvjera) {
    LoginProvjera.servis = this.login;
  }

  async ngOnInit() {
    await this.login.provjeraPrijave();
    if (this.auth.jeLogiran()) {
      LoginProvjera.interval = setInterval(async () => await this.login.provjeraPrijave(), 1000);
      LoginProvjera.svjetloInterval = setInterval(()=> { this.login.getSvjetlost()},1000);
    }
  }

  getSvjetlo() {
    let visina = 750;
    if(window.innerHeight>750) {
      visina = window.innerHeight;
    }
    if(AppComponent.lightMode) {
      return {backgroundColor : "rgb(26,54,93)" ,
        height: visina +"px"
      };
    }
    else {
      return {backgroundColor : "black",
        height: visina +"px"};
    }
  }
  getBorder() {
    if (AppComponent.lightMode) {
      return {border: "10px solid rgb(26,54,93)"};
    } else {
      return {border: "10px solid black"};
    }
  }
}
