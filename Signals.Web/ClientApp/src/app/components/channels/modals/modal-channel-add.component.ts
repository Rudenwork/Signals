import { Component } from '@angular/core';
import { ChannelType, TelegramChannel } from 'src/app/models/channel';
import { DataService } from 'src/app/services/data.service';
import { ModalComponent } from '../../modal/modal.component';

@Component({
  selector: 'app-modal-channel-add',
  template: `
    <p>
      modal-channel-add works!
    </p>
    <button (click)="test()">Test</button>
  `,
  styles: [
  ]
})
export class ModalChannelAddComponent {
  constructor(private modal: ModalComponent, private dataService: DataService) { }
  test() {
    let channel = new TelegramChannel();
    channel.$type = ChannelType.Telegram;
    channel.username = 'test';

    this.dataService.createChannel(channel)
      .subscribe(channel => this.modal.close());
  }
}
