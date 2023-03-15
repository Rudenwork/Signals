import { Component, Input } from '@angular/core';
import { EmailChannel } from 'src/app/models/channel.model';

@Component({
    selector: 'app-channel-update-email[channel]',
    templateUrl: './channel-update-email.component.html',
    styleUrls: ['./channel-update-email.component.scss']
})
export class ChannelUpdateEmailComponent {
    @Input() channel!: EmailChannel;
}
