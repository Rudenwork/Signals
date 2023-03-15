import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Channel, ChannelType } from 'src/app/models/channel.model';
import { DataService } from 'src/app/services/data.service';

@Component({
    selector: 'app-channel-delete[id]',
    templateUrl: './channel-delete.component.html',
    styleUrls: ['./channel-delete.component.scss']
})
export class ChannelDeleteComponent implements OnInit{
    constructor(private dataService: DataService) { }

    ngOnInit() {
        this.dataService.getChannel(this.id)
            .subscribe(channel => this.channel = channel);
    }

    @Output() deleted: EventEmitter<any> = new EventEmitter();
    @Input() id?: string;
    isDeleting: boolean = false;
    channel!: any;

    ChannelType: typeof ChannelType = ChannelType;

    del() {
        this.isDeleting = true;
        this.dataService.deleteChannel(this.id)
            .subscribe(() => this.deleted.emit());
    }
}
