import { Injectable } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private oAuthService: OAuthService) { }

  private isAuthenticatedSubject$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  get isAuthenticated(): boolean {
    return this.isAuthenticatedSubject$.getValue();
  }
  private set isAuthenticated(value: boolean) {
    if (this.isAuthenticatedSubject$.getValue() != value) {
      this.isAuthenticatedSubject$.next(value);
    }
  }

  isAuthenticatedChanged: Observable<boolean> = this.isAuthenticatedSubject$;

  init() {
    this.oAuthService.setStorage(localStorage);
    this.oAuthService.oidc = false;
    this.oAuthService.clockSkewInSec = 0;
    this.oAuthService.tokenEndpoint = `${window.origin}/connect/token`;
    this.oAuthService.userinfoEndpoint = `${window.origin}/connect/userinfo`;
    this.oAuthService.clientId = 'client';
    this.oAuthService.scope = 'openid profile offline_access';

    this.isAuthenticated = this.oAuthService.hasValidAccessToken();

    this.oAuthService.events.subscribe(event => {
      if (event.type == 'token_expires') {
        if (this.isAuthenticated) {
          this.oAuthService.refreshToken()
            .catch(() => this.logout());
        }
      }

      if (event.type == 'token_received') {
        this.isAuthenticated = true;
      }
    });
  }

  login(username: string, password: string): Promise<any> {
    return this.oAuthService.fetchTokenUsingPasswordFlow(username, password)
      .then(() => this.isAuthenticated = true);
  }

  logout(): Promise<any> {
    localStorage.clear();
    this.isAuthenticated = false;
    return Promise.resolve();
  }

  getToken(): string {
    return this.oAuthService.getAccessToken();
  }
}
