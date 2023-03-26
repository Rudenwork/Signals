import { Component, Input } from '@angular/core';
import { User } from 'src/app/models/user.model';

@Component({
    selector: 'app-user[user]',
    templateUrl: './user.component.html',
    styleUrls: ['./user.component.scss']
})
export class UserComponent {
    @Input() user!: User;
}
