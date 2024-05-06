import {HttpEvent, HttpHandler, HttpInterceptor, HttpRequest} from "@angular/common/http";
import {Observable} from "rxjs";
import {AuthService} from "./AuthService";
import {Injectable} from "@angular/core";
@Injectable()
export class MyHttpInterceptor implements HttpInterceptor {
  constructor(private auth: AuthService) {

  }
     intercept(req: HttpRequest<any>, next: HttpHandler) {
        let clone = req.clone({
          headers: req.headers.set("my-token", this.auth.getToken())
        });
        return next.handle(clone);
    }

}
