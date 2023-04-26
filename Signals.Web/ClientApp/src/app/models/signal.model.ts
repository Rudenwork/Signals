export class Signal {
    id?: string;
    userId?: string;
    name?: string;
    schedule?: string;
    stages?: Stage[];
    isDisabled?: boolean;
    execution?: Execution;
}

export class Execution {
    id?: string;
    stage?: Stage;
}

export enum TimeUnit {
    Second = 'Second',
    Minute = 'Minute',
    Hour = 'Hour',
    Day = 'Day',
    Week = 'Week',
    Month = 'Month',
    Year = 'Year'
}

export enum IntervalEnum
{
    OneSecond = 'OneSecond',
    OneMinute = 'OneMinute',
    ThreeMinutes = 'ThreeMinutes',
    FiveMinutes = 'FiveMinutes',
    FifteenMinutes = 'FifteenMinutes',
    ThirtyMinutes = 'ThirtyMinutes',
    OneHour = 'OneHour',
    TwoHour = 'TwoHour',
    FourHour = 'FourHour',
    SixHour = 'SixHour',
    EightHour = 'EightHour',
    TwelveHour = 'TwelveHour',
    OneDay = 'OneDay',
    ThreeDay = 'ThreeDay',
    OneWeek = 'OneWeek',
    OneMonth = 'OneMonth'
}

export enum StageType {
    Waiting = 'Waiting',
    Condition = 'Condition',
    Notification = 'Notification'
}

export class Stage {
    type$?: StageType;
}

export class WaitingStage extends Stage {
    unit?: TimeUnit
    amount?: number
}

export class NotificationStage extends Stage {
    channelId?: string
    text?: string
}

export class ConditionStage extends Stage { 
    retryCount?: number
    delayUnit?: TimeUnit
    delayAmount?: number
    block?: Block
}

export enum BlockType {
    Group = 'Group',
    Value = 'Value',
    Change = 'Change'
}

export class Block {
    type$?: BlockType;
}

export enum GroupBlockType {
    And = 'And',
    Or = 'Or'
}

export class GroupBlock extends Block {
    type?: GroupBlockType
    children?: Block[]
}

export enum OperatorEnum {
    GreaterOrEqual = 'GreaterOrEqual',
    LessOrEqual = 'LessOrEqual',
    Crossed = 'Crossed'
}

export class ValueBlock extends Block {
    leftIndicator?: Indicator
    rightIndicator?: Indicator
    operator?: OperatorEnum
}

export enum ChangeBlockType {
    Increase = 'Increase',
    Decrease = 'Decrease',
    Cross = 'Cross'
}

export class ChangeBlock extends Block {
    indicator?: Indicator
    type?: ChangeBlockType
    operator?: OperatorEnum
    target?: number
    isPercentage?: boolean
    unit?: TimeUnit
    amount?: number
}

export class Indicator {
    interval?: IntervalEnum
    loopbackPeriod?: number
    symbol?: string
}

export enum BBIndicatorBandType {
    Lower = 'Lower',
    Middle = 'Middle',
    Upper = 'Upper'
}

export class BBIndicator extends Indicator {
    bandType?: BBIndicatorBandType
}

export enum CandleIndicatorParameter {
    Open = 'Open',
    Close = 'Close',
    Low = 'Low',
    High = 'High',
    Average = 'Average',
    Volume = 'Volume'
}

export class CandleIndicator extends Indicator {
    parameterType?: CandleIndicatorParameter
}

export class ConstantIndicator extends Indicator {
    value?: number
}

export class EMAIndicator extends Indicator {

}

export class RSIIndicator extends Indicator {

}

export class SMAIndicator extends Indicator {

}
