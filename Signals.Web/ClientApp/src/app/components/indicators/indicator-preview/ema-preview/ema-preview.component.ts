import { Component, HostBinding, Input } from '@angular/core';
import { EMAIndicator } from 'src/app/models/signal.model';

@Component({
    selector: 'app-ema-preview[indicator]',
    templateUrl: './ema-preview.component.html',
    styleUrls: ['./ema-preview.component.scss']
})
export class EmaPreviewComponent {
    @HostBinding('class.preview') isPreview: boolean = true;

    @Input() indicator!: EMAIndicator;
}
