export class Channel {
    constructor(type: ChannelType) { this.$type = type; }
    $type?: ChannelType;
    id?: string;
    userId?: string;
    description?: string;
    isVerified?: boolean;
}

export class TelegramChannel extends Channel {
    constructor() { super(ChannelType.Telegram); }

    username!: string;
}

export class EmailChannel extends Channel {
    constructor() { super(ChannelType.Email); }

    address!: string;
}

export enum ChannelType {
    Telegram = 'Telegram',
    Email = 'Email'
}