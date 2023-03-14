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
        this.authService.init();
        this.authService.isAuthenticatedChanged.subscribe(isAuthenticated => {
            this.isAuthenticated = isAuthenticated;

            if (!isAuthenticated) {
                this.router.navigate(['login']);
            }
        });
    }
}
