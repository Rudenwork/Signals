import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { OAuthService } from 'angular-oauth2-oidc';
import { SettingsService } from './services/settings.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  constructor(private oAuthService: OAuthService, private router: Router, private settingsService: SettingsService) { }

  ngOnInit() {
    this.settingsService.getSettings()
      .subscribe(settings => {

        this.oAuthService.setStorage(localStorage);
        this.oAuthService.oidc = false;
        this.oAuthService.clientId = 'client';
        this.oAuthService.scope = 'openid profile offline_access';
        this.oAuthService.tokenEndpoint = `${settings.apiBaseAddress}/connect/token`;
        this.oAuthService.userinfoEndpoint = `${settings.apiBaseAddress}/connect/userinfo`;

        if (!this.oAuthService.hasValidAccessToken()) {
          this.router.navigate(['login']);
        }
      });
  }
}
