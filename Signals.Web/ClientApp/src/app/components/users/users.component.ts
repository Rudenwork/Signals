import { Component, HostBinding, OnInit, ViewChild } from '@angular/core';
import { User } from 'src/app/models/user.model';
import { DataService } from 'src/app/services/data.service';
import { ModalComponent } from '../modal/modal.component';

@Component({
    selector: 'app-users',
    templateUrl: './users.component.html',
    styleUrls: ['./users.component.scss']
})
export class UsersComponent implements OnInit {
    constructor(private dataService: DataService) {}

    @HostBinding('class.page') isPage: boolean = true;
    @HostBinding('class.loading') isLoading: boolean = true;
    
    @ViewChild('modalCreate') modalCreate!: ModalComponent;
    
    users!: User[];

    ngOnInit() {
        this.dataService.getUsers()
            .subscribe(users => {
                this.users = users;
                this.isLoading = false;
            });
    }

    create(user: User) {
        this.dataService.createUser(user)
            .subscribe({
                next: user => {
                    this.users.push(user);
                    this.modalCreate.close();
                },
                error: () => {
                    this.modalCreate.error();
                }
            });
    }

    remove(index: number) {
        this.users.splice(index, 1);
    }
}
