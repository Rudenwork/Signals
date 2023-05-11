import { Component, HostBinding, Input } from '@angular/core';
import { CandleIndicator } from 'src/app/models/signal.model';

@Component({
    selector: 'app-candle-preview[indicator]',
    templateUrl: './candle-preview.component.html',
    styleUrls: ['./candle-preview.component.scss']
})
export class CandlePreviewComponent {
    @HostBinding('class.preview') isPreview: boolean = true;

    @Input() indicator!: CandleIndicator;
}
