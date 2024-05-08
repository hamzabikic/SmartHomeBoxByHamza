import {ActivatedRouteSnapshot, CanDeactivate, RouterStateSnapshot, UrlTree} from "@angular/router";
import {Observable} from "rxjs";
import {Injectable} from "@angular/core";

@Injectable()
export class ComponentCanDeactivate implements CanDeactivate<any> {
    canDeactivate(component: any, currentRoute: ActivatedRouteSnapshot, currentState: RouterStateSnapshot, nextState?: RouterStateSnapshot | undefined): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
        clearInterval(component.provjera);
        if(component.login.utoku) {
          component.provjera = setInterval(async ()=> {component.login.utoku= true; await component.login.provjeraPrijave();
          } ,1000);
          return false;
        }
        return true;
    }

}
