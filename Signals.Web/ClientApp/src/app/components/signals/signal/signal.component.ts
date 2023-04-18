import { Component, EventEmitter, Input, Output, ViewChild } from '@angular/core';
import { Signal } from 'src/app/models/signal.model';
import { DataService } from 'src/app/services/data.service';
import { ModalComponent } from '../../modal/modal.component';

@Component({
    selector: 'app-signal[signal]',
    templateUrl: './signal.component.html',
    styleUrls: ['./signal.component.scss']
})
export class SignalComponent {
    constructor(private dataService: DataService) { }

    @ViewChild('modalDelete') modalDelete!: ModalComponent;
    @ViewChild('modalUpdate') modalUpdate!: ModalComponent;
    @ViewChild('modalEnable') modalEnable!: ModalComponent;
    @ViewChild('modalDisable') modalDisable!: ModalComponent;
    @ViewChild('modalStart') modalStart!: ModalComponent;
    @ViewChild('modalStop') modalStop!: ModalComponent;
    
    @Input() signal!: Signal;
    @Output() deleted: EventEmitter<any> = new EventEmitter();

    update(signal: Signal) {
        this.dataService.updateSignal(this.signal.id ?? '', signal)
            .subscribe({
                next: signal => {
                    this.signal = signal;
                    this.modalUpdate.close();
                },
                error: () => {
                    this.modalUpdate.error();
                }
            });
    }

    del() {
        this.dataService.deleteSignal(this.signal.id ?? '')
            .subscribe({
                next: () => {
                    this.deleted.emit();
                    this.modalDelete.close();
                },
                error: () => {
                    this.modalDelete.error();
                }
            });
    }

    enable() {
        this.dataService.enableSignal(this.signal.id ?? '')
            .subscribe({
                next: signal => {
                    this.signal = signal;
                    this.modalEnable.close();
                },
                error: () => {
                    this.modalEnable.error();
                }
            });
    }

    disable() {
        this.dataService.disableSignal(this.signal.id ?? '')
            .subscribe({
                next: signal => {
                    this.signal = signal;
                    this.modalDisable.close();
                },
                error: () => {
                    this.modalDisable.error();
                }
            });
    }

    start() {
        this.dataService.startSignal(this.signal.id ?? '')
            .subscribe({
                next: signal => {
                    this.signal = signal;
                    this.modalStart.close();
                },
                error: () => {
                    this.modalStart.error();
                }
            });
    }

    stop() {
        this.dataService.stopSignal(this.signal.id ?? '')
            .subscribe({
                next: signal => {
                    this.signal = signal;
                    this.modalStop.close();
                },
                error: () => {
                    this.modalStop.error();
                }
            });
    }
}
