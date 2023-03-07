import { Component, Input } from '@angular/core';
import { EmailChannel } from 'src/app/services/data.service';

@Component({
  selector: 'app-channel-item-email',
  template: `
    [Address: {{channel.address}}]
  `
})
export class ChannelItemEmailComponent {
  @Input() channel!: EmailChannel;
}
