import { Component, EventEmitter, Input, Output, ViewChild } from '@angular/core';
import { User } from 'src/app/models/user.model';
import { DataService } from 'src/app/services/data.service';
import { ModalComponent } from '../../modal/modal.component';

@Component({
    selector: 'app-user[user]',
    templateUrl: './user.component.html',
    styleUrls: ['./user.component.scss']
})
export class UserComponent {
    constructor(private dataService: DataService) {}

    @ViewChild('modalDelete') modalDelete!: ModalComponent;
    @ViewChild('modalUpdate') modalUpdate!: ModalComponent;

    @Input() user!: User;
    @Output() deleted: EventEmitter<any> = new EventEmitter();

    update(user: User) {
        this.dataService.updateUser(this.user.id ?? '', user)
            .subscribe({
                next: user => {
                    this.user = user;
                    this.modalUpdate.close();
                },
                error: error => {
                    this.modalUpdate.error();
                }
            });
    }

    del() {
        this.dataService.deleteUser(this.user.id ?? '')
            .subscribe({
                next: () => {
                    this.deleted.emit();
                    this.modalDelete.close();
                },
                error: error => {
                    this.modalDelete.error();
                }
            });
    }
}
