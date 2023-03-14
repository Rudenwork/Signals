import { Component } from '@angular/core';

@Component({
    selector: 'app-modal',
    templateUrl: './modal.component.html',
    styleUrls: ['./modal.component.scss']
})
export class ModalComponent {

    isOpened: boolean = false;
    private resolve: any;

    open(): Promise<any> {
        this.isOpened = true;
        return new Promise<any>(resolve => this.resolve = resolve);
    }

    close() {
        this.isOpened = false;
        this.resolve();
    }

    onWindowClick(event: Event) {
        event.stopPropagation();
    }
}
