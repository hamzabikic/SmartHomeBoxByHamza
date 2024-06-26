import {Component, OnDestroy, OnInit} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {KorisnikInfo} from "../Klase/Klase";
import {Router} from "@angular/router";
import {LoginProvjera} from "../Services/LoginProvjera";

import {AppComponent} from "../app.component";

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit, OnDestroy {

  korisnik :KorisnikInfo|undefined;
  staraLozinka ="";
  novaLozinka ="";
  odjava:boolean = false;
  moguce_slanje = true;
  moguce_slanje2 = true;
  moguce_slanje3 = true;
  podloga = false;
  constructor(private http:HttpClient, private router: Router) { }

  ngOnDestroy(): void {
    }


  async ngOnInit() {
    await this.ucitajPodatke();
  }
  async ucitajPodatke() {
    this.korisnik = await this.http.get<KorisnikInfo>("https://smarthomeapi.p2347.app.fit.ba/getInfo").toPromise();
  }
  async posaljiPodatke() {
    this.moguce_slanje2 = false;
    let res = await
      this.http.post("https://smarthomeapi.p2347.app.fit.ba/editujKorisnika",this.korisnik).toPromise();
    // @ts-ignore
    if(res.editovan) {
      alert("Success upload");
      await this.ucitajPodatke();
      this.moguce_slanje2 = true;
    }
    else {
      // @ts-ignore
      alert(res.greska);
      this.moguce_slanje2 = true;
    }
  }
  async odjavise(){
    this.podloga=true;
    clearInterval(LoginProvjera.interval);
    clearInterval(LoginProvjera.svjetloInterval);
    this.moguce_slanje3 = false;
    setTimeout(async () => {
      let res = await this.http.get("https://smarthomeapi.p2347.app.fit.ba/Odjava").toPromise();
      if (res) {
        localStorage.removeItem("my-token");
        this.router.navigate(["/login"]);
        this.moguce_slanje3 = true;
        this.podloga = false;
        return;
      }
      alert("Unsuccessful logout!");
      if(localStorage.getItem("my-token") == null) {
        this.moguce_slanje3 = true;
        this.podloga =false;
        return;
      }
      LoginProvjera.svjetloInterval = setInterval(()=> { LoginProvjera.servis!.getSvjetlost()},1000);
      LoginProvjera.interval = setInterval(async ()=> await LoginProvjera.servis!.provjeraPrijave(),1000);
      this.moguce_slanje3 = true;
      this.podloga = false;
    }, 2000);
  }

  async promijeniSifru() {
    if(this.staraLozinka.length==0 || this.novaLozinka.length ==0) {
      alert("Current password and new password required!");
      return;
    }
     let obj = {
       staraLozinka: this.staraLozinka,
       novaLozinka: this.novaLozinka,
       odjava: this.odjava
     }
     this.moguce_slanje = false;
     let res = await this.http.post("https://smarthomeapi.p2347.app.fit.ba/promjenaLozinke", obj).toPromise();
     // @ts-ignore
    if(res.editovan) {
      alert("Success upload!");
      this.staraLozinka="";
      this.novaLozinka ="";
      this.odjava = false;
      this.moguce_slanje = true;
      return;
     }
    // @ts-ignore
    alert(res.greska);
    this.moguce_slanje= true;
  }

  protected readonly clearInterval = clearInterval;
}
