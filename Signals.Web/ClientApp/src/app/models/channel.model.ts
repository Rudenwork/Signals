export class Channel {
    $type!: ChannelType;
    id!: string;
    userId!: string;
    description!: string;
    isVerified!: boolean;
}

export class TelegramChannel extends Channel {
    username!: string;
}

export class EmailChannel extends Channel {
    address!: string;
}

export enum ChannelType {
    Telegram = 'Telegram',
    Email = 'Email'
}