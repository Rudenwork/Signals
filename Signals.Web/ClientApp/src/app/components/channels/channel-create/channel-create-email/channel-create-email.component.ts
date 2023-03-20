import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { EmailChannel } from 'src/app/models/channel.model';

@Component({
    selector: 'app-channel-create-email[channel]',
    templateUrl: './channel-create-email.component.html',
    styleUrls: ['./channel-create-email.component.scss']
})
export class ChannelCreateEmailComponent {
    @Input() channel!: EmailChannel;
    @Output() formInitialized: EventEmitter<FormGroup> = new EventEmitter<FormGroup>();

    address!: FormControl;
    form!: FormGroup;

    ngOnInit() {
        this.address = new FormControl(this.channel.address, [
            Validators.required,
            Validators.email
        ]);
        
        this.address.valueChanges.subscribe(address => this.channel.address = address);

        this.form = new FormGroup([
            this.address
        ]);

        this.formInitialized.emit(this.form);
    }
}
