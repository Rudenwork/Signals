import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { ModalComponent } from '../../modal/modal.component';
import { LoginRequest } from 'src/app/models/login.model';
import { FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
    selector: 'app-login-form',
    templateUrl: './login-form.component.html',
    styleUrls: ['./login-form.component.scss']
})
export class LoginFormComponent implements OnInit {
    constructor(private modal: ModalComponent) { }

    @Output() submitted: EventEmitter<LoginRequest> = new EventEmitter();

    username!: FormControl;
    password!: FormControl;
    form!: FormGroup;

    loginRequest!: LoginRequest;

    ngOnInit() {
        this.loginRequest = new LoginRequest();

        this.username = new FormControl('', [
            Validators.required,
            Validators.pattern(/^(?!\.)[a-zA-Z0-9._]{1,50}$/)
        ]);

        this.password = new FormControl('', [
            Validators.required
        ]);

        this.username.valueChanges.subscribe(username => this.loginRequest.username = username);
        this.password.valueChanges.subscribe(password => this.loginRequest.password = password);

        this.form = new FormGroup([
            this.username,
            this.password
        ]);

        this.modal.form.addControl('login-form', this.form);
        this.modal.submitted.subscribe(() => this.submit());
    }

    submit() {
        if (this.form.pristine) {
            return;
        }
        
        this.submitted.emit(this.loginRequest);
        this.form.reset();
    }
}
