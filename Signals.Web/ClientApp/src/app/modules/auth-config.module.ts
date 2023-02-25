import { NgModule } from '@angular/core';
import { AuthModule } from 'angular-auth-oidc-client';


@NgModule({
  imports: [AuthModule.forRoot({
    config: {
      authority: 'http://localhost:5020',
      redirectUrl: 'login',
      clientId: 'client',
      scope: 'openid profile offline_access',
      responseType: 'id_token token',
      silentRenew: true,
      useRefreshToken: true
    }
  })],
  exports: [AuthModule],
})
export class AuthConfigModule { }
