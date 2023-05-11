import { Component, HostBinding, Input } from '@angular/core';
import { RSIIndicator } from 'src/app/models/signal.model';

@Component({
    selector: 'app-rsi-preview[indicator]',
    templateUrl: './rsi-preview.component.html',
    styleUrls: ['./rsi-preview.component.scss']
})
export class RsiPreviewComponent {
    @HostBinding('class.preview') isPreview: boolean = true;

    @Input() indicator!: RSIIndicator;
}
