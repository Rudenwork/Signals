import { Component, HostBinding, OnInit, ViewChild } from '@angular/core';
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

    @HostBinding('class.page') isPage: boolean = true;
    @HostBinding('class.loading') isLoading: boolean = true;

    @ViewChild('modalCreate') modalCreate!: ModalComponent;

    channels: Channel[] = [];

    ngOnInit() {
        this.dataService.getChannels()
            .subscribe(channels => {
                this.channels = channels;
                this.sort();
                this.isLoading = false;
            });
    }

    sort() {
        this.channels = this.channels.sort((a, b) => {
            let typeResult = b.type!.localeCompare(a!.type ?? '');

            if (typeResult == 0) {
                return a.destination!.localeCompare(b!.destination ?? '');
            }
            
            return typeResult;
        });
    }

    create(channel: Channel) {
        this.dataService.createChannel(channel)
            .subscribe({
                next: channel => {
                    this.channels.push(channel);
                    this.sort();
                    this.modalCreate.close();
                },
                error: () => {
                    this.modalCreate.error();
                }
            });
    }

    remove(index: number) {
        this.channels.splice(index, 1);
        this.sort();
    }
}
