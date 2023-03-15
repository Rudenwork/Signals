import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Channel } from '../models/channel.model';
import { AuthService } from './auth.service';

@Injectable({
    providedIn: 'root'
})
export class DataService {

    constructor(private authService: AuthService, private http: HttpClient) { }

    private getUrl(endpoint: string): string {
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
        return this.http.post<T>(this.getUrl(endpoint), item, { headers: this.getHeaders() });
    }

    private delete(endpoint: string): Observable<Object> {
        return this.http.delete(this.getUrl(endpoint), { headers: this.getHeaders() });
    }

    getChannels(): Observable<Channel[]> {
        return this.get<Channel[]>('channels');
    }

    createChannel(channel: Channel): Observable<Channel> {
        return this.post<Channel>('channels', channel);
    }

    deleteChannel(id?: string): Observable<Object> {
        return this.delete(`channels/${id}`);
    }
}
