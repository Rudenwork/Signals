import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';
import { ModalComponent } from '../modal/modal.component';
import { LoginRequest } from 'src/app/models/login.model';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
    constructor(private authService: AuthService, private router: Router) { }

    @ViewChild('modalLogin') modalLogin!: ModalComponent;

    ngOnInit() {
        if (this.authService.isAuthenticated) {
            this.router.navigate(['/']);
        }
    }

    login(loginRequest: LoginRequest) {
        this.authService.login(loginRequest.username, loginRequest.password)
            .then(() => this.router.navigate(['/']))
            .catch(() => {
                this.modalLogin.error();
                
                this.modalLogin.showClose = true;
                this.modalLogin.closed.subscribe(() => {
                    this.modalLogin.showClose = false;
                });
            });
    }
}
