import { Component, Input } from '@angular/core';
import { Channel, ChannelType } from 'src/app/models/channel';

@Component({
  selector: 'app-channel-item',
  template: `
    [{{channel.$type}}]
    <div [ngSwitch]="channel.$type">
      <app-channel-item-telegram *ngSwitchCase="ChannelType.Telegram" [channel]="getChannel()"/>
      <app-channel-item-email *ngSwitchCase="ChannelType.Email" [channel]="getChannel()"/>
    </div>
    ({{channel.id}})
  `
})
export class ChannelItemComponent {
  @Input() channel!: Channel;

  ChannelType: typeof ChannelType = ChannelType;

  getChannel<T>(): T {
    return this.channel as T;
  }
}
