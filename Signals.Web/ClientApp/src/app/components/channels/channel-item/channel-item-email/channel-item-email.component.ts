import { Component, Input } from '@angular/core';
import { EmailChannel } from 'src/app/models/channel.model';

@Component({
    selector: 'app-channel-item-email[channel]',
    templateUrl: './channel-item-email.component.html',
    styleUrls: ['./channel-item-email.component.scss']
})
export class ChannelItemEmailComponent {
    @Input() channel!: EmailChannel;
}
