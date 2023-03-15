import { Component, Input } from '@angular/core';
import { TelegramChannel } from 'src/app/models/channel.model';

@Component({
    selector: 'app-channel-update-telegram[channel]',
    templateUrl: './channel-update-telegram.component.html',
    styleUrls: ['./channel-update-telegram.component.scss']
})
export class ChannelUpdateTelegramComponent {
    @Input() channel!: TelegramChannel;
}
