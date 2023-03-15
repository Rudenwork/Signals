import { Component, Input } from '@angular/core';
import { TelegramChannel } from 'src/app/models/channel.model';

@Component({
    selector: 'app-channel-create-telegram[channel]',
    templateUrl: './channel-create-telegram.component.html',
    styleUrls: ['./channel-create-telegram.component.scss']
})
export class ChannelCreateTelegramComponent {
    @Input() channel!: TelegramChannel;
}
