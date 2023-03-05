import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class DataService {

  constructor(private authService: AuthService, private http: HttpClient) { }

  private get<T>(endpoint: string): Observable<T> {

    let url = `${window.origin}/api/${endpoint}`;

    let headers = new HttpHeaders({
      'Authorization': `Bearer ${this.authService.getToken()}`
    });

    return this.http.get<T>(url, { headers: headers })
  }

  getChannels(): Observable<Channel[]> {
    return this.get<Channel[]>('channels');
  }
}

export class Channel {
  $type!: string;
  id!: string;
  userId!: string;
  description!: string;
  isVerified!: boolean;
}
