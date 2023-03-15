import { Component, EventEmitter, Output } from '@angular/core';
import { Channel, ChannelType, EmailChannel, TelegramChannel } from 'src/app/models/channel.model';
import { DataService } from 'src/app/services/data.service';

@Component({
    selector: 'app-channel-add',
    templateUrl: './channel-add.component.html',
    styleUrls: ['./channel-add.component.scss']
})
export class ChannelAddComponent {
    constructor(private dataService: DataService) { }
    
    @Output() created: EventEmitter<any> = new EventEmitter();
    channel: Channel = new TelegramChannel();
    isCreating: boolean = false;

    ChannelType: typeof ChannelType = ChannelType;

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

    getTypeOptions(): string[] {
        return Object.keys(ChannelType);
    }

    create() {
        this.isCreating = true;
        this.dataService.createChannel(this.channel)
            .subscribe(() => this.created.emit());
    }
}
