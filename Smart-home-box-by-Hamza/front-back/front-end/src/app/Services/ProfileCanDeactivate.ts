import {ActivatedRouteSnapshot, CanDeactivate, RouterStateSnapshot, UrlTree} from "@angular/router";
import {Observable} from "rxjs";
import {ProfileComponent} from "../profile/profile.component";
import {Injectable} from "@angular/core";
 @Injectable()
export class ProfileCanDeactivate implements CanDeactivate<ProfileComponent> {
    canDeactivate(component: ProfileComponent, currentRoute: ActivatedRouteSnapshot, currentState: RouterStateSnapshot, nextState?: RouterStateSnapshot | undefined): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
      if(!component.izbrisan){
        clearInterval(component.provjera); }
      if(component.login.utoku) {
        component.provjera = setInterval(async ()=> {component.login.utoku= true; await component.login.provjeraPrijave();
        } ,1000);
        return false;
      }
      return true;

    }

}
