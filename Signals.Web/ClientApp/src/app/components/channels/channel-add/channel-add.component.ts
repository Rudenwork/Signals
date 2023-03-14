import { Component, EventEmitter, Output } from '@angular/core';
import { ChannelType, TelegramChannel } from 'src/app/models/channel.model';
import { DataService } from 'src/app/services/data.service';

@Component({
    selector: 'app-channel-add',
    templateUrl: './channel-add.component.html',
    styleUrls: ['./channel-add.component.scss']
})
export class ChannelAddComponent {
    constructor(private dataService: DataService) { }

    @Output() completed: EventEmitter<any> = new EventEmitter();

    test() {
        let channel = new TelegramChannel();
        channel.$type = ChannelType.Telegram;
        channel.username = 'test';

        this.dataService.createChannel(channel)
            .subscribe(() => this.completed.emit());
    }
}
