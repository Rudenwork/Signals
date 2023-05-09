import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { BBIndicator, CandleIndicator, ConstantIndicator, EMAIndicator, Indicator, IndicatorType, IntervalEnum, RSIIndicator, SMAIndicator } from 'src/app/models/signal.model';
import { ModalComponent } from 'src/app/components/modal/modal.component';
import { IndicatorFormHelperService } from 'src/app/services/indicator-form-helper.service';

@Component({
    selector: 'app-indicator-form',
    templateUrl: './indicator-form.component.html',
    styleUrls: ['./indicator-form.component.scss']
})
export class IndicatorFormComponent implements OnInit {
    constructor(public formHelper : IndicatorFormHelperService, private modal: ModalComponent) { }
    @Input() indicator!: Indicator;
    @Output() submitted: EventEmitter<Indicator> = new EventEmitter();

    type!: FormControl;
    form!: FormGroup;

    ngOnInit() {
        if (this.indicator != undefined) {            
            this.indicator = { ...this.indicator };
        }
        
        this.type = new FormControl(this.formHelper.convertIndicatorTypeEnumToOption(this.indicator?.$type), [
            Validators.required
        ]);

        this.type.valueChanges.subscribe(type => {
            let value = this.formHelper.convertIndicatorTypeOptionToEnumValue(type);
            this.changeIndicator(value)
        });

        this.form = new FormGroup([
            this.type
        ]);

        this.modal.form.addControl('indicator-form', this.form);
        this.modal.submitted.subscribe(() => this.submitted.emit(this.indicator));
    }

    getTypeOptions(): string[] {
        return Object.keys(IndicatorType);
    }

    castIndicator<T>(): T {
        return this.indicator as T;
    }

    changeIndicator(type: string) {
        if (type == IndicatorType.BollingerBands) {            
            this.indicator = new BBIndicator();
        }
        else if (type == IndicatorType.Candle) {            
            this.indicator = new CandleIndicator();
        }
        else if (type == IndicatorType.Constant) {            
            this.indicator = new ConstantIndicator();
        }
        else if (type == IndicatorType.ExponentialMovingAverage) {            
            this.indicator = new EMAIndicator();
        }
        else if (type == IndicatorType.RelativeStrengthIndex) {            
            this.indicator = new RSIIndicator();
        }
        else if (type == IndicatorType.SimpleMovingAverage) {            
            this.indicator = new SMAIndicator();
        }
    }
}
