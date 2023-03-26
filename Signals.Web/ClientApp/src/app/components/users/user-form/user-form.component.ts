import { Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { User } from 'src/app/models/user.model';
import { ModalComponent } from '../../modal/modal.component';

@Component({
    selector: 'app-user-form',
    templateUrl: './user-form.component.html',
    styleUrls: ['./user-form.component.scss']
})
export class UserFormComponent implements OnInit {
    constructor(private modal: ModalComponent) {}

    @Input() user!: User;

    username!: FormControl;
    form!: FormGroup;
    
    ngOnInit() {
        this.user = { ...this.user };

        this.username = new FormControl(this.user.username, [
            Validators.required
        ]);

        this.username.valueChanges.subscribe(username => this.user.username = username);

        this.form = new FormGroup([
            this.username
        ]);

        this.modal.form.addControl("form", this.form);
    }
}
