import {Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {Router} from "@angular/router";
import {Observable} from "rxjs";

@Injectable()
export class AuthService {

  constructor (private http: HttpClient, private router: Router) {

  }
  jeLogiran() {
    let obj = localStorage.getItem("my-token");
    if(obj == null) return false;
    return true;
    }


 getId() :number{
    if(this.jeLogiran()) {
      let obj = JSON.parse(localStorage.getItem("my-token")!);
      return obj.korisnikId;
    }
    return 0;
  }
 getToken() :string {
  if (this.jeLogiran()) {
    let obj = JSON.parse(localStorage.getItem("my-token")!);
    return obj.token;
  }
  return "";
  }
  getPrijavaId ():number{
    if (this.jeLogiran()) {
      let obj = JSON.parse(localStorage.getItem("my-token")!);
      return obj.id;
    }
    return 0;
  }

}
