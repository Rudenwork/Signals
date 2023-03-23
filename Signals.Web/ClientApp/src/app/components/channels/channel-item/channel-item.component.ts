import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Channel, ChannelType } from 'src/app/models/channel.model';

@Component({
    selector: 'app-channel-item[channel]',
    templateUrl: './channel-item.component.html',
    styleUrls: ['./channel-item.component.scss']
})
export class ChannelItemComponent {
    @Output() changed: EventEmitter<any> = new EventEmitter();
    @Input() channel!: Channel;

    ChannelType: typeof ChannelType = ChannelType;

    castChannel<T>(): T {
        return this.channel as T;
    }
}
