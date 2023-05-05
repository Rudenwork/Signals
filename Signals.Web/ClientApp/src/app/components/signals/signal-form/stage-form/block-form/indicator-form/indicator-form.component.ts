import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { BBIndicator, BBIndicatorBandType, CandleIndicator, CandleIndicatorParameter, ConstantIndicator, EMAIndicator, Indicator, IndicatorType, IntervalEnum, RSIIndicator, SMAIndicator } from 'src/app/models/signal.model';
import { ModalComponent } from 'src/app/components/modal/modal.component';

@Component({
    selector: 'app-indicator-form',
    templateUrl: './indicator-form.component.html',
    styleUrls: ['./indicator-form.component.scss']
})
export class IndicatorFormComponent implements OnInit {
    constructor(private modal: ModalComponent) { }
    @Input() indicator!: Indicator;
    @Output() submitted: EventEmitter<Indicator> = new EventEmitter();

    IndicatorType: typeof IndicatorType = IndicatorType;

    type!: FormControl;
    form!: FormGroup;

    ngOnInit() {
        if (this.indicator == undefined) {            
            this.indicator = this.getDefaultConstantIndicator();
        }
        else {
            this.indicator = { ...this.indicator };
        }
        
        this.type = new FormControl(this.indicator.$type, [
            Validators.required
        ]);

        this.type.valueChanges.subscribe(type => this.changeIndicator(type));

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
        if (type == IndicatorType.BB) {            
            this.indicator = this.getDefaultBBIndicator();
        }
        else if (type == IndicatorType.Candle) {            
            this.indicator = this.getDefaultCandleIndicator();
        }
        else if (type == IndicatorType.Constant) {            
            this.indicator = this.getDefaultConstantIndicator();
        }
        else if (type == IndicatorType.EMA) {            
            this.indicator = this.getDefaultEMAIndicator();
        }
        else if (type == IndicatorType.RSI) {            
            this.indicator = this.getDefaultRSIIndicator();
        }
        else if (type == IndicatorType.SMA) {            
            this.indicator = this.getDefaultSMAIndicator();
        }
    }

    private getDefaultBBIndicator() : BBIndicator {
        let bbIndicator = new BBIndicator();
        bbIndicator.loopbackPeriod = 14;
        return bbIndicator;
    }

    private getDefaultCandleIndicator() : CandleIndicator {
        let candleIndicator = new CandleIndicator();
        candleIndicator.loopbackPeriod = 14;
        return candleIndicator;
    }

    private getDefaultConstantIndicator() : ConstantIndicator {
        let constantIndicator = new ConstantIndicator();
        return constantIndicator;
    }

    private getDefaultEMAIndicator() : EMAIndicator {
        let emaIndicator = new EMAIndicator();
        return emaIndicator;
    }

    private getDefaultRSIIndicator() : RSIIndicator {
        let rsiIndicator = new RSIIndicator();
        return rsiIndicator;
    }

    private getDefaultSMAIndicator() : SMAIndicator {
        let smaIndicator = new SMAIndicator();
        return smaIndicator;
    }
}
