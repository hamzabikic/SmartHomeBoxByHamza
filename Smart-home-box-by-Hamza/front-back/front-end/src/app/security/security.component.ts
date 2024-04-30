import { Component, OnInit } from '@angular/core';
import {initializeApp} from "firebase/app";
import {environment} from "../../environments/environment";
import {gasfire, gasfirelista, Pokret, PokretiResponse} from "../Klase/Klase";
import {HttpClient} from "@angular/common/http";
import {child, get, getDatabase, ref, update} from "firebase/database";

@Component({
  selector: 'app-security',
  templateUrl: './security.component.html',
  styleUrls: ['./security.component.css']
})
export class SecurityComponent implements OnInit {

  aktivirajSigurnosni = true;
  app = initializeApp(environment.firebaseConfig);
  db:any;
  isDate = false;
  historyLista :Pokret[] = [];
  datum:string ="";
  dropDown = "1";
  movement = "";
  constructor(private http: HttpClient) {
    this.db = getDatabase();
  }

  async ngOnInit() {
    this.setujDatum();
    await this.ucitajSenzore();
    await this.ucitajHistory();
    setInterval(()=> {this.ucitajSenzore();},1000);
  }
  setujDatum() {
    this.datum=new Date().toISOString().split('T')[0];
  }
  async ucitajHistory() {
    if(!this.isDate) {
      if(this.dropDown=="1"){
        let datumobj :Date = new Date();
        let datum2: string = datumobj.getFullYear() + "-" + (datumobj.getMonth() + 1) + "-" + datumobj.getDate();
        let infoLista: PokretiResponse | undefined =
          await this.http.get<PokretiResponse>
          ("https://smarthomeapi.p2347.app.fit.ba/SkeniraniPokreti/getPokretiByDate?datum=" + datum2).toPromise();
        this.historyLista = infoLista!.pokreti; }
      else if(this.dropDown =="7") {
        let infoLista: PokretiResponse | undefined =
          await this.http.get<PokretiResponse>
          ("https://smarthomeapi.p2347.app.fit.ba/SkeniraniPokreti/getPokretiLast7Days" ).toPromise();
        this.historyLista = infoLista!.pokreti;
      }
      else {
        let infoLista: PokretiResponse | undefined =
          await this.http.get<PokretiResponse>
          ("https://smarthomeapi.p2347.app.fit.ba/SkeniraniPokreti/getPokretiLastMonth" ).toPromise();
        this.historyLista = infoLista!.pokreti;
      }
    }
    else {
      let datumobj :Date = new Date(this.datum);
      let datum2: string = datumobj.getFullYear() + "-" + (datumobj.getMonth() + 1) + "-" + datumobj.getDate();
      let infoLista: PokretiResponse | undefined =
        await this.http.get<PokretiResponse>
        ("https://smarthomeapi.p2347.app.fit.ba/SkeniraniPokreti/getPokretiByDate?datum=" + datum2).toPromise();
      this.historyLista = infoLista!.pokreti;
    }
  }
  aktivirajSigurnosniSustav() {
    if(this.aktivirajSigurnosni == false) {
      update(ref(this.db, "Sensors/"), {
        AktiviranSigurnosniSustav: 1
      }).then(() => {
        console.log("Podaci uspjesno ucitani!");
      }).catch((err) => {
        console.log("Greska: " + err);
      });
    }
    else {
      update(ref(this.db, "Sensors/"), {
        AktiviranSigurnosniSustav: 0,
        SenzorPokreta:0,
        AlarmPokret:0
      }).then(() => {
        console.log("Podaci uspjesno ucitani!");
      }).catch((err) => {
        console.log("Greska: " + err);
      });
      this.movement = "Deactivated";
    }
  }
  ucitajSenzore() {
    get(child(ref(this.db), "Sensors/")).then(
      (snapshot: any) => {
        if (snapshot.exists()) {
          if (snapshot.val().AktiviranSigurnosniSustav == 1) {
            this.aktivirajSigurnosni = true;
          } else {
            this.aktivirajSigurnosni = false;
          }
          if (snapshot.val().SenzorPokreta == 1) {
            this.movement = "Activated";
          } else {
            this.movement = "Deactivated";
          }
        } else {
          alert("Podaci nisu pronadjeni!");
        }
      }
    ).catch((err: any) => {
      alert("Greska: " + err);
    });
  }
  ugasiAlarm() {
    update(ref(this.db, "Sensors/"), {
      SenzorPokreta:0,
      AlarmPokret:0
    }).then(() => {
      console.log("Podaci uspjesno ucitani!");
    }).catch((err) => {
      console.log("Greska: " + err);
    });
    this.movement = "Deactivated";
  }
  movementcss() {
    if (this.movement =="Activated") {
       return {color:"red", fontWeight:"bolder" };
    }
    else {
      return {color:"green", fontWeight: "bolder"};
    }
  }
  async obrisi(id:number) {
    let response =
      await this.http.post
      ("https://smarthomeapi.p2347.app.fit.ba/SkeniraniPokreti/deletePokret?id="+id, {}).toPromise();
    if(response) {
      await this.ucitajHistory();
    }
    else {
      alert("Error");
    }
  }

}