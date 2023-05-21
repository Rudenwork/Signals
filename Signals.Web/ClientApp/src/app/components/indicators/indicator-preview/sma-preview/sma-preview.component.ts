import { Component, Input } from '@angular/core';
import { SMAIndicator } from 'src/app/models/signal.model';
import { IndicatorFormHelperService } from 'src/app/services/indicator-form-helper.service';

@Component({
    selector: 'app-sma-preview[indicator]',
    templateUrl: './sma-preview.component.html',
    styleUrls: ['./sma-preview.component.scss']
})
export class SmaPreviewComponent {
    constructor(public helper: IndicatorFormHelperService) { }

    @Input() indicator!: SMAIndicator;
}
