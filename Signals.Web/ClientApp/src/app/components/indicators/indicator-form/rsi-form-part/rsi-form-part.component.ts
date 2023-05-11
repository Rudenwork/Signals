import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { RSIIndicator } from 'src/app/models/signal.model';
import { RsiIndicatorFormHelperService } from 'src/app/services/indicator-form-helper.service';
import { IndicatorFormComponent } from '../indicator-form.component';

@Component({
    selector: 'app-rsi-form-part[indicator]',
    templateUrl: './rsi-form-part.component.html',
    styleUrls: ['./rsi-form-part.component.scss']
})
export class RsiFormPartComponent implements OnInit, OnDestroy {
    constructor(public formHelper : RsiIndicatorFormHelperService, private indicatorForm: IndicatorFormComponent) { }
    
    @Input() indicator!: RSIIndicator;

    interval!: FormControl;
    loopbackPeriod!: FormControl;
    symbol!: FormControl;

    form!: FormGroup;

    ngOnInit() {
        this.interval = new FormControl(this.formHelper.convertIntervalEnumToOption(this.indicator.interval), [
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
        
        this.indicatorForm.form.addControl('rsi-form-part', this.form);
    }

    ngOnDestroy() {
        this.indicatorForm.form.removeControl('rsi-form-part');
    }
}
