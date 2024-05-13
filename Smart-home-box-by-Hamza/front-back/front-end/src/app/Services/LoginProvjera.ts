import {Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {Router} from "@angular/router";

@Injectable()
export class LoginProvjera {
  constructor(private http: HttpClient,private router:Router) {

  }
  static interval: NodeJS.Timeout | undefined = undefined;
  static servis :LoginProvjera |undefined = undefined;
   async provjeraPrijave() {
    try{
    let res = await this.http.get("https://smarthomeapi.p2347.app.fit.ba/jePrijavljen").toPromise();
    if(!res) {
      localStorage.removeItem("my-token");
      clearInterval(LoginProvjera.interval);
      this.router.navigate(["/login"]);
      return;
    }
    }
    catch(err) {
     console.log("Error: " + err);
    }
  }
}
