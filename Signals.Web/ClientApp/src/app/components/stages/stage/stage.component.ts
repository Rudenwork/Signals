import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Stage } from 'src/app/models/signal.model';

@Component({
    selector: 'app-stage[stage]',
    templateUrl: './stage.component.html',
    styleUrls: ['./stage.component.scss']
})
export class StageComponent {
    @Input() stage!: Stage;
    @Output() updated: EventEmitter<Stage> = new EventEmitter();
    @Output() deleted: EventEmitter<any> = new EventEmitter();
}
