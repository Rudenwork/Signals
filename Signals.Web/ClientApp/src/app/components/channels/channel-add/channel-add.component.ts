import { Component } from '@angular/core';
import { ChannelType, TelegramChannel } from 'src/app/models/channel.model';
import { DataService } from 'src/app/services/data.service';
import { ModalComponent } from '../../modal/modal.component';

@Component({
    selector: 'app-channel-add',
    templateUrl: './channel-add.component.html',
    styleUrls: ['./channel-add.component.scss']
})
export class ChannelAddComponent {
    constructor(private modal: ModalComponent, private dataService: DataService) { }
    test() {
        let channel = new TelegramChannel();
        channel.$type = ChannelType.Telegram;
        channel.username = 'test';

        this.dataService.createChannel(channel)
            .subscribe(channel => this.modal.close());
    }
}
