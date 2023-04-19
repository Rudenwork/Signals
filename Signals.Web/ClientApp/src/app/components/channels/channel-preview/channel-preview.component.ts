import { Component, HostBinding, Input } from '@angular/core';
import { Channel } from 'src/app/models/channel.model';

@Component({
    selector: 'app-channel-preview[channel]',
    templateUrl: './channel-preview.component.html',
    styleUrls: ['./channel-preview.component.scss']
})
export class ChannelPreviewComponent {
    @HostBinding('class.preview') isPreview: boolean = true;

    @Input() channel!: Channel;
}
