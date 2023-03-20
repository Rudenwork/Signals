import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
    constructor(private authService: AuthService, private router: Router) { }

    username!: FormControl;
    password!: FormControl;
    form!: FormGroup;

    ngOnInit() {
        if (this.authService.isAuthenticated) {
            this.router.navigate(['/']);
        }

        this.username = new FormControl('', Validators.required);
        this.password = new FormControl('', Validators.required);

        this.form = new FormGroup([
            this.username,
            this.password
        ]);
    }

    login() {
        this.authService.login(this.username.value, this.password.value)
            .then(() => this.router.navigate(['/']))
            .catch(() => this.form.reset());
    }
}
