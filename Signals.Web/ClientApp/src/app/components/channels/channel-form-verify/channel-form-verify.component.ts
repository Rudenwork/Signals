import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ModalComponent } from '../../modal/modal.component';
import { Channel } from 'src/app/models/channel.model';
import { FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
    selector: 'app-channel-form-verify',
    templateUrl: './channel-form-verify.component.html',
    styleUrls: ['./channel-form-verify.component.scss']
})
export class ChannelFormVerifyComponent implements OnInit {
    constructor(private modal: ModalComponent) { }

    @Input() channel!: Channel;
    @Output() submitted: EventEmitter<any> = new EventEmitter();

    code!: FormControl;

    form!: FormGroup;

    ngOnInit() {
        this.code = new FormControl(this.code, [
            Validators.required,
            Validators.pattern('^[0-9]+$')
        ]);

        this.form = new FormGroup([
            this.code
        ]);

        this.modal.form.addControl('channel-form-verify', this.form);
        this.modal.submitted.subscribe(() => this.submit());
    }

    submit() {
        if (this.form.pristine) {
            return;
        }

        this.submitted.emit(this.code.value);
    }
}
