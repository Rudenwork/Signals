import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { IndicatorFormComponent } from '../indicator-form.component';
import { CandleIndicator, CandleIndicatorParameter} from 'src/app/models/signal.model';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { CandleIndicatorFormHelperService } from 'src/app/services/indicator-form-helper.service';

@Component({
    selector: 'app-candle-form-part[indicator]',
    templateUrl: './candle-form-part.component.html',
    styleUrls: ['./candle-form-part.component.scss']
})
export class CandleFormPartComponent implements OnInit, OnDestroy {
    constructor(public formHelper : CandleIndicatorFormHelperService, private indicatorForm: IndicatorFormComponent) { }

    @Input() indicator!: CandleIndicator;

    interval!: FormControl;
    symbol!: FormControl;
    parameterType!: FormControl;

    form!: FormGroup;

    ngOnInit() {
        this.interval = new FormControl(this.formHelper.convertIntervalEnumToOption(this.indicator.interval) || '', [
            Validators.required
        ]);

        this.parameterType = new FormControl(this.indicator.parameterType || '', [
            Validators.required
        ]);

        this.symbol = new FormControl(this.indicator.symbol || '', [
            Validators.required
        ]);

        this.interval.valueChanges.subscribe(interval => {
            this.indicator.interval = this.formHelper.convertIntervalOptionToEnumValue(interval);
        });
        this.parameterType.valueChanges.subscribe(parameterType => this.indicator.parameterType = parameterType);
        this.symbol.valueChanges.subscribe(symbol => this.indicator.symbol = symbol);

        this.form = new FormGroup([
            this.interval,
            this.symbol,
            this.parameterType
        ]);
        
        this.indicatorForm.form.addControl('candle-form-part', this.form);
    }

    ngOnDestroy() {
        this.indicatorForm.form.removeControl('candle-form-part');
    }
}