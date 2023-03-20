import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { TelegramChannel } from 'src/app/models/channel.model';

@Component({
    selector: 'app-channel-create-telegram[channel]',
    templateUrl: './channel-create-telegram.component.html',
    styleUrls: ['./channel-create-telegram.component.scss']
})
export class ChannelCreateTelegramComponent implements OnInit {
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
