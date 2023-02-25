import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class SettingsService{

  constructor(private http: HttpClient) { }

  getSettings() {
    return this.http.get<Settings>('settings');
  }
}

export class Settings {
  apiBaseAddress!: string;
}
