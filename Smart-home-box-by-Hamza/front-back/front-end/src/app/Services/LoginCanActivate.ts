import {ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree} from "@angular/router";
import {Observable} from "rxjs";
import {Injectable} from "@angular/core";
import {AuthService} from "./AuthService";
import {HttpClient} from "@angular/common/http";

@Injectable()
export class LoginCanActivate implements CanActivate {

  constructor(private auth:AuthService, private http:HttpClient, private router: Router) {
  }
  async canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) :Promise<boolean> {
        if(!this.auth.jeLogiran()) {
          return true;
        }
        let res = await this.http.get("https://smarthomeapi.p2347.app.fit.ba/jePrijavljen").toPromise();
        if(res) {
          this.router.navigate(["profile"]);
          return false;
        }
        localStorage.removeItem("my-token");
        return true;
    }
}
