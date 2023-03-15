import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
    selector: 'app-modal',
    templateUrl: './modal.component.html',
    styleUrls: ['./modal.component.scss']
})
export class ModalComponent {
    @Output() closed: EventEmitter<any> = new EventEmitter();
    @Input() isOpened: boolean = false;

    open() {
        this.isOpened = true;
    }

    close() {
        this.isOpened = false;
        this.closed.emit();
    }
}
