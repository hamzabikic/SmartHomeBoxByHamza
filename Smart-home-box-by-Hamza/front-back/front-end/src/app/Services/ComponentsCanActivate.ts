import {Injectable} from "@angular/core";
import {ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree} from "@angular/router";
import {Observable} from "rxjs";
import {AuthService} from "./AuthService";
import {HttpClient} from "@angular/common/http";

@Injectable()

export class ComponentsCanActivate implements CanActivate {
    constructor(private router: Router,private auth: AuthService) {

    }
    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        if(!this.auth.jeLogiran()) {
          this.router.navigate(["/login"]);
          return false;
        }
      return true;
    }

}
