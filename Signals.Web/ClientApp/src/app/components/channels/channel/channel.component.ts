import { Component, EventEmitter, Input, Output, ViewChild } from '@angular/core';
import { DataService } from 'src/app/services/data.service';
import { ModalComponent } from '../../modal/modal.component';
import { Channel, ChannelType } from 'src/app/models/channel.model';

@Component({
    selector: 'app-channel[channel]',
    templateUrl: './channel.component.html',
    styleUrls: ['./channel.component.scss']
})
export class ChannelComponent {
    constructor(private dataService: DataService) { }

    @ViewChild('modalDelete') modalDelete!: ModalComponent;
    @ViewChild('modalUpdate') modalUpdate!: ModalComponent;
    @ViewChild('modalVerify') modalVerify!: ModalComponent;

    @Input() channel!: Channel;
    @Output() deleted: EventEmitter<any> = new EventEmitter();

    code: string = '';

    ChannelType: typeof ChannelType = ChannelType;

    update(channel: Channel) {
        this.dataService.updateChannel(this.channel.id ?? '', channel)
            .subscribe({
                next: channel => {
                    this.channel = channel;
                    this.modalUpdate.close();
                },
                error: () => {
                    this.modalUpdate.error();
                }
            });
    }

    del() {
        this.dataService.deleteChannel(this.channel.id ?? '')
            .subscribe({
                next: () => {
                    this.deleted.emit();
                    this.modalDelete.close();
                },
                error: () => {
                    this.modalDelete.error();
                }
            });
    }

    verify(code: string) {
        this.dataService.verifyChannel(this.channel.id ?? '', code)
            .subscribe({
                next: channel => {
                    this.channel = channel;
                    this.modalVerify.close();
                },
                error: () => {
                    this.modalVerify.error();
                }
            });
    }
}
