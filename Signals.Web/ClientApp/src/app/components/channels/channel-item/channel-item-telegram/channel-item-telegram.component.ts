import { Component, Input } from '@angular/core';
import { TelegramChannel } from 'src/app/models/channel.model';

@Component({
    selector: 'app-channel-item-telegram',
    templateUrl: './channel-item-telegram.component.html',
    styleUrls: ['./channel-item-telegram.component.scss']
})
export class ChannelItemTelegramComponent {
    @Input() channel!: TelegramChannel;
}
