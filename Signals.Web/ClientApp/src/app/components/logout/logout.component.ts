import { Component } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';

@Component({
    selector: 'app-logout',
    templateUrl: './logout.component.html',
})
export class LogoutComponent {
    constructor(private authService: AuthService) { }

    close() {
        history.back();
    }

    submit() {
        this.authService.logout();
    }
}
