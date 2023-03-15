import { Component, Input } from '@angular/core';
import { TelegramChannel } from 'src/app/models/channel.model';

@Component({
  selector: 'app-channel-add-telegram',
  templateUrl: './channel-add-telegram.component.html',
  styleUrls: ['./channel-add-telegram.component.scss']
})
export class ChannelAddTelegramComponent {
  @Input() channel!: TelegramChannel;
}
