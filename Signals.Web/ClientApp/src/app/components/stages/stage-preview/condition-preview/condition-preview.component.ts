import { Component, Input } from '@angular/core';
import { ConditionStage } from 'src/app/models/signal.model';

@Component({
    selector: 'app-condition-preview',
    templateUrl: './condition-preview.component.html',
    styleUrls: ['./condition-preview.component.scss']
})
export class ConditionPreviewComponent {
    @Input() stage!: ConditionStage;
}
