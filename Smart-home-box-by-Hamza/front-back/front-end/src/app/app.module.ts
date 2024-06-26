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
import { PasswordComponent } from './password/password.component';
import { ApplicationsComponent } from './applications/applications.component';
import {ComponentCanDeactivate} from "./Services/ComponentCanDeactivate";
import { TemphumchartComponent } from './temphumchart/temphumchart.component';

@NgModule({
  declarations: [
    AppComponent,
    TemphumComponent,
    GasfireComponent,
    SecurityComponent,
    ProfileComponent,
    LightComponent,
    LoginComponent,
    PasswordComponent,
    ApplicationsComponent,
    TemphumchartComponent
  ],
    imports: [
        BrowserModule,
        FormsModule,
      RouterModule.forRoot([
        {path:"temperaturehumidity", component:TemphumchartComponent, canActivate:[ComponentsCanActivate],
        canDeactivate:[ComponentCanDeactivate], runGuardsAndResolvers: 'always'},
        {path:"temphumdata", component:TemphumComponent, canActivate:[ComponentsCanActivate],
          canDeactivate:[ComponentCanDeactivate], runGuardsAndResolvers: 'always'},
        {path:"gasfire", component:GasfireComponent, canActivate:[ComponentsCanActivate]
          ,
          canDeactivate:[ComponentCanDeactivate], runGuardsAndResolvers: 'always'} ,
        {path:"security", component:SecurityComponent, canActivate:[ComponentsCanActivate]
          ,
          canDeactivate:[ComponentCanDeactivate], runGuardsAndResolvers: 'always'},
        {path:"profile", component:ProfileComponent, canActivate:[ComponentsCanActivate],
        canDeactivate:[ComponentCanDeactivate], runGuardsAndResolvers: 'always'},
        {path:"light", component:LightComponent, canActivate:[ComponentsCanActivate],
        canDeactivate:[ComponentCanDeactivate], runGuardsAndResolvers: 'always'},
        {path:"login", component:LoginComponent , canActivate:[LoginCanActivate], runGuardsAndResolvers: 'always'},
        {path:"password", component:PasswordComponent, canActivate:[LoginCanActivate], runGuardsAndResolvers: 'always'},
        {path:"applications", component:ApplicationsComponent, canActivate:[ComponentsCanActivate] ,
          canDeactivate:[ComponentCanDeactivate], runGuardsAndResolvers: 'always'},
        {path:"**", redirectTo:"profile", pathMatch:"full"}
      ],{ useHash: true }),
      HttpClientModule,
    ],
  providers: [AuthService, {provide:HTTP_INTERCEPTORS, multi:true, useClass:MyHttpInterceptor}, LoginCanActivate,
  ComponentsCanActivate, LoginProvjera, ComponentCanDeactivate],
  bootstrap: [AppComponent]
})
export class AppModule { }
