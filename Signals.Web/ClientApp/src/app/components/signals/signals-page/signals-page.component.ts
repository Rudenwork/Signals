import { Component, HostBinding, OnInit, ViewChild } from '@angular/core';
import { Signal } from 'src/app/models/signal.model';
import { DataService } from 'src/app/services/data.service';
import { ModalComponent } from '../../modal/modal.component';

@Component({
    selector: 'app-signals',
    templateUrl: './signals-page.component.html',
    styleUrls: ['./signals-page.component.scss']
})
export class SignalsPageComponent implements OnInit {
    constructor(private dataService: DataService) { }

    @HostBinding('class.page') isPage: boolean = true;
    @HostBinding('class.loading') isLoading: boolean = true;

    @ViewChild('modalCreate') modalCreate!: ModalComponent;

    signals: Signal[] = [];

    ngOnInit() {
        this.dataService.getSignals()
            .subscribe(signals => {
                this.signals = signals;
                this.sort();
                this.isLoading = false;
            });
    }

    sort() {
        this.signals = this.signals.sort((a, b) => a.name!.localeCompare(b!.name ?? ''));
    }

    create(signal: Signal) {
        this.dataService.createSignal(signal)
            .subscribe({
                next: signal => {
                    this.signals.push(signal);
                    this.sort();
                    this.modalCreate.close();
                },
                error: () => {
                    this.modalCreate.error();
                }
            });
    }

    remove(index: number) {
        this.signals.splice(index, 1);
        this.sort();
    }
}
