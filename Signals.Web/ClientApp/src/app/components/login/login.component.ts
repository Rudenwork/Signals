import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { OAuthService } from 'angular-oauth2-oidc';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  constructor(private oAuthService: OAuthService, private router: Router) { }

  username: string = 'admin';
  password: string = 'admin';

  ngOnInit() {
    if (this.oAuthService.hasValidAccessToken()) {
      this.router.navigate(['/']);
    }
  }

  login() {
    this.oAuthService.fetchTokenUsingPasswordFlow(this.username, this.password)
      .then(() => {
        this.router.navigate(['/']);
      });
  }
}
