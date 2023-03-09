import { Component } from '@angular/core';

@Component({
  selector: 'app-modal',
  template: `
    <div id="modal-background" *ngIf="isOpened" (click)="close()">
      <div id="modal-window" (click)="onContentClick($event)">
        <div id="modal-close-container">
          <button id="modal-close" (click)="close()">x</button>
        </div>
        <div id="modal-content-container">
          <ng-content/>
        </div>
      </div>
    </div>
  `,
  styles: [`
    #modal-background {
      display: flex;
      align-items: center;
      justify-content: center;
      position: absolute;
      top: 0;
      left: 0;
      height:100%;
      width: 100%;
      backdrop-filter: blur(2px);
    }

    #modal-window {
      padding: 10px;
      background: white;
      border-radius: 10px;
      box-shadow: 0px 0px 20px 0px black;
    }

    #modal-close-container {
      display: flex;
      justify-content: right;
    }

    #modal-content-container {
      padding: 10px;
    }
  `]
})
export class ModalComponent {

  isOpened: boolean = false;

  open() {
    this.isOpened = true;
  }

  close() {
    this.isOpened = false;
  }

  onContentClick(event: Event) {
    event.stopPropagation();
  }
}
