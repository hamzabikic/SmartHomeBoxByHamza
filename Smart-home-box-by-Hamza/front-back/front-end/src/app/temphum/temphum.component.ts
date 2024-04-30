import { Component, OnInit } from '@angular/core';
import {getDatabase, ref, get, set, child, update, remove} from 'firebase/database';
import {initializeApp} from "firebase/app";
import {environment} from "../../environments/environment";
import {HttpClient} from "@angular/common/http";
import {TemperaturaVlaznost, temphumlista} from "../Klase/Klase";

@Component({
  selector: 'app-temphum',
  templateUrl: './temphum.component.html',
  styleUrls: ['./temphum.component.css']
})
export class TemphumComponent implements OnInit {
  app = initializeApp(environment.firebaseConfig);
  temperatura ="";
  humidity ="";
  db:any;
  isDate = false;
  historyLista :TemperaturaVlaznost[] = [];
  datum:string ="";
  dropDown = "1";
  constructor(private http: HttpClient) {
    this.db = getDatabase();
  }

  async ngOnInit() {
    this.setujDatum();
    await this.ucitajPodatke();
    await this.ucitajHistory();
    setInterval(()=> {this.ucitajPodatke()},1000);
  }
  setujDatum() {
    this.datum=new Date().toISOString().split('T')[0];
  }
  async ucitajHistory() {
    if(!this.isDate) {
      if(this.dropDown=="1"){
      let datumobj :Date = new Date();
      let datum2: string = datumobj.getFullYear() + "-" + (datumobj.getMonth() + 1) + "-" + datumobj.getDate();
      let infoLista: temphumlista | undefined =
        await this.http.get<temphumlista>
        ("https://smarthomeapi.p2347.app.fit.ba/TemperaturaVlaznost/getInfoByDate?datum=" + datum2).toPromise();
      this.historyLista = infoLista!.infoLista; }
      else if(this.dropDown =="7") {
        let infoLista: temphumlista | undefined =
          await this.http.get<temphumlista>
          ("https://smarthomeapi.p2347.app.fit.ba/TemperaturaVlaznost/getInfoLast7Days" ).toPromise();
        this.historyLista = infoLista!.infoLista;
      }
      else {
        let infoLista: temphumlista | undefined =
          await this.http.get<temphumlista>
          ("https://smarthomeapi.p2347.app.fit.ba/TemperaturaVlaznost/getInfoLastMonth" ).toPromise();
        this.historyLista = infoLista!.infoLista;
      }
    }
    else {
        let datumobj :Date = new Date(this.datum);
        let datum2: string = datumobj.getFullYear() + "-" + (datumobj.getMonth() + 1) + "-" + datumobj.getDate();
        let infoLista: temphumlista | undefined =
          await this.http.get<temphumlista>
          ("https://smarthomeapi.p2347.app.fit.ba/TemperaturaVlaznost/getInfoByDate?datum=" + datum2).toPromise();
        this.historyLista = infoLista!.infoLista;
    }
  }
  ucitajPodatke () {
    get(child(ref(this.db), "Sensors/")).then(
      (snapshot: any) => {
        if (snapshot.exists()) {
          this.temperatura = String(snapshot.val().Temperature);
          this.humidity = String(snapshot.val().Humidity);
        } else {
          alert("Podaci nisu pronadjeni!");
        }
      }
    ).catch((err: any) => {
      alert("Greska: " + err);
    });
  }
  tempcss() {
    if (parseInt(this.temperatura) < 5) {
      return {color: "blue", fontWeight: "bolder"};
    } else if (parseInt(this.temperatura) >= 5 && parseInt(this.temperatura) < 15) {
      return {color: "dodgerblue", fontWeight: "bolder"};
    } else if (parseInt(this.temperatura) >= 15 && parseInt(this.temperatura) < 25) {
      return {color: "green", fontWeight: "bolder"};
    } else if (parseInt(this.temperatura) >= 25 && parseInt(this.temperatura) < 35) {
      return {color: "orange", fontWeight: "bolder"};
    } else {
      return {color: "red", fontWeight: "bolder"};
    }
  }
}
