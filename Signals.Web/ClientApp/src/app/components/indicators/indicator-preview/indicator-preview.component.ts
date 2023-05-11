import { Component, HostBinding, Input } from '@angular/core';
import { Indicator, IndicatorType } from 'src/app/models/signal.model';

@Component({
    selector: 'app-indicator-preview[indicator]',
    templateUrl: './indicator-preview.component.html',
    styleUrls: ['./indicator-preview.component.scss']
})
export class IndicatorPreviewComponent {
    @HostBinding('class.preview') isPreview: boolean = true;

    @Input() indicator!: Indicator;

    IndicatorType: typeof IndicatorType = IndicatorType;

    castIndicator<T>(): T {
        return this.indicator as T;
    }
}
