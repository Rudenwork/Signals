import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Channel, ChannelType } from 'src/app/models/channel.model';
import { DataService } from 'src/app/services/data.service';

@Component({
    selector: 'app-channel-verify',
    templateUrl: './channel-verify.component.html',
    styleUrls: ['./channel-verify.component.scss']
})
export class ChannelVerifyComponent {
    constructor(private dataService: DataService) { }

    @Output() verified: EventEmitter<any> = new EventEmitter();
    @Input() channel!: Channel;
    isVerifying: boolean = false;
    code: string = '';

    ChannelType: typeof ChannelType = ChannelType;

    castChannel<T>(): T {
        return this.channel as T;
    }

    verify() {
        this.isVerifying = true;
        this.dataService.verifyChannel(this.channel.id ?? '', this.code)
            .subscribe(() => this.verified.emit());
    }
}
