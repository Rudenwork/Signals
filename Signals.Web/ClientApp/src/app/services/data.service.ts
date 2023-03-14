import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class DataService {
  
  constructor(private authService: AuthService, private http: HttpClient) { }

  getUrl(endpoint: string): string {
    return `${window.origin}/api/${endpoint}`;
  }

  private getHeaders(): any {
    return new HttpHeaders({
      'Authorization': `Bearer ${this.authService.getToken()}`
    });
  }

  private get<T>(endpoint: string): Observable<T> {
    return this.http.get<T>(this.getUrl(endpoint), { headers: this.getHeaders() });
  }

  private post<T>(endpoint: string, item: T): Observable<T> {
    return this.http.post<T>(this.getUrl(endpoint), item, { headers: this.getHeaders() })
  }

  getChannels(): Observable<Channel[]> {
    return this.get<Channel[]>('channels');
  }

  createChannel(channel: Channel): Observable<Channel> {
    return this.post<Channel>('channels', channel);
  }
}

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
