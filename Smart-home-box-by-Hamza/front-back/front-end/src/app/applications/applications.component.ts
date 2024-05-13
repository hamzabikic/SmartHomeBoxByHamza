import {Component, OnDestroy, OnInit} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {AuthService} from "../Services/AuthService";
import {LoginProvjera} from "../Services/LoginProvjera";
import {TimeoutInfo} from "rxjs";

@Component({
  selector: 'app-applications',
  templateUrl: './applications.component.html',
  styleUrls: ['./applications.component.css']
})
export class ApplicationsComponent implements OnInit{
  prijave:any;
  moguce_slanje = true;
  constructor(private http:HttpClient, protected auth:AuthService) { }

  async ngOnInit(){
    await this.ucitajPrijave();
  }
  async ucitajPrijave() {
     this.prijave = await this.http.get("https://smarthomeapi.p2347.app.fit.ba/getPrijave").toPromise();
  }
  async obrisiPrijavu(id:number) {
    this.moguce_slanje = false;
     let res = await this.http.get("https://smarthomeapi.p2347.app.fit.ba/odjaviUredjaj?id="+id).toPromise();
     if(res) {
       await this.ucitajPrijave();
       alert("Success!");
       this.moguce_slanje = true;
       return;
     }
     alert("Unsuccess load!");
     this.moguce_slanje = true;
  }

  protected readonly parseInt = parseInt;
}
