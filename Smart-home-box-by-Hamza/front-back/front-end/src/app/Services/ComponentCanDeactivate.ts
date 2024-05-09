import {ActivatedRouteSnapshot, CanDeactivate, RouterStateSnapshot, UrlTree} from "@angular/router";
import {Observable} from "rxjs";
import {Injectable} from "@angular/core";

@Injectable()
export class ComponentCanDeactivate implements CanDeactivate<any> {
    canDeactivate(component: any, currentRoute: ActivatedRouteSnapshot, currentState: RouterStateSnapshot, nextState?: RouterStateSnapshot | undefined): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
      clearInterval(component.provjera);
      clearInterval(component.interval);
      clearInterval(component.interval2);
       return true;
    }

}
