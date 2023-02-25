import { NgModule } from '@angular/core';
import { AuthModule } from 'angular-auth-oidc-client';


@NgModule({
  imports: [AuthModule.forRoot({
    config: {
      authority: 'http://localhost:5020',
      clientId: 'client',
      scope: 'openid profile offline_access',
      responseType: 'password',
      silentRenew: true,
      useRefreshToken: true
    }
  })],
  exports: [AuthModule],
})
export class AuthConfigModule { }
