import { Component, OnInit } from '@angular/core';
import { Channel, DataService } from 'src/app/services/data.service';

@Component({
  selector: 'app-channels',
  template: `
    <p>Channels</p>
    <ul>
        <li *ngFor="let channel of channels">
            <app-channel-item [channel]="channel"/>
        </li>
    </ul>
  `
})
export class ChannelsComponent implements OnInit {
  constructor(private dataService: DataService) { }

  channels!: Channel[];

  ngOnInit() {

    this.dataService.getChannels()
      .subscribe(response => this.channels = response);
  }
}
