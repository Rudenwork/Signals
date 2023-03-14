import { Component, Input } from '@angular/core';
import { TelegramChannel } from 'src/app/models/channel';

@Component({
    selector: 'app-channel-item-telegram',
    template: `
        [Username: {{channel.username}}]
    `
})
export class ChannelItemTelegramComponent {
    @Input() channel!: TelegramChannel;
}
