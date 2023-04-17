import { Component, HostBinding, OnDestroy, OnInit } from '@angular/core';
import { Signal } from 'src/app/models/signal.model';
import { DataService } from 'src/app/services/data.service';

@Component({
    selector: 'app-signals',
    templateUrl: './signals.component.html',
    styleUrls: ['./signals.component.scss']
})
export class SignalsComponent implements OnInit, OnDestroy {
    constructor(private dataService: DataService) { }

    @HostBinding('class.page') isPage: boolean = true;
    @HostBinding('class.loading') isLoading: boolean = true;

    signals: Signal[] = [];
    
    private interval!: any;
    private fetchInterval: number = 1 * 60 * 1000;

    ngOnInit() {
        this.isLoading = false;
        this.startFetching();
    }

    ngOnDestroy() {
        this.stopFetching();
    }

    startFetching() {
        this.stopFetching();
        this.getSignals();
        this.interval = setInterval(() => this.getSignals(), this.fetchInterval);
    }

    stopFetching() {
        clearInterval(this.interval);
    }

    getSignals() {
        this.dataService.getSignals()
            .subscribe(signals => this.signals = signals);
    }
}
