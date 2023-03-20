import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { TelegramChannel } from 'src/app/models/channel.model';

@Component({
    selector: 'app-channel-update-telegram[channel]',
    templateUrl: './channel-update-telegram.component.html',
    styleUrls: ['./channel-update-telegram.component.scss']
})
export class ChannelUpdateTelegramComponent implements OnInit {
    @Input() channel!: TelegramChannel;
    @Output() formInitialized: EventEmitter<FormGroup> = new EventEmitter<FormGroup>();

    username!: FormControl;
    form!: FormGroup;

    ngOnInit() {
        this.username = new FormControl(this.channel.username, [
            Validators.required,
            Validators.pattern('^[a-zA-Z0-9]+([_ -]?[a-zA-Z0-9])*$')
        ]);

        this.username.valueChanges.subscribe(username => this.channel.username = username);

        this.form = new FormGroup([
            this.username
        ]);

        this.formInitialized.emit(this.form);
    }
}
