import { Component, OnInit, ViewChild } from '@angular/core';
import { Channel, DataService } from 'src/app/services/data.service';
import { ModalComponent } from '../modal/modal.component';

@Component({
  selector: 'app-channels',
  template: `
    <p>Channels <button (click)="add()">Add</button></p>
    <ul>
        <li *ngFor="let channel of channels">
            <app-channel-item [channel]="channel"/>
        </li>
    </ul>
    <app-modal #modalAdd>
      <app-modal-channel-add/>
    </app-modal>
  `
})
export class ChannelsComponent implements OnInit {
  constructor(private dataService: DataService) { }

  @ViewChild('modalAdd') modalAdd!: ModalComponent;
  channels!: Channel[];

  ngOnInit() {

    this.getChannels();
  }

  getChannels() {
    this.dataService.getChannels()
      .subscribe(response => this.channels = response);
  }

  add() {
    this.modalAdd.open()
      .then(() => this.getChannels());
  }
}
