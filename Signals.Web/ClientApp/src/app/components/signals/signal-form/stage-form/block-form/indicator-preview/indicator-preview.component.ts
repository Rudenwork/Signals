import { Component, Input } from '@angular/core';
import { Indicator } from 'src/app/models/signal.model';

@Component({
    selector: 'app-indicator-preview[indicator]',
    templateUrl: './indicator-preview.component.html',
    styleUrls: ['./indicator-preview.component.scss']
})
export class IndicatorPreviewComponent {
    @Input() indicator!: Indicator;
}
