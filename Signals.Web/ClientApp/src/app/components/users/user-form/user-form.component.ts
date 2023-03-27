import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { User } from 'src/app/models/user.model';
import { ModalComponent } from '../../modal/modal.component';

@Component({
    selector: 'app-user-form',
    templateUrl: './user-form.component.html',
    styleUrls: ['./user-form.component.scss']
})
export class UserFormComponent implements OnInit {
    constructor(private modal: ModalComponent) { }

    @Input() user!: User;
    @Output() submitted: EventEmitter<User> = new EventEmitter();

    username!: FormControl;
    password!: FormControl;
    isAdmin!: FormControl;

    form!: FormGroup;

    ngOnInit() {
        if (this.user == undefined) {
            this.user = new User();
        }
        else {
            this.user = { ...this.user };
            delete this.user.id;
            delete this.user.isDisabled;
        }

        this.username = new FormControl(this.user.username, [
            Validators.required
        ]);

        this.password = new FormControl(this.user.password);
        this.isAdmin = new FormControl(this.user.isAdmin);

        this.username.valueChanges.subscribe(username => this.user.username = username);
        this.password.valueChanges.subscribe(password => this.user.password = password);
        this.isAdmin.valueChanges.subscribe(isAdmin => this.user.isAdmin = isAdmin);

        this.form = new FormGroup([
            this.username,
            this.password,
            this.isAdmin
        ]);

        this.modal.form.addControl("form", this.form);
        this.modal.submitted.subscribe(() => this.submit());
    }

    submit() {
        if (this.form.pristine) {
            return;
        }

        if (this.username.pristine) {
            delete this.user.username;
        }
            

        if (this.password.pristine) {
            delete this.user.password;
        }

        if (this.isAdmin.pristine) {
            delete this.user.isAdmin;
        }
        
        this.submitted.emit(this.user);
    }
}
