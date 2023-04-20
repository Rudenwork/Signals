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

export class Stage {
    name?: string;
    type$?: StageType;
}

export enum StageType {
    Waiting = 'Waiting',
    Condition = 'Condition',
    Notification = 'Notification'
}
