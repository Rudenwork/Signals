import { Component, Input } from '@angular/core';
import { EmailChannel } from 'src/app/models/channel.model';

@Component({
    selector: 'app-channel-verify-email[channel]',
    templateUrl: './channel-verify-email.component.html',
    styleUrls: ['./channel-verify-email.component.scss']
})
export class ChannelVerifyEmailComponent {
    @Input() channel!: EmailChannel;
}
