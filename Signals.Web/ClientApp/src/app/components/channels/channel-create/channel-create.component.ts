import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Channel, ChannelType, EmailChannel, TelegramChannel } from 'src/app/models/channel.model';
import { DataService } from 'src/app/services/data.service';

@Component({
    selector: 'app-channel-create',
    templateUrl: './channel-create.component.html',
    styleUrls: ['./channel-create.component.scss']
})
export class ChannelCreateComponent implements OnInit {
    constructor(private dataService: DataService) { }
    
    @Output() created: EventEmitter<any> = new EventEmitter();

    ChannelType: typeof ChannelType = ChannelType;
    channel: Channel = new TelegramChannel();
    isCreating: boolean = false;

    description!: FormControl;
    form!: FormGroup;

    ngOnInit() {
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

        this.channel.description = oldChannel.description;
    }

    castChannel<T>(): T {
        return this.channel as T;
    }

    setChildForm(form: FormGroup) {
        this.form.setControl('childForm', form);
    }

    getTypeOptions(): string[] {
        return Object.keys(ChannelType);
    }

    create() {
        this.isCreating = true;
        this.dataService.createChannel(this.channel)
            .subscribe(() => this.created.emit());
    }
}
