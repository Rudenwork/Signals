import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Channel, ChannelType, EmailChannel, TelegramChannel } from 'src/app/models/channel.model';
import { DataService } from 'src/app/services/data.service';

@Component({
    selector: 'app-channel-update[channel]',
    templateUrl: './channel-update.component.html',
    styleUrls: ['./channel-update.component.scss']
})
export class ChannelUpdateComponent implements OnInit {
    constructor(private dataService: DataService) { }

    @Input() channel!: Channel;
    @Output() updated: EventEmitter<any> = new EventEmitter();

    ChannelType: typeof ChannelType = ChannelType;
    isUpdating: boolean = false;

    description!: FormControl;
    form!: FormGroup;

    ngOnInit() {
        this.channel = { ...this.channel };

        this.description = new FormControl(this.channel.description, Validators.maxLength(100));
        this.description.valueChanges.subscribe(description => this.channel.description = description);

        this.form = new FormGroup([
            this.description
        ]);
    }

    changeChannelType(type: string) {
        let oldChannel = this.channel;

        if (type == ChannelType.Telegram) {
            this.channel = new TelegramChannel();
        }
        else if (type == ChannelType.Email) {
            this.channel = new EmailChannel();
        }

        this.channel.id = oldChannel.id;
        this.channel.description = oldChannel.description;
    }

    castChannel<T>(): T {
        return this.channel as T;
    }

    setChildForm(form: FormGroup) {
        Promise.resolve()
            .then(() => this.form.setControl('childForm', form));
    }

    getTypeOptions(): string[] {
        return Object.keys(ChannelType);
    }

    update() {
        let id = this.channel.id  ?? '';
        this.trimChannel();

        this.isUpdating = true;
        this.dataService.updateChannel(id, this.channel)
            .subscribe(() => this.updated.emit());
    }

    trimChannel() {
        delete this.channel.id;
        delete this.channel.userId;
        delete this.channel.isVerified;
    }
}
