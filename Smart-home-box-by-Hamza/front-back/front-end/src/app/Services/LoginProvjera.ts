import {Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {Router} from "@angular/router";
import {initializeApp} from "firebase/app";
import {environment} from "../../environments/environment.prod";
import {child, get, getDatabase, ref} from "firebase/database";
import {AppComponent} from "../app.component";
import {AuthService} from "./AuthService";

@Injectable()
export class LoginProvjera {
  constructor(private http: HttpClient,private router:Router, private auth: AuthService) {
    this.db = getDatabase();
  }
  static interval: NodeJS.Timeout | undefined = undefined;
  static svjetloInterval: NodeJS.Timeout |undefined;
  static servis :LoginProvjera |undefined = undefined;
  app = initializeApp(environment.firebaseConfig);
  db:any;
   async provjeraPrijave() {
    try{
    let res = await this.http.get("https://smarthomeapi.p2347.app.fit.ba/jePrijavljen").toPromise();
    if(!res) {
      localStorage.removeItem("my-token");
      clearInterval(LoginProvjera.interval);
      clearInterval(LoginProvjera.svjetloInterval);
      this.router.navigate(["/login"]);
      return;
    }
    }
    catch(err) {
     console.log("Error: " + err);
    }
  }
  getSvjetlost () {
    get(child(ref(this.db), `${this.auth.getId()}/`)).then(
      (snapshot: any) => {
        if (snapshot.exists()) {
          if (snapshot.val().Svjetlost >=1023) {
            AppComponent.lightMode = false;
          }
          else {
            AppComponent.lightMode = true;
          }
        }}
    ).catch((err: any) => {
      console.log("Greska: " + err);
    });
  }
}

