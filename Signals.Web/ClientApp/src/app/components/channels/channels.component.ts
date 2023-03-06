import { Component, OnInit } from '@angular/core';
import { Channel, DataService } from 'src/app/services/data.service';

@Component({
  selector: 'app-channels',
  template: `
    <p>Channels</p>
    <ul>
        <li *ngFor="let channel of channels">
            [{{channel.$type}}] {{channel.id}}
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
