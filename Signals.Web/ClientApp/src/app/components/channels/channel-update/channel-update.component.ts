import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Channel, ChannelType, EmailChannel, TelegramChannel } from 'src/app/models/channel.model';
import { DataService } from 'src/app/services/data.service';

@Component({
    selector: 'app-channel-update[channel]',
    templateUrl: './channel-update.component.html',
    styleUrls: ['./channel-update.component.scss']
})
export class ChannelUpdateComponent implements OnInit {
    constructor(private dataService: DataService) { }

    @Output() updated: EventEmitter<any> = new EventEmitter();
    @Input() channel!: Channel;
    isUpdating: boolean = false;

    ChannelType: typeof ChannelType = ChannelType;

    ngOnInit() {
        this.channel = { ...this.channel };
    }

    castChannel<T>(): T {
        return this.channel as T;
    }

    update() {
        this.isUpdating = true;
        this.dataService.updateChannel(this.channel.id ?? '', this.channel)
            .subscribe(() => this.updated.emit());
    }
}
