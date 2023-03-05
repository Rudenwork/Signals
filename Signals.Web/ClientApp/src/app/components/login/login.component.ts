import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  constructor(private authService: AuthService, private router: Router) { }

  username: string = 'admin';
  password: string = 'admin';

  ngOnInit() {
    if (this.authService.isAuthenticated) {
      this.router.navigate(['/']);
    }
  }

  login() {
    this.authService.login(this.username, this.password)
      .then(() => this.router.navigate(['/']));
  }
}
