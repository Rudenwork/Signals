import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/models/user.model';

@Component({
    selector: 'app-users',
    templateUrl: './users.component.html',
    styleUrls: ['./users.component.scss']
})
export class UsersComponent implements OnInit {
    users!: User[];

    ngOnInit() {

        let firstUser = new User();
        let secondUser = new User();

        firstUser.username = 'JackSparrow';
        secondUser.username = 'Rudenvad';

        this.users = [ firstUser, secondUser ];
    }
}
