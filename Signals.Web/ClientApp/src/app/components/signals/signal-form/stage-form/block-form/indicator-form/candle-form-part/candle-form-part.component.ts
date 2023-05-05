import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { IndicatorFormComponent } from '../indicator-form.component';
import { CandleIndicator, CandleIndicatorParameter, IntervalEnum, SymbolEnum } from 'src/app/models/signal.model';
import { FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
    selector: 'app-candle-form-part[indicator]',
    templateUrl: './candle-form-part.component.html',
    styleUrls: ['./candle-form-part.component.scss']
})
export class CandleFormPartComponent implements OnInit, OnDestroy {
    constructor(private indicatorForm: IndicatorFormComponent) { }

    ParameterTypeEnum: typeof CandleIndicatorParameter = CandleIndicatorParameter;

    @Input() indicator!: CandleIndicator;

    interval!: FormControl;
    symbol!: FormControl;
    parameterType!: FormControl;

    form!: FormGroup;

    ngOnInit() {
        this.interval = new FormControl(this.indicator.interval || '', [
            Validators.required
        ]);

        this.parameterType = new FormControl(this.indicator.parameterType || '', [
            Validators.required
        ]);

        this.symbol = new FormControl(this.indicator.symbol || '', [
            Validators.required
        ]);

        this.interval.valueChanges.subscribe(interval => this.indicator.interval = this.convertIntervalOption(interval));
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

    getIntervalOptions(): string[] {
        let options = Object.values(this.options);
        return options;
    }

    getParameterOptions(): string[] {
        return Object.keys(CandleIndicatorParameter);
    }

    private options: Record<string, string> = {
        OneSecond: '1 Second',
        OneMinute: '1 Minute',
        ThreeMinutes: '3 Minutes',
        FiveMinutes: '5 Minutes',
        FifteenMinutes: '15 Minutes',
        ThirtyMinutes: '30 Minutes',
        OneHour: '1 Hour',
        TwoHour: '2 Hours',
        FourHour: '4 Hours',
        SixHour: '6 Hours',
        EightHour: '8 Hours',
        TwelveHour: '12 Hours',
        OneDay: '1 Day',
        ThreeDay: '3 Days',
        OneWeek: '1 Week',
        OneMonth: '1 Month',
    }

    private convertIntervalOption(option: String) : IntervalEnum {
        if(option == this.options[IntervalEnum.OneSecond]) {
            return IntervalEnum.OneSecond;
        }
        else if(option == this.options[IntervalEnum.OneMinute]) {
            return IntervalEnum.OneMinute;
        }
        else if(option == this.options[IntervalEnum.ThreeMinutes]) {
            return IntervalEnum.ThreeMinutes;
        }
        else if(option == this.options[IntervalEnum.FiveMinutes]) {
            return IntervalEnum.FiveMinutes;
        }
        else if(option == this.options[IntervalEnum.FifteenMinutes]) {
            return IntervalEnum.FifteenMinutes;
        }
        else if(option == this.options[IntervalEnum.ThirtyMinutes]) {
            return IntervalEnum.ThirtyMinutes;
        }
        else if(option == this.options[IntervalEnum.OneHour]) {
            return IntervalEnum.OneHour;
        }
        else if(option == this.options[IntervalEnum.TwoHour]) {
            return IntervalEnum.TwoHour;
        }
        else if(option == this.options[IntervalEnum.FourHour]) {
            return IntervalEnum.FourHour;
        }
        else if(option == this.options[IntervalEnum.SixHour]) {
            return IntervalEnum.SixHour;
        }
        else if(option == this.options[IntervalEnum.EightHour]) {
            return IntervalEnum.EightHour;
        }
        else if(option == this.options[IntervalEnum.TwelveHour]) {
            return IntervalEnum.TwelveHour;
        }
        else if(option == this.options[IntervalEnum.OneDay]) {
            return IntervalEnum.OneDay;
        }
        else if(option == this.options[IntervalEnum.ThreeDay]) {
            return IntervalEnum.ThreeDay;
        }
        else if(option == this.options[IntervalEnum.OneWeek]) {
            return IntervalEnum.OneWeek;
        }
        else if(option == this.options[IntervalEnum.OneMonth]) {
            return IntervalEnum.OneMonth;
        }

        return IntervalEnum.OneMinute;
    }

    getSymbolOptions(): string[] {
        return Object.keys(SymbolEnum);
    }
}
