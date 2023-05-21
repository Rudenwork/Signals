import { Component, Input } from '@angular/core';
import { EMAIndicator } from 'src/app/models/signal.model';
import { IndicatorFormHelperService } from 'src/app/services/indicator-form-helper.service';

@Component({
    selector: 'app-ema-preview[indicator]',
    templateUrl: './ema-preview.component.html',
    styleUrls: ['./ema-preview.component.scss']
})
export class EmaPreviewComponent {
    constructor(public helper: IndicatorFormHelperService) { }

    @Input() indicator!: EMAIndicator;
}
