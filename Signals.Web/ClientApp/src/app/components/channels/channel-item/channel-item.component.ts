import { Component, Input } from '@angular/core';
import { Channel, ChannelType } from 'src/app/models/channel.model';

@Component({
    selector: 'app-channel-item',
    templateUrl: './channel-item.component.html',
    styleUrls: ['./channel-item.component.scss']
})
export class ChannelItemComponent {
    @Input() channel!: Channel;

    ChannelType: typeof ChannelType = ChannelType;

    castChannel<T>(): T {
        return this.channel as T;
    }
}
