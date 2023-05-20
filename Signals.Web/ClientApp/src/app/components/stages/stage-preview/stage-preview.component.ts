import { Component, Input } from '@angular/core';
import { Stage, StageType } from 'src/app/models/signal.model';

@Component({
    selector: 'app-stage-preview[stage]',
    templateUrl: './stage-preview.component.html',
    styleUrls: ['./stage-preview.component.scss']
})
export class StagePreviewComponent {

    @Input() stage!: Stage;

    StageType: typeof StageType = StageType;

    castBlock<T>(): T {
        return this.stage as T;
    }
}
