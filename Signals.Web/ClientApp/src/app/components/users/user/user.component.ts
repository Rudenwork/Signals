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
    constructor(private dataService: DataService) { }

    @ViewChild('modalDelete') modalDelete!: ModalComponent;
    @ViewChild('modalUpdate') modalUpdate!: ModalComponent;
    @ViewChild('modalEnable') modalEnable!: ModalComponent;
    @ViewChild('modalDisable') modalDisable!: ModalComponent;

    @Input() user!: User;
    @Output() deleted: EventEmitter<any> = new EventEmitter();

    update(user: User) {
        this.dataService.updateUser(this.user.id ?? '', user)
            .subscribe({
                next: user => {
                    this.user = user;
                    this.modalUpdate.close();
                },
                error: () => {
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
                error: () => {
                    this.modalDelete.error();
                }
            });
    }

    enable() {
        this.dataService.enableUser(this.user.id ?? '')
            .subscribe({
                next: user => {
                    this.user = user;
                    this.modalEnable.close();
                },
                error: () => {
                    this.modalEnable.error();
                }
            });
    }

    disable() {
        this.dataService.disableUser(this.user.id ?? '')
            .subscribe({
                next: user => {
                    this.user = user;
                    this.modalDisable.close();
                },
                error: () => {
                    this.modalDisable.error();
                }
            });
    }
}
