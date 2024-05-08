import {Component, OnDestroy, OnInit} from '@angular/core';
import {getDatabase, ref, get, set, child, update, remove} from 'firebase/database';
import {initializeApp} from "firebase/app";
import {HttpClient} from "@angular/common/http";
import {TemperaturaVlaznost, temphumlista} from "../Klase/Klase";
import {AuthService} from "../Services/AuthService";
import {LoginComponent} from "../login/login.component";
import {LoginProvjera} from "../Services/LoginProvjera";
import {environment} from "../../environments/environment.prod";

@Component({
  selector: 'app-temphum',
  templateUrl: './temphum.component.html',
  styleUrls: ['./temphum.component.css']
})
export class TemphumComponent implements OnInit, OnDestroy {
  app = initializeApp(environment.firebaseConfig);
  temperatura ="";
  humidity ="";
  db:any;
  isDate = false;
  historyLista :TemperaturaVlaznost[] = [];
  datum:string ="";
  dropDown = "1";
  interval: any;
  provjera:any;
  constructor(private http: HttpClient, private auth: AuthService, private login: LoginProvjera) {
    this.db = getDatabase();

  }

  ngOnDestroy(): void {
        clearInterval(this.interval);
        clearInterval(this.provjera);
    }

  async ngOnInit() {
    this.setujDatum();
    this.interval = setInterval(()=> {this.ucitajPodatke()},1000);
    this.provjera = setInterval(async ()=> await this.login.provjeraPrijave(),1000 );
    await this.ucitajHistory();
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
    get(child(ref(this.db), `${this.auth.getId()}/`)).then(
      (snapshot: any) => {
        if (snapshot.exists()) {
          this.temperatura = String(snapshot.val().Temperature);
          this.humidity = String(snapshot.val().Humidity);
        } else {
          console.log("Podaci nisu pronadjeni!");
        }
      }
    ).catch((err: any) => {
      console.log("Greska: " + err);
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
