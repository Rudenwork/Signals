import { Component, HostBinding, Input } from '@angular/core';
import { Signal } from 'src/app/models/signal.model';

@Component({
    selector: 'app-signal-preview[signal]',
    templateUrl: './signal-preview.component.html',
    styleUrls: ['./signal-preview.component.scss']
})
export class SignalPreviewComponent {
    @HostBinding('class.preview') isPreview: boolean = true;

    @Input() signal!: Signal;
}
