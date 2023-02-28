import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';
import { SettingsService } from '../../services/settings.service';

@Component({
  selector: 'app-channels',
  templateUrl: './channels.component.html',
  styleUrls: ['./channels.component.scss']
})
export class ChannelsComponent implements OnInit {
  constructor(private oAuthService: OAuthService, private settingsService: SettingsService, private http: HttpClient) { }

  test!: string;

  ngOnInit(): void {
    this.settingsService.getSettings()
      .subscribe(settings => {

        const headers = new HttpHeaders({
          'Authorization': `Bearer ${this.oAuthService.getAccessToken()}`
        })

        this.http.get(`${settings.apiBaseAddress}/api/channels`, { headers: headers })
          .subscribe(response => this.test = JSON.stringify(response));
      });
  }
}
