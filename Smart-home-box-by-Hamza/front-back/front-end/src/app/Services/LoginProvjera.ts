import {Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {Router} from "@angular/router";

@Injectable()
export class LoginProvjera {
  constructor(private http: HttpClient,private router:Router) {

  }
  async provjeraPrijave() {
    try{
    let res = await this.http.get("https://smarthomeapi.p2347.app.fit.ba/jePrijavljen").toPromise();
    if(!res) {
      localStorage.removeItem("my-token");
      this.router.navigate(["/login"]);
    }
    }
    catch(err) {
     console.log("Error: " + err);
    }
  }

}
