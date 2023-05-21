import { Component, Input } from '@angular/core';
import { BBIndicator } from 'src/app/models/signal.model';
import { IndicatorFormHelperService } from 'src/app/services/indicator-form-helper.service';

@Component({
    selector: 'app-bb-preview[indicator]',
    templateUrl: './bb-preview.component.html',
    styleUrls: ['./bb-preview.component.scss']
})
export class BbPreviewComponent {
    constructor(public helper: IndicatorFormHelperService) { }

    @Input() indicator!: BBIndicator;
}
