import { Injectable } from "@angular/core";
import { BBIndicatorBandType, CandleIndicatorParameter, IndicatorType, IntervalEnum, SymbolEnum } from "../models/signal.model";

@Injectable({
    providedIn: 'root'
})
export class IndicatorFormHelperService {

    public IndicatorTypeEnum: typeof IndicatorType = IndicatorType;

    private indicatorTypeOptions: Record<string, string> = {
        BollingerBands: 'BB',
        Candle: 'Candle',
        Constant: 'Constant',
        ExponentialMovingAverage: 'EMA',
        RelativeStrengthIndex: 'RSI',
        SimpleMovingAverage: 'SMA',
    }

    private intervalOptions: Record<string, string> = {
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

    public getIntervalShort(interval?: IntervalEnum) {
        switch (interval) {
            case IntervalEnum.OneSecond: return '1s';
            case IntervalEnum.OneMinute: return '1m';
            case IntervalEnum.ThreeMinutes: return '3m';
            case IntervalEnum.FiveMinutes: return '5m';
            case IntervalEnum.FifteenMinutes: return '15m';
            case IntervalEnum.ThirtyMinutes: return '30m';
            case IntervalEnum.OneHour: return '1h';
            case IntervalEnum.TwoHour: return '2h';
            case IntervalEnum.FourHour: return '4h';
            case IntervalEnum.SixHour: return '6h';
            case IntervalEnum.EightHour: return '8h';
            case IntervalEnum.TwelveHour: return '12h';
            case IntervalEnum.OneDay: return '1D';
            case IntervalEnum.ThreeDay: return '3D';
            case IntervalEnum.OneWeek: return '1w';
            case IntervalEnum.OneMonth: return '1M';
            default: return ''
        }
    }

    public getIndicatorTypeOptions(): string[] {
        return Object.values(this.indicatorTypeOptions);
    }

    public getIntervalOptions(): string[] {
        return Object.values(this.intervalOptions);
    }

    public getSymbolOptions(): string[] {
        return Object.keys(SymbolEnum);
    }

    public convertIndicatorTypeOptionToEnumValue(option: String) : IndicatorType {
        if(option == this.indicatorTypeOptions[IndicatorType.BollingerBands]) {
            return IndicatorType.BollingerBands;
        }
        else if(option == this.indicatorTypeOptions[IndicatorType.Candle]) {
            return IndicatorType.Candle;
        }
        else if(option == this.indicatorTypeOptions[IndicatorType.Constant]) {
            return IndicatorType.Constant;
        }
        else if(option == this.indicatorTypeOptions[IndicatorType.ExponentialMovingAverage]) {
            return IndicatorType.ExponentialMovingAverage;
        }
        else if(option == this.indicatorTypeOptions[IndicatorType.RelativeStrengthIndex]) {
            return IndicatorType.RelativeStrengthIndex;
        }
        else if(option == this.indicatorTypeOptions[IndicatorType.SimpleMovingAverage]) {
            return IndicatorType.SimpleMovingAverage;
        }

        return IndicatorType.Constant;
    }

    public convertIndicatorTypeEnumToOption(value?: IndicatorType) : string {
        if(value == IndicatorType.BollingerBands) {
            return this.indicatorTypeOptions[IndicatorType.BollingerBands];
        }
        else if(value == IndicatorType.Candle) {
            return this.indicatorTypeOptions[IndicatorType.Candle];
        }
        else if(value == IndicatorType.Constant) {
            return this.indicatorTypeOptions[IndicatorType.Constant]
        }
        else if(value == IndicatorType.ExponentialMovingAverage) {
            return this.indicatorTypeOptions[IndicatorType.ExponentialMovingAverage]
        }
        else if(value == IndicatorType.RelativeStrengthIndex) {
            return this.indicatorTypeOptions[IndicatorType.RelativeStrengthIndex]
        }
        else if(value == IndicatorType.SimpleMovingAverage) {
            return this.indicatorTypeOptions[IndicatorType.SimpleMovingAverage]
        }

        return '';
    }

    public convertIntervalOptionToEnumValue(option?: string) : IntervalEnum {
        if(option == undefined) {
            return IntervalEnum.OneMinute;
        }
        else if(option == this.intervalOptions[IntervalEnum.OneSecond]) {
            return IntervalEnum.OneSecond;
        }
        else if(option == this.intervalOptions[IntervalEnum.OneMinute]) {
            return IntervalEnum.OneMinute;
        }
        else if(option == this.intervalOptions[IntervalEnum.ThreeMinutes]) {
            return IntervalEnum.ThreeMinutes;
        }
        else if(option == this.intervalOptions[IntervalEnum.FiveMinutes]) {
            return IntervalEnum.FiveMinutes;
        }
        else if(option == this.intervalOptions[IntervalEnum.FifteenMinutes]) {
            return IntervalEnum.FifteenMinutes;
        }
        else if(option == this.intervalOptions[IntervalEnum.ThirtyMinutes]) {
            return IntervalEnum.ThirtyMinutes;
        }
        else if(option == this.intervalOptions[IntervalEnum.OneHour]) {
            return IntervalEnum.OneHour;
        }
        else if(option == this.intervalOptions[IntervalEnum.TwoHour]) {
            return IntervalEnum.TwoHour;
        }
        else if(option == this.intervalOptions[IntervalEnum.FourHour]) {
            return IntervalEnum.FourHour;
        }
        else if(option == this.intervalOptions[IntervalEnum.SixHour]) {
            return IntervalEnum.SixHour;
        }
        else if(option == this.intervalOptions[IntervalEnum.EightHour]) {
            return IntervalEnum.EightHour;
        }
        else if(option == this.intervalOptions[IntervalEnum.TwelveHour]) {
            return IntervalEnum.TwelveHour;
        }
        else if(option == this.intervalOptions[IntervalEnum.OneDay]) {
            return IntervalEnum.OneDay;
        }
        else if(option == this.intervalOptions[IntervalEnum.ThreeDay]) {
            return IntervalEnum.ThreeDay;
        }
        else if(option == this.intervalOptions[IntervalEnum.OneWeek]) {
            return IntervalEnum.OneWeek;
        }
        else if(option == this.intervalOptions[IntervalEnum.OneMonth]) {
            return IntervalEnum.OneMonth;
        }

        return IntervalEnum.OneMinute;
    }

    public convertIntervalEnumToOption(value?: IntervalEnum) : string {
        if(value == undefined) {
            return '';
        }
        else if(value == IntervalEnum.OneSecond) {
            return this.intervalOptions[IntervalEnum.OneSecond];
        }
        else if(value == IntervalEnum.OneMinute) {
            return this.intervalOptions[IntervalEnum.OneMinute]
        }
        else if(value == IntervalEnum.ThreeMinutes) {
            return this.intervalOptions[IntervalEnum.ThreeMinutes]
        }
        else if(value == IntervalEnum.FiveMinutes) {
            return this.intervalOptions[IntervalEnum.FiveMinutes]
        }
        else if(value == IntervalEnum.FifteenMinutes) {
            return this.intervalOptions[IntervalEnum.FifteenMinutes]
        }
        else if(value == IntervalEnum.ThirtyMinutes) {
            return this.intervalOptions[IntervalEnum.ThirtyMinutes]
        }
        else if(value == IntervalEnum.OneHour) {
            return this.intervalOptions[IntervalEnum.OneHour]
        }
        else if(value == IntervalEnum.TwoHour) {
            return this.intervalOptions[IntervalEnum.TwoHour]
        }
        else if(value == IntervalEnum.FourHour) {
            return this.intervalOptions[IntervalEnum.FourHour]
        }
        else if(value == IntervalEnum.SixHour) {
            return this.intervalOptions[IntervalEnum.SixHour]
        }
        else if(value == IntervalEnum.EightHour) {
            return this.intervalOptions[IntervalEnum.EightHour]
        }
        else if(value == IntervalEnum.TwelveHour) {
            return this.intervalOptions[IntervalEnum.TwelveHour]
        }
        else if(value == IntervalEnum.OneDay) {
            return this.intervalOptions[IntervalEnum.OneDay]
        }
        else if(value == IntervalEnum.ThreeDay) {
            return this.intervalOptions[IntervalEnum.ThreeDay]
        }
        else if(value == IntervalEnum.OneWeek) {
            return this.intervalOptions[IntervalEnum.OneWeek]
        }
        else if(value == IntervalEnum.OneMonth) {
            return this.intervalOptions[IntervalEnum.OneMonth]
        }

        return '';
    }
}

@Injectable({
    providedIn: 'root'
})
export class CandleIndicatorFormHelperService extends IndicatorFormHelperService {
    
    public ParameterTypeEnum: typeof CandleIndicatorParameter = CandleIndicatorParameter;

    public getParameterTypeOptions(): string[] {
        return Object.keys(CandleIndicatorParameter);
    }
}

@Injectable({
    providedIn: 'root'
})
export class BbIndicatorFormHelperService extends IndicatorFormHelperService {
    
    public BandTypeEnum: typeof BBIndicatorBandType = BBIndicatorBandType;

    public getBandTypeOptions(): string[] {
        return Object.keys(BBIndicatorBandType);
    }
}

@Injectable({
    providedIn: 'root'
})
export class EmaIndicatorFormHelperService extends IndicatorFormHelperService {
    
}

@Injectable({
    providedIn: 'root'
})
export class RsiIndicatorFormHelperService extends IndicatorFormHelperService {
    
}

@Injectable({
    providedIn: 'root'
})
export class SmaIndicatorFormHelperService extends IndicatorFormHelperService {
    
}