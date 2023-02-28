import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { OAuthModule } from 'angular-oauth2-oidc';

import { AppRoutingModule } from './modules/app-routing.module';
import { AppComponent } from './app.component';

import { LoginComponent } from './components/login/login.component';
import { HomeComponent } from './components/home/home.component';
import { SignalsComponent } from './components/signals/signals.component';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    HomeComponent,
    SignalsComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    AppRoutingModule,
    HttpClientModule,
    OAuthModule.forRoot()
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
