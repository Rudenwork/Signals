import { Component, Input } from '@angular/core';
import { TelegramChannel } from 'src/app/models/channel.model';

@Component({
  selector: 'app-channel-verify-telegram[channel]',
  templateUrl: './channel-verify-telegram.component.html',
  styleUrls: ['./channel-verify-telegram.component.scss']
})
export class ChannelVerifyTelegramComponent {
    @Input() channel!: TelegramChannel;
}
