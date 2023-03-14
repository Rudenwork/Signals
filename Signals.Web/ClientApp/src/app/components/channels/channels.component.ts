import { Component, OnInit, ViewChild } from '@angular/core';
import { Channel } from 'src/app/models/channel.model';
import { DataService } from 'src/app/services/data.service';
import { ModalComponent } from '../modal/modal.component';

@Component({
    selector: 'app-channels',
    templateUrl: './channels.component.html',
    styleUrls: ['./channels.component.scss']
})
export class ChannelsComponent implements OnInit {
    constructor(private dataService: DataService) { }

    channels!: Channel[];

    ngOnInit() {
        this.getChannels();
    }

    getChannels() {
        this.dataService.getChannels()
            .subscribe(channels => this.channels = channels);
    }
}
