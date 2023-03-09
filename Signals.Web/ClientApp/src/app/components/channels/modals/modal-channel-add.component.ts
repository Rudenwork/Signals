import { Component } from '@angular/core';
import { ModalComponent } from '../../modal/modal.component';

@Component({
  selector: 'app-modal-channel-add',
  template: `
    <p>
      modal-channel-add works!
    </p>
    <button (click)="close()">Internal Close</button>
  `,
  styles: [
  ]
})
export class ModalChannelAddComponent {
  constructor(private modal: ModalComponent) { }
  close() {
    this.modal.close();
  }
}
