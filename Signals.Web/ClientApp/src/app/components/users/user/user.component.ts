import { Component, EventEmitter, Input, Output, ViewChild } from '@angular/core';
import { User } from 'src/app/models/user.model';
import { DataService } from 'src/app/services/data.service';

@Component({
    selector: 'app-user[user]',
    templateUrl: './user.component.html',
    styleUrls: ['./user.component.scss']
})
export class UserComponent {
    constructor(private dataService: DataService) {}

    @Input() user!: User;
    @Output() deleted: EventEmitter<any> = new EventEmitter();

    update(user: User) {
        this.dataService.updateUser(this.user.id ?? '', user)
            .subscribe(user => this.user = user);
    }

    del() {
        this.dataService.deleteUser(this.user.id ?? '')
            .subscribe(() => this.deleted.emit());
    }
}
