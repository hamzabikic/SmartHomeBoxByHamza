import { Component, OnInit } from '@angular/core';
import {initializeApp} from "firebase/app";
import {environment} from "../../environments/environment";
import {gasfire, gasfirelista, TemperaturaVlaznost, temphumlista} from "../Klase/Klase";
import {HttpClient} from "@angular/common/http";
import {child, get, getDatabase, ref, update} from "firebase/database";

@Component({
  selector: 'app-gasfire',
  templateUrl: './gasfire.component.html',
  styleUrls: ['./gasfire.component.css']
})
export class GasfireComponent implements OnInit {
  gasvalue ="";
  alarm = "";
  fire="";
  app = initializeApp(environment.firebaseConfig);
  db:any;
  isDate = false;
  historyLista :gasfire[] = [];
  datum:string ="";
  dropDown = "1";
  constructor(private http: HttpClient) {
    this.db = getDatabase();
  }

  async ngOnInit() {
    this.setujDatum();
    await this.ucitajPodatke();
    await this.ucitajHistory();
    setInterval(()=> {this.ucitajPodatke();},1000);
  }
  setujDatum() {
    this.datum=new Date().toISOString().split('T')[0];
  }
  async ucitajHistory() {
    if(!this.isDate) {
      if(this.dropDown=="1"){
        let datumobj :Date = new Date();
        let datum2: string = datumobj.getFullYear() + "-" + (datumobj.getMonth() + 1) + "-" + datumobj.getDate();
        let infoLista: gasfirelista | undefined =
          await this.http.get<gasfirelista>
          ("https://smarthomeapi.p2347.app.fit.ba/GasFire/getInfoByDate?datum=" + datum2).toPromise();
        this.historyLista = infoLista!.lista; }
      else if(this.dropDown =="7") {
        let infoLista: gasfirelista | undefined =
          await this.http.get<gasfirelista>
          ("https://smarthomeapi.p2347.app.fit.ba/GasFire/getInfoLast7Days" ).toPromise();
        this.historyLista = infoLista!.lista;
      }
      else {
        let infoLista: gasfirelista | undefined =
          await this.http.get<gasfirelista>
          ("https://smarthomeapi.p2347.app.fit.ba/GasFire/getInfoLastMonth" ).toPromise();
        this.historyLista = infoLista!.lista;
      }
    }
    else {
      let datumobj :Date = new Date(this.datum);
      let datum2: string = datumobj.getFullYear() + "-" + (datumobj.getMonth() + 1) + "-" + datumobj.getDate();
      let infoLista: gasfirelista | undefined =
        await this.http.get<gasfirelista>
        ("https://smarthomeapi.p2347.app.fit.ba/GasFire/getInfoByDate?datum=" + datum2).toPromise();
      this.historyLista = infoLista!.lista;
    }
  }
  ucitajPodatke () {
    get(child(ref(this.db), "Sensors/")).then(
      (snapshot: any) => {
        if (snapshot.exists()) {
          this.gasvalue= String(snapshot.val().CO2);
          if(snapshot.val().AlarmCO2 ==1) {
            this.alarm ="Activated";
          }
          else {
            this.alarm ="Deactivated";
          }
          if(snapshot.val().FireSensor==1) {
            this.fire ="Yes";
          }
          else {
            this.fire = "No";
          }
        } else {
          alert("Podaci nisu pronadjeni!");
        }
      }
    ).catch((err: any) => {
      alert("Greska: " + err);
    });
  }
  gascss() {
    if (parseInt(this.gasvalue) < 500) {
      return {color: "green", fontWeight: "bolder"};
    } else {
      return {color: "red", fontWeight: "bolder"};
    }
  }
  gasalarmcss() {
    if (this.alarm == "Activated") {
      return {color: "red", fontWeight: "bolder"};
    } else {
      return {color: "green", fontWeight: "bolder"};
    }
  }
  firecss() {
    if (this.fire == "Yes") {
      return {color: "red", fontWeight: "bolder"};
    } else {
      return {color: "green", fontWeight: "bolder"};
    }
  }
  ugasiAlarm() {
    update(ref(this.db, "Sensors/"), {
      AlarmCO2: 0,
      FireSensor: 0
    }).then(() => {
      console.log("Podaci uspjesno ucitani!");
    }).catch((err) => {
      console.log("Greska: " + err);
    });
    this.alarm = "Deactivated";
    this.fire = "No";
  }
  async obrisi (id:number)  {
    let response =
      await this.http.post
      ("https://smarthomeapi.p2347.app.fit.ba/GasFire/deleteInfo?id="+id, {}).toPromise();
    if(response) {
      await this.ucitajHistory();
    }
    else {
      alert("Error");
    }
  }


}