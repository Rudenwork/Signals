import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { OAuthService } from 'angular-oauth2-oidc';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  constructor(private oAuthService: OAuthService, private router: Router) { }

  ngOnInit() {

    this.oAuthService.setStorage(localStorage);
    this.oAuthService.oidc = false;
    this.oAuthService.clientId = 'client';
    this.oAuthService.scope = 'openid profile offline_access';
    this.oAuthService.tokenEndpoint = `${window.origin}/connect/token`;
    this.oAuthService.userinfoEndpoint = `${window.origin}/connect/userinfo`;

    if (!this.oAuthService.hasValidAccessToken()) {
      this.router.navigate(['login']);
    }
  }
}
