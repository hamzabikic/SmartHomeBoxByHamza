import {Component, OnInit} from '@angular/core';
import {AuthService} from "./Services/AuthService";
import {InfoClass} from "./Services/InfoClass";
import {LoginProvjera} from "./Services/LoginProvjera";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  constructor(protected auth:AuthService, protected login: LoginProvjera) {
    LoginProvjera.servis = this.login;
  }

  async ngOnInit(){
    await this.login.provjeraPrijave();
      if(this.auth.jeLogiran()) {
        LoginProvjera.interval = setInterval(async ()=> await this.login.provjeraPrijave(),1000);
      }
    }

  protected readonly InfoClass = InfoClass;
}
