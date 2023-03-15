import { Component, Input } from '@angular/core';
import { EmailChannel } from 'src/app/models/channel.model';

@Component({
    selector: 'app-channel-create-email[channel]',
    templateUrl: './channel-create-email.component.html',
    styleUrls: ['./channel-create-email.component.scss']
})
export class ChannelCreateEmailComponent {
    @Input() channel!: EmailChannel;
}
