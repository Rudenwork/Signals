import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { IndicatorFormComponent } from '../indicator-form.component';
import { BBIndicator } from 'src/app/models/signal.model';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { BbIndicatorFormHelperService } from 'src/app/services/indicator-form-helper.service';

@Component({
    selector: 'app-bb-form-part[indicator]',
    templateUrl: './bb-form-part.component.html',
    styleUrls: ['./bb-form-part.component.scss']
})
export class BbFormPartComponent implements OnInit, OnDestroy {
    constructor(public formHelper : BbIndicatorFormHelperService, private indicatorForm: IndicatorFormComponent) { }
    
    @Input() indicator!: BBIndicator;

    interval!: FormControl;
    loopbackPeriod!: FormControl;
    symbol!: FormControl;
    bandType!:FormControl;

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

        this.bandType = new FormControl(this.indicator.bandType ?? '', [
            Validators.required
        ]);

        this.interval.valueChanges.subscribe(interval => this.indicator.interval = this.formHelper.convertIntervalOptionToEnumValue(interval));
        this.loopbackPeriod.valueChanges.subscribe(loopbackPeriod => this.indicator.loopbackPeriod = loopbackPeriod);
        this.symbol.valueChanges.subscribe(symbol => this.indicator.symbol = symbol);
        this.bandType.valueChanges.subscribe(bandType => this.indicator.bandType = bandType);

        this.form = new FormGroup([
            this.interval,
            this.loopbackPeriod,
            this.symbol,
            this.bandType
        ]);
        
        this.indicatorForm.form.addControl('bb-form-part', this.form);
    }

    ngOnDestroy() {
        this.indicatorForm.form.removeControl('bb-form-part');
    }
}
