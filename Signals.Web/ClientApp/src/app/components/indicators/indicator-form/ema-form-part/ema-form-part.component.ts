import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { EMAIndicator } from 'src/app/models/signal.model';
import { IndicatorFormComponent } from '../indicator-form.component';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { EmaIndicatorFormHelperService } from 'src/app/services/indicator-form-helper.service';

@Component({
    selector: 'app-ema-form-part[indicator]',
    templateUrl: './ema-form-part.component.html',
    styleUrls: ['./ema-form-part.component.scss']
})
export class EmaFormPartComponent implements OnInit, OnDestroy {
    constructor(public formHelper : EmaIndicatorFormHelperService, private indicatorForm: IndicatorFormComponent) { }
    
    @Input() indicator!: EMAIndicator;

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
        
        this.indicatorForm.form.addControl('ema-form-part', this.form);
    }

    ngOnDestroy() {
        this.indicatorForm.form.removeControl('ema-form-part');
    }
}
