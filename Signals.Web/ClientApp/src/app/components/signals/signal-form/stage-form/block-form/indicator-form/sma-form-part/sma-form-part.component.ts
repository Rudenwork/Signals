import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { SmaIndicatorFormHelperService } from 'src/app/services/indicator-form-helper.service';
import { IndicatorFormComponent } from '../indicator-form.component';
import { SMAIndicator } from 'src/app/models/signal.model';
import { FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
    selector: 'app-sma-form-part',
    templateUrl: './sma-form-part.component.html',
    styleUrls: ['./sma-form-part.component.scss']
})
export class SmaFormPartComponent implements OnInit, OnDestroy {
    constructor(public formHelper : SmaIndicatorFormHelperService, private indicatorForm: IndicatorFormComponent) { }
    
    @Input() indicator!: SMAIndicator;

    interval!: FormControl;
    loopbackPeriod!: FormControl;
    symbol!: FormControl;

    form!: FormGroup;

    ngOnInit() {
        this.interval = new FormControl(this.formHelper.convertIntervalEnumToOption(this.indicator.interval) ?? '', [
            Validators.required
        ]);

        this.loopbackPeriod = new FormControl(this.indicator.loopbackPeriod, [
            Validators.required
        ]);

        this.symbol = new FormControl(this.indicator.symbol ?? '', [
            Validators.required
        ]);

        this.interval.valueChanges.subscribe(interval => this.indicator.interval = this.formHelper.convertIntervalOptionToEnumValue(interval));
        this.loopbackPeriod.valueChanges.subscribe(loopbackPeriod => this.indicator.loopbackPeriod = loopbackPeriod);
        this.symbol.valueChanges.subscribe(symbol => this.indicator.symbol = symbol);

        this.form = new FormGroup([
            this.interval,
            this.loopbackPeriod,
            this.symbol
        ]);
        
        this.indicatorForm.form.addControl('sma-form-part', this.form);
    }

    ngOnDestroy() {
        this.indicatorForm.form.removeControl('sma-form-part');
    }
}
