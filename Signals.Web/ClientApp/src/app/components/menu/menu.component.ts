import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';

@Component({
    selector: 'app-menu',
    templateUrl: './menu.component.html',
    styleUrls: ['./menu.component.scss']
})
export class MenuComponent implements OnInit {
    constructor(private authService: AuthService) { }

    isAdmin: boolean = false;

    ngOnInit() {
        this.authService.isAuthenticatedChanged
            .subscribe(isAuthenticated => {
                if(isAuthenticated) {
                    this.isAdmin = this.authService.isAdmin();
                }
            });
    }
}
