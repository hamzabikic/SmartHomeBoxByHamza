import {Component, OnDestroy, OnInit} from '@angular/core';
import {getDatabase, ref, get, set, child, update, remove} from 'firebase/database';
import {initializeApp} from "firebase/app";
import {HttpClient} from "@angular/common/http";
import {AuthService} from "../Services/AuthService";
import {LoginProvjera} from "../Services/LoginProvjera";
import {environment} from "../../environments/environment.prod";
@Component({
  selector: 'app-light',
  templateUrl: './light.component.html',
  styleUrls: ['./light.component.css']
})
export class LightComponent implements OnInit, OnDestroy {
  app = initializeApp(environment.firebaseConfig);
  prvoVrijeme :number =0;
  drugoVrijeme :number =0;
  automatskoSvjetlo = false;
  upaljenoSvjetlo = false;
  db:any;
  interval:any;
  provjera:any;
  constructor(private http :HttpClient ,private auth:AuthService, private login: LoginProvjera) {
    this.db = getDatabase();
  }

  ngOnDestroy(): void {
        clearInterval(this.interval);
        clearInterval(this.provjera);
    }

  async ngOnInit() {
    await this.ucitajPodatke();
    this.interval = setInterval(()=> {this.ucitajPodatke()}, 1000);
    this.provjera = setInterval(async ()=> await this.login.provjeraPrijave() ,1000);
    await this.ucitajTajming();
  }
  async ucitajTajming() {
    let obj = await this.http.get("https://smarthomeapi.p2347.app.fit.ba/Light/getTime").toPromise();
    // @ts-ignore
    this.prvoVrijeme = obj.pocetak;
    // @ts-ignore
    this.drugoVrijeme = obj.kraj;
  }
  async setTime () {
   let obj = await this.http.post("https://smarthomeapi.p2347.app.fit.ba/Light/setTime?pocetak="+this.prvoVrijeme+"&kraj="+
      this.drugoVrijeme, {}).toPromise();
   if(obj == true) {
     alert("Success upload!");
   }
   else {
     alert("Error");
   }
   await this.ucitajPodatke();
  }
  ucitajPodatke () {
    get(child(ref(this.db), `${this.auth.getId()}/`)).then(
      (snapshot: any) => {
        if (snapshot.exists()) {
          this.automatskoSvjetlo = snapshot.val().AutomatskoSvjetlo;
          this.upaljenoSvjetlo = snapshot.val().UpaljenoSvjetlo;
        } else {
          alert("Podaci nisu pronadjeni!");
        }
      }
    ).catch((err: any) => {
      alert("Greska: " + err);
    });
  }
  automatskoPaljenje() {
    if(this.automatskoSvjetlo) {
      update(ref(this.db, `${this.auth.getId()}/`), {
        AutomatskoSvjetlo:1,
        UpaljenoSvjetlo:0
      }).then(() => {
        console.log("Podaci uspjesno ucitani!");
      }).catch((err) => {
        console.log("Greska: " + err);
      });
      this.upaljenoSvjetlo =false;
    }
    else {
      update(ref(this.db, `${this.auth.getId()}/`), {
        AutomatskoSvjetlo:0
      }).then(() => {
        console.log("Podaci uspjesno ucitani!");
      }).catch((err) => {
        console.log("Greska: " + err);
      });
    }
  }
  upaliSvjetlo() {
    if(this.upaljenoSvjetlo) {
      update(ref(this.db, `${this.auth.getId()}/`), {
        UpaljenoSvjetlo:1
      }).then(() => {
        console.log("Podaci uspjesno ucitani!");
      }).catch((err) => {
        console.log("Greska: " + err);
      });
    }
    else {
        update(ref(this.db, `${this.auth.getId()}/`), {
          UpaljenoSvjetlo:0
        }).then(() => {
          console.log("Podaci uspjesno ucitani!");
        }).catch((err) => {
          console.log("Greska: " + err);
        });
    }
  }
}
