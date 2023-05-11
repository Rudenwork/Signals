import { Component, HostBinding, OnInit, ViewChild } from '@angular/core';
import { User } from 'src/app/models/user.model';
import { DataService } from 'src/app/services/data.service';
import { ModalComponent } from '../../modal/modal.component';

@Component({
    selector: 'app-users',
    templateUrl: './users-page.component.html',
    styleUrls: ['./users-page.component.scss']
})
export class UsersPageComponent implements OnInit {
    constructor(private dataService: DataService) {}

    @HostBinding('class.page') isPage: boolean = true;
    @HostBinding('class.loading') isLoading: boolean = true;
    
    @ViewChild('modalCreate') modalCreate!: ModalComponent;
    
    users: User[] = [];

    ngOnInit() {
        this.dataService.getUsers()
            .subscribe(users => {
                this.users = users;
                this.sort();
                this.isLoading = false;
            });
    }

    sort() {
        this.users = this.users.sort((a, b) => a.username!.localeCompare(b!.username ?? ''));
    }

    create(user: User) {
        this.dataService.createUser(user)
            .subscribe({
                next: user => {
                    this.users.push(user);
                    this.sort();
                    this.modalCreate.close();
                },
                error: () => {
                    this.modalCreate.error();
                }
            });
    }

    remove(index: number) {
        this.users.splice(index, 1);
        this.sort();
    }
}
