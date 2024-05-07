import {Component, OnDestroy, OnInit} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {KorisnikInfo} from "../Klase/Klase";
import {Router} from "@angular/router";
import {LoginProvjera} from "../Services/LoginProvjera";

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
  provjera:any;
  constructor(private http:HttpClient, private router: Router, private login : LoginProvjera) { }

  ngOnDestroy(): void {
        clearInterval(this.provjera);
    }

  async ngOnInit() {
    await this.ucitajPodatke();
    this.provjera = setInterval(async ()=> await this.login.provjeraPrijave(),1000);
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
  async odjavise() {
    this.moguce_slanje3 =false;
    let res = await this.http.get("https://smarthomeapi.p2347.app.fit.ba/Odjava").toPromise();
    if(res) {
      localStorage.removeItem("my-token");
      this.router.navigate(["/login"]);
      this.moguce_slanje3 = true;
      return;
    }
    alert("Unsuccessful logout!");
    this.moguce_slanje3 = true;
  }
  async promijeniSifru() {
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
}
