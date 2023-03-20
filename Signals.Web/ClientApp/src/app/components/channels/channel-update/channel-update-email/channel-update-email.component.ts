import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { EmailChannel } from 'src/app/models/channel.model';

@Component({
    selector: 'app-channel-update-email[channel]',
    templateUrl: './channel-update-email.component.html',
    styleUrls: ['./channel-update-email.component.scss']
})
export class ChannelUpdateEmailComponent implements OnInit {
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
