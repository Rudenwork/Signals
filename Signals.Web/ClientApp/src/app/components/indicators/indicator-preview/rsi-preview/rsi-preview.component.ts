import { Component, Input } from '@angular/core';
import { RSIIndicator } from 'src/app/models/signal.model';
import { IndicatorFormHelperService } from 'src/app/services/indicator-form-helper.service';

@Component({
    selector: 'app-rsi-preview[indicator]',
    templateUrl: './rsi-preview.component.html',
    styleUrls: ['./rsi-preview.component.scss']
})
export class RsiPreviewComponent {
    constructor(public helper: IndicatorFormHelperService) { }

    @Input() indicator!: RSIIndicator;
}
