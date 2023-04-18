import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Channel } from '../models/channel.model';
import { Signal } from '../models/signal.model';
import { User } from '../models/user.model';
import { AuthService } from './auth.service';

@Injectable({
    providedIn: 'root'
})
export class DataService {
    constructor(private authService: AuthService, private http: HttpClient) { }

    //#region Common

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

    private post<T>(endpoint: string, item?: T): Observable<T> {
        return this.http.post<T>(this.getUrl(endpoint), item, { headers: this.getHeaders() });
    }

    private patch<T>(endpoint: string, item: T): Observable<T> {
        return this.http.patch<T>(this.getUrl(endpoint), item, { headers: this.getHeaders() });
    }

    private delete(endpoint: string): Observable<Object> {
        return this.http.delete(this.getUrl(endpoint), { headers: this.getHeaders() });
    }

    //#endregion Common
    //#region Channel

    getChannels(): Observable<Channel[]> {
        return this.get<Channel[]>('channels');
    }

    createChannel(channel: Channel): Observable<Channel> {
        return this.post<Channel>('channels', channel);
    }

    verifyChannel(id: string, code: string): Observable<any> {
        return this.post(`channels/${id}/verify`, { code: code });
    }

    updateChannel(id: string, channel: Channel): Observable<Channel> {
        return this.patch<Channel>(`channels/${id}`, channel);
    }

    deleteChannel(id: string): Observable<any> {
        return this.delete(`channels/${id}`);
    }

    //#endregion Channel
    //#region Signal

    getSignals(): Observable<Signal[]> {
        return this.get<Signal[]>('signals');
    }

    getSignal(id: string): Observable<Signal> {
        return this.get<Signal>(`signals/${id}`);
    }

    createSignal(signal: Signal): Observable<Signal> {
        return this.post<Signal>('signals', signal);
    }

    updateSignal(id: string, signal: Signal): Observable<Signal> {
        return this.patch<Signal>(`signals/${id}`, signal);
    }

    deleteSignal(id: string): Observable<any> {
        return this.delete(`signals/${id}`);
    }

    enableSignal(id: string): Observable<Signal> {
        return this.post(`signals/${id}/enable`);
    }

    disableSignal(id: string): Observable<Signal> {
        return this.post(`signals/${id}/disable`);
    }

    startSignal(id: string): Observable<Signal> {
        return this.post(`signals/${id}/start`);
    }

    stopSignals(id: string): Observable<Signal> {
        return this.post(`signals/${id}/stop`);
    }

    //#endregion Signal
    //#region User

    getUsers(): Observable<User[]> {
        return this.get<User[]>('users');
    }

    createUser(user: User): Observable<User> {
        return this.post<User>('users', user);
    }

    updateUser(id: string, user: User): Observable<User> {
        return this.patch<User>(`users/${id}`, user);
    }

    deleteUser(id: string): Observable<any> {
        return this.delete(`users/${id}`);
    }

    enableUser(id: string): Observable<User> {
        return this.post(`users/${id}/enable`);
    }

    disableUser(id: string): Observable<User> {
        return this.post(`users/${id}/disable`);
    }

    //#endregion User
}
