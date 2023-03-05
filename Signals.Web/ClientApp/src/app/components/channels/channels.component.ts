import { Component, OnInit } from '@angular/core';
import { DataService, Channel } from '../../services/data.service';

@Component({
  selector: 'app-channels',
  templateUrl: './channels.component.html',
  styleUrls: ['./channels.component.scss']
})
export class ChannelsComponent implements OnInit {
  constructor(private dataService: DataService) { }

  channels!: Channel[];

  ngOnInit() {

    this.dataService.getChannels()
      .subscribe(response => this.channels = response);
  }
}
