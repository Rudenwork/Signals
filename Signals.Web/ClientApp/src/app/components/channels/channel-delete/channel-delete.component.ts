import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Channel, ChannelType } from 'src/app/models/channel.model';
import { DataService } from 'src/app/services/data.service';

@Component({
    selector: 'app-channel-delete[channel]',
    templateUrl: './channel-delete.component.html',
    styleUrls: ['./channel-delete.component.scss']
})
export class ChannelDeleteComponent {
    constructor(private dataService: DataService) { }

    @Output() deleted: EventEmitter<any> = new EventEmitter();
    @Input() channel!: Channel;
    isDeleting: boolean = false;

    ChannelType: typeof ChannelType = ChannelType;

    castChannel<T>(): T {
        return this.channel as T;
    }

    del() {
        this.isDeleting = true;
        this.dataService.deleteChannel(this.channel.id ?? '')
            .subscribe(() => this.deleted.emit());
    }
}
