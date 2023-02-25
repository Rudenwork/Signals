import { Component, OnInit } from '@angular/core';
import { SettingsService } from './services/settings.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {

  constructor(private settingsService: SettingsService) {

  }

  ngOnInit(): void {

    this.settingsService.getSettings().subscribe(settings => {
      this.apiBaseUrl = settings.apiBaseAddress;
    })
  }

  apiBaseUrl!: string;
}
