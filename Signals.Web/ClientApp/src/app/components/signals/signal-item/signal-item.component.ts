import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Signal } from 'src/app/models/signal.model';

@Component({
    selector: 'app-signal-item',
    templateUrl: './signal-item.component.html',
    styleUrls: ['./signal-item.component.scss']
})
export class SignalItemComponent {
    @Output() changed: EventEmitter<any> = new EventEmitter();
    @Input() signal!: Signal;
}
