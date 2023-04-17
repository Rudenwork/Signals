import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Signal } from 'src/app/models/signal.model';

@Component({
    selector: 'app-signal[signal]',
    templateUrl: './signal.component.html',
    styleUrls: ['./signal.component.scss']
})
export class SignalComponent {
    @Output() changing: EventEmitter<any> = new EventEmitter();
    @Output() changed: EventEmitter<any> = new EventEmitter();
    @Input() signal!: Signal;
}
