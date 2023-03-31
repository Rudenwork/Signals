import { Component, ElementRef, EventEmitter, Input, OnDestroy, OnInit, Output, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { User } from 'src/app/models/user.model';
import { ModalComponent } from '../../modal/modal.component';

@Component({
    selector: 'app-user-form',
    templateUrl: './user-form.component.html',
    styleUrls: ['./user-form.component.scss']
})
export class UserFormComponent implements OnInit, OnDestroy {
    constructor(private modal: ModalComponent) { }

    @Input() user!: User;
    @Output() submitted: EventEmitter<User> = new EventEmitter();

    username!: FormControl;
    password!: FormControl;
    isAdmin!: FormControl;
    isDisabled!: FormControl;

    form!: FormGroup;
    isCreating: boolean = false;

    ngOnInit() {
        if (this.user == undefined) {
            this.user = new User();
            this.isCreating = true;
        }
        else {
            this.user = { ...this.user };
            delete this.user.id;
            delete this.user.isDisabled;
        }

        this.username = new FormControl(this.user.username, [
            Validators.required,
            Validators.pattern(/^(?!\.)[a-zA-Z0-9._]{1,50}$/)
        ]);

        this.password = new FormControl(this.user.password);

        if (this.isCreating) {
            this.password.addValidators([ Validators.required ]);
        }

        this.isAdmin = new FormControl(this.user.isAdmin);
        this.isDisabled = new FormControl(this.user.isDisabled);

        this.username.valueChanges.subscribe(username => this.user.username = username);
        this.password.valueChanges.subscribe(password => this.user.password = password);
        this.isAdmin.valueChanges.subscribe(isAdmin => this.user.isAdmin = isAdmin);
        this.isDisabled.valueChanges.subscribe(isDisabled => this.user.isDisabled = isDisabled);

        this.form = new FormGroup([
            this.username,
            this.password,
            this.isAdmin
        ]);

        if (this.isCreating) {
            this.form.addControl('isDisabled', this.isDisabled);
        }

        this.modal.form.addControl('user-form', this.form);
        this.modal.submitted.subscribe(() => this.submit());
    }

    ngOnDestroy() {
        this.modal.form.removeControl('user-form');
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

        if (this.isDisabled.pristine) {
            delete this.user.isDisabled;
        }
        
        this.submitted.emit(this.user);
    }
}
