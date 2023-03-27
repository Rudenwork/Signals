import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/models/user.model';
import { DataService } from 'src/app/services/data.service';

@Component({
    selector: 'app-users',
    templateUrl: './users.component.html',
    styleUrls: ['./users.component.scss']
})
export class UsersComponent implements OnInit {
    constructor(private dataService: DataService) {}
    
    users!: User[];

    ngOnInit() {
        this.dataService.getUsers()
            .subscribe(users => this.users = users);
    }

    create(user: User) {
        this.dataService.createUser(user)
            .subscribe(user => this.users.push(user));
    }

    remove(index: number) {
        this.users.splice(index, 1);
    }
}
