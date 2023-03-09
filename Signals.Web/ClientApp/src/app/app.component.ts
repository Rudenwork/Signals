import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from './services/auth.service';

@Component({
  selector: 'app-root',
  template: `
    <app-menu *ngIf="isAuthenticated"></app-menu>
    <router-outlet></router-outlet>
  `,
  styles: [`
    :host {
      background: rgb(53, 54, 58);
      height: 100%;
      width: 100%;
      display: flex;
    }
  `]
})
export class AppComponent implements OnInit {
  constructor(private authService: AuthService, private router: Router) { }

  isAuthenticated!: boolean;

  ngOnInit() {
    this.authService.init();
    this.authService.isAuthenticatedChanged.subscribe(isAuthenticated => {
      this.isAuthenticated = isAuthenticated;
      
      if (!isAuthenticated) {
        this.router.navigate(['login']);
      }
    });
  }
}
