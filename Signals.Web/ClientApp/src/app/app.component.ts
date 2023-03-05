import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { OAuthService } from 'angular-oauth2-oidc';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  constructor(private authService: OAuthService, private router: Router) { }

  hasValidAccessToken: boolean = false;

  ngOnInit() {

    this.authService.setStorage(localStorage);
    this.authService.oidc = false;
    this.authService.clockSkewInSec = 0;
    this.authService.tokenEndpoint = `${window.origin}/connect/token`;
    this.authService.userinfoEndpoint = `${window.origin}/connect/userinfo`;
    this.authService.clientId = 'client';
    this.authService.scope = 'openid profile offline_access';

    this.hasValidAccessToken = this.authService.hasValidAccessToken();

    this.authService.events.subscribe(event => {
      if (event.type == 'token_expires') {
        console.log(event.type);
        this.refreshToken();
      }

      if (event.type == 'token_received') {
        console.log(event.type);
        this.hasValidAccessToken = true;
      }
    });
  }

  refreshToken(): Promise<any> {
    return this.authService.refreshToken()
      .catch(() => {
        this.hasValidAccessToken = false;
        this.router.navigate(['logout']);
      });
  }
}
