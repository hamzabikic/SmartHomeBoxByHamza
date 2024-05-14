import {Component, OnInit} from '@angular/core';
import {AuthService} from "./Services/AuthService";
import {InfoClass} from "./Services/InfoClass";
import {LoginProvjera} from "./Services/LoginProvjera";
import {initializeApp} from "firebase/app";
import {environment} from "../environments/environment.prod";
import {child, get, getDatabase, ref} from "firebase/database";

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

  protected readonly InfoClass = InfoClass;
  getSvjetlo() {
    if(AppComponent.lightMode) {
      return {backgroundColor : "rgb(26,54,93)" };
    }
    else {
      return {backgroundColor : "black" };
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
