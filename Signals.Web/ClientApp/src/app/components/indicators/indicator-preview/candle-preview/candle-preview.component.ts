import { Component, Input } from '@angular/core';
import { CandleIndicator } from 'src/app/models/signal.model';
import { IndicatorFormHelperService } from 'src/app/services/indicator-form-helper.service';

@Component({
    selector: 'app-candle-preview[indicator]',
    templateUrl: './candle-preview.component.html',
    styleUrls: ['./candle-preview.component.scss']
})
export class CandlePreviewComponent {
    constructor(public helper: IndicatorFormHelperService) { }

    @Input() indicator!: CandleIndicator;
}
