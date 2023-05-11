import { Component, HostBinding, Input } from '@angular/core';
import { SMAIndicator } from 'src/app/models/signal.model';

@Component({
    selector: 'app-sma-preview[indicator]',
    templateUrl: './sma-preview.component.html',
    styleUrls: ['./sma-preview.component.scss']
})
export class SmaPreviewComponent {
    @HostBinding('class.preview') isPreview: boolean = true;

    @Input() indicator!: SMAIndicator;
}
