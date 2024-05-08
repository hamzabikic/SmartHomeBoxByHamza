import {ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree} from "@angular/router";
import {Observable} from "rxjs";
import {Injectable} from "@angular/core";
import {AuthService} from "./AuthService";
import {HttpClient} from "@angular/common/http";

@Injectable()
export class LoginCanActivate implements CanActivate {

  constructor(private auth:AuthService, private router: Router) {
  }
  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot){
        if (!this.auth.jeLogiran()) {
          return true;
        }
        this.router.navigate([""]);
        return false;
    }
}
