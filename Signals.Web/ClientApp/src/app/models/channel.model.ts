export class Channel {
    id?: string;
    userId?: string;
    type?: ChannelType;
    destination?: string;
    description?: string;
    isVerified?: boolean;
}

export enum ChannelType {
    Telegram = 'Telegram',
    Email = 'Email'
}