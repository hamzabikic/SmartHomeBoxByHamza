import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import {FormsModule} from "@angular/forms";
import {RouterModule} from "@angular/router";
import { TemphumComponent } from './temphum/temphum.component';
import {HTTP_INTERCEPTORS, HttpClientModule} from "@angular/common/http";
import { GasfireComponent } from './gasfire/gasfire.component';
import { SecurityComponent } from './security/security.component';
import { ProfileComponent } from './profile/profile.component';
import { LightComponent } from './light/light.component';
import { LoginComponent } from './login/login.component';
import {AuthService} from "./Services/AuthService";
import {MyHttpInterceptor} from "./Services/MyHttpInterceptor";
import {ComponentsCanActivate} from "./Services/ComponentsCanActivate";
import {LoginCanActivate} from "./Services/LoginCanActivate";
import {LoginProvjera} from "./Services/LoginProvjera";

@NgModule({
  declarations: [
    AppComponent,
    TemphumComponent,
    GasfireComponent,
    SecurityComponent,
    ProfileComponent,
    LightComponent,
    LoginComponent
  ],
    imports: [
        BrowserModule,
        FormsModule,
      RouterModule.forRoot([
        {path:"", redirectTo:"profile", pathMatch:"full"},
        {path:"temperaturehumidity", component:TemphumComponent, canActivate:[ComponentsCanActivate]},
        {path:"gasfire", component:GasfireComponent, canActivate:[ComponentsCanActivate]},
        {path:"security", component:SecurityComponent, canActivate:[ComponentsCanActivate]},
        {path:"profile", component:ProfileComponent, canActivate:[ComponentsCanActivate]},
        {path:"light", component:LightComponent, canActivate:[ComponentsCanActivate]},
        {path:"login", component:LoginComponent , canActivate:[LoginCanActivate]}
      ]),
      HttpClientModule
    ],
  providers: [AuthService, {provide:HTTP_INTERCEPTORS, multi:true, useClass:MyHttpInterceptor}, LoginCanActivate,
  ComponentsCanActivate, LoginProvjera],
  bootstrap: [AppComponent]
})
export class AppModule { }
