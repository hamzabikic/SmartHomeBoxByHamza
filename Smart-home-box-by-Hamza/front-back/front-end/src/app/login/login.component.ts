import { Component, OnInit } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Router} from "@angular/router";
import {LoginProvjera} from "../Services/LoginProvjera";
import {AppComponent} from "../app.component";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  moguce_slanje = true;
  username ="";
  password ="";
  moguc_unos1=true;
  moguc_unos2 =true;
  constructor(private http:HttpClient, private router: Router) { }

  ngOnInit(): void {
  }

  async prijava() {
    if(this.username.length==0 || this.password.length ==0) {
      alert("Username and password required!");
      return;
    }
    let obj= {
      username:this.username,
      password:this.password
    };
    this.moguce_slanje= false;
    this.moguc_unos1= false;
    this.moguc_unos2=false;
    let res = await this.http.post("https://smarthomeapi.p2347.app.fit.ba/Prijava", obj).toPromise();
    // @ts-ignore
    if(res.jeLogiran) {
      // @ts-ignore
      localStorage.setItem("my-token", JSON.stringify(res.prijava));
      this.moguce_slanje= true;
      this.moguc_unos1= true;
      this.moguc_unos2=true;
      LoginProvjera.interval = setInterval(async ()=> await LoginProvjera.servis!.provjeraPrijave(),1000);
      LoginProvjera.svjetloInterval = setInterval(()=> { LoginProvjera.servis!.getSvjetlost()},1000);
      this.router.navigate([""]);
      return;
    }
    alert("Incorrect data!");
    this.password ="";
    this.moguce_slanje= true;
    this.moguc_unos1=true;
    this.moguc_unos2=true;
  }
  getSvjetlo() {
    if(AppComponent.lightMode) {
      return {backgroundColor:"rgb(26,54,93)"};
    }
    else {
      return {backgroundColor:"black"};
    }
  }

}
