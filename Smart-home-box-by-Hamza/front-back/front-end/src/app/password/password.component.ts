import { Component, OnInit } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Router} from "@angular/router";
import {AppComponent} from "../app.component";

@Component({
  selector: 'app-password',
  templateUrl: './password.component.html',
  styleUrls: ['./password.component.css']
})
export class PasswordComponent implements OnInit {
  email ="";
  moguce_slanje = true;
  moguc_unos = true;
  constructor(private http: HttpClient, private router: Router) { }

  ngOnInit(): void {
  }

  async promijeniLozinku() {
    this.moguce_slanje= false;
    this.moguc_unos=false;
    let obj = {
      email: this.email
    };
   let res = await this.http.post("https://smarthomeapi.p2347.app.fit.ba/generisiNovuLozinku", obj)
     .toPromise();
   // @ts-ignore
    if(res.editovan) {
      alert("Your temporary password has been sent to your email!");
      this.moguce_slanje=true;
      this.moguc_unos=true;
      this.router.navigate(["/login"]);
      return;
   }
    // @ts-ignore
    alert(res.greska);
    this.email="";
    this.moguce_slanje= true;
    this.moguc_unos=true;
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
