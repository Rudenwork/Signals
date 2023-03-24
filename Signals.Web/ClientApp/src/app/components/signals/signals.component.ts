import { Component, OnInit } from '@angular/core';
import { Signal } from 'src/app/models/signal.model';
import { DataService } from 'src/app/services/data.service';

@Component({
    selector: 'app-signals',
    templateUrl: './signals.component.html',
    styleUrls: ['./signals.component.scss']
})
export class SignalsComponent implements OnInit {
    constructor(private dataService: DataService) { }

    signals!: Signal[];

    ngOnInit() {
        this.getSignals();
    }

    getSignals() {
        this.dataService.getSignals()
            .subscribe(signals => this.signals = signals);
    }
}
