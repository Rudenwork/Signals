import { Component, Input, OnInit } from '@angular/core';
import { Channel } from 'src/app/models/channel.model';
import { NotificationStage } from 'src/app/models/signal.model';
import { DataService } from 'src/app/services/data.service';

@Component({
    selector: 'app-notification-preview',
    templateUrl: './notification-preview.component.html',
    styleUrls: ['./notification-preview.component.scss']
})
export class NotificationPreviewComponent implements OnInit {
    constructor(private dataService: DataService) { }

    @Input() stage!: NotificationStage;
    channel!: Channel;

    ngOnInit() {
        this.dataService.getChannel(this.stage.channelId ?? '')
            .subscribe(channel => {
                this.channel = channel;
            });
    }
}
