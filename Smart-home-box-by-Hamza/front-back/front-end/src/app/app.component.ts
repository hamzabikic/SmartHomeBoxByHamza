import { Component } from '@angular/core';
import {AuthService} from "./Services/AuthService";
import {InfoClass} from "./Services/InfoClass";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  constructor(protected auth:AuthService) {
  }

  protected readonly InfoClass = InfoClass;
}
