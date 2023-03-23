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
    id?: string;
    name?: string;
    type$?: string;
}
