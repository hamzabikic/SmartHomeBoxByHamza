import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import {FormsModule} from "@angular/forms";
import {RouterModule} from "@angular/router";
import { TemphumComponent } from './temphum/temphum.component';
import {HttpClientModule} from "@angular/common/http";
import { GasfireComponent } from './gasfire/gasfire.component';
import { SecurityComponent } from './security/security.component';
import { ProfileComponent } from './profile/profile.component';
import { LightComponent } from './light/light.component';

@NgModule({
  declarations: [
    AppComponent,
    TemphumComponent,
    GasfireComponent,
    SecurityComponent,
    ProfileComponent,
    LightComponent
  ],
    imports: [
        BrowserModule,
        FormsModule,
      RouterModule.forRoot([
        {path:"", redirectTo:"profile", pathMatch:"full"},
        {path:"temperaturehumidity", component:TemphumComponent},
        {path:"gasfire", component:GasfireComponent},
        {path:"security", component:SecurityComponent},
        {path:"profile", component:ProfileComponent},
        {path:"light", component:LightComponent}
      ]),
      HttpClientModule
    ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
