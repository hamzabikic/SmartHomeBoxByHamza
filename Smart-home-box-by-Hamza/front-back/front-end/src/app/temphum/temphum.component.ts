import {Component, OnDestroy, OnInit} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {TemperaturaVlaznost, temphumlista} from "../Klase/Klase";
import {AuthService} from "../Services/AuthService";


@Component({
  selector: 'app-temphum',
  templateUrl: './temphum.component.html',
  styleUrls: ['./temphum.component.css']
})
export class TemphumComponent implements OnInit, OnDestroy {
  isDate = false;
  historyLista :TemperaturaVlaznost[] = [];
  datum:string ="";
  dropDown = "1";
  constructor(private http: HttpClient, private auth: AuthService) {

  }

  ngOnDestroy(): void {

    }

  async ngOnInit() {
    this.setujDatum();
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

}
