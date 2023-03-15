import { Component, Input } from '@angular/core';
import { EmailChannel } from 'src/app/models/channel.model';

@Component({
  selector: 'app-channel-add-email',
  templateUrl: './channel-add-email.component.html',
  styleUrls: ['./channel-add-email.component.scss']
})
export class ChannelAddEmailComponent {
  @Input() channel!: EmailChannel;
}
