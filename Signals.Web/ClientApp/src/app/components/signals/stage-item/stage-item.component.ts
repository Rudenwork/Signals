import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Stage } from 'src/app/models/signal.model';

@Component({
    selector: 'app-stage-item[stage]',
    templateUrl: './stage-item.component.html',
    styleUrls: ['./stage-item.component.scss']
})
export class StageItemComponent {
    @Output() updated: EventEmitter<Stage> = new EventEmitter();
    @Output() deleted: EventEmitter<any> = new EventEmitter();
    @Input() stage!: Stage;
}
