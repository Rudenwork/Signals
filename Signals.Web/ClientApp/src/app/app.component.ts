import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from './services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  constructor(private authService: AuthService, private router: Router) { }

  isAuthenticated!: boolean;

  ngOnInit() {
    this.authService.isAuthenticatedChanged.subscribe(isAuthenticated => {
      if (!isAuthenticated) {
        this.router.navigate(['login']);
      }
      console.log('ZZZ', isAuthenticated);

      this.isAuthenticated = isAuthenticated;
    });

    this.authService.init();
  }
}
