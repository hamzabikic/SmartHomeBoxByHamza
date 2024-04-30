import { Component, OnInit } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {KorisnikInfo} from "../Klase/Klase";

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {

  korisnik :KorisnikInfo|undefined;
  constructor(private http:HttpClient) { }

  async ngOnInit() {
    await this.ucitajPodatke();
  }
  async ucitajPodatke() {
    this.korisnik = await this.http.get<KorisnikInfo>("https://smarthomeapi.p2347.app.fit.ba/getInfo").toPromise();
  }
  async posaljiPodatke() {
    let poslano = await
      this.http.post<boolean>("https://smarthomeapi.p2347.app.fit.ba/editujKorisnika",this.korisnik).toPromise();
    if(poslano) {
      alert("Success upload");
      await this.ucitajPodatke();
    }
    else {
      alert("Error");
    }
  }
}
