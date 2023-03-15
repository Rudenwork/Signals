import { Component, Input } from '@angular/core';
import { EmailChannel } from 'src/app/models/channel.model';

@Component({
    selector: 'app-channel-delete-email[channel]',
    templateUrl: './channel-delete-email.component.html',
    styleUrls: ['./channel-delete-email.component.scss']
})
export class ChannelDeleteEmailComponent {
    @Input() channel!: EmailChannel;
}
