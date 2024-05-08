import {Component, OnDestroy, OnInit} from '@angular/core';
import {initializeApp} from "firebase/app";
import {gasfire, gasfirelista, Pokret, PokretiResponse} from "../Klase/Klase";
import {HttpClient} from "@angular/common/http";
import {child, get, getDatabase, ref, update} from "firebase/database";
import {AuthService} from "../Services/AuthService";
import {LoginProvjera} from "../Services/LoginProvjera";
import {environment} from "../../environments/environment.prod";

@Component({
  selector: 'app-security',
  templateUrl: './security.component.html',
  styleUrls: ['./security.component.css']
})
export class SecurityComponent implements OnInit, OnDestroy {

  aktivirajSigurnosni = false;
  app = initializeApp(environment.firebaseConfig);
  db:any;
  isDate = false;
  historyLista :Pokret[] = [];
  datum:string ="";
  dropDown = "1";
  movement = "";
  interval:any;
  provjera:any;
  constructor(private http: HttpClient, private auth:AuthService, private login: LoginProvjera) {
    this.db = getDatabase();
  }

  ngOnDestroy(): void {
        clearInterval(this.interval);
        clearInterval(this.provjera);
    }

  async ngOnInit() {
    this.setujDatum();
    this.interval = setInterval(()=> {this.ucitajSenzore();},1000);
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
      update(ref(this.db, `${this.auth.getId()}/`), {
        AktiviranSigurnosniSustav: 1
      }).then(() => {
        console.log("Podaci uspjesno ucitani!");
      }).catch((err) => {
        console.log("Greska: " + err);
      });
    }
    else {
      update(ref(this.db, `${this.auth.getId()}/`), {
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
    get(child(ref(this.db), `${this.auth.getId()}/`)).then(
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
          console.log("Podaci nisu pronadjeni!");
        }
      }
    ).catch((err: any) => {
      console.log("Greska: " + err);
    });
  }
  ugasiAlarm() {
    update(ref(this.db, `${this.auth.getId()}/`), {
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
