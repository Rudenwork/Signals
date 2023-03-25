import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Channel, ChannelType } from 'src/app/models/channel.model';
import { DataService } from 'src/app/services/data.service';

@Component({
    selector: 'app-channel-item[channel]',
    templateUrl: './channel-item.component.html',
    styleUrls: ['./channel-item.component.scss']
})
export class ChannelItemComponent {
    constructor(private dataService: DataService) { }
    
    @Input() channel!: Channel;
    @Output() changed: EventEmitter<any> = new EventEmitter();

    ChannelType: typeof ChannelType = ChannelType;

    castChannel<T>(): T {
        return this.channel as T;
    }

    del() {
        this.dataService.deleteChannel(this.channel.id ?? '')
            .subscribe(() => this.changed.emit());
    }
}
