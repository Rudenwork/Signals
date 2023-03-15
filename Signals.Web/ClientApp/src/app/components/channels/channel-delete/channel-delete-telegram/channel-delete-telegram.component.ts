import { Component, Input } from '@angular/core';
import { TelegramChannel } from 'src/app/models/channel.model';

@Component({
    selector: 'app-channel-delete-telegram[channel]',
    templateUrl: './channel-delete-telegram.component.html',
    styleUrls: ['./channel-delete-telegram.component.scss']
})
export class ChannelDeleteTelegramComponent {
    @Input() channel!: TelegramChannel;
}
