import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormGroup } from '@angular/forms';

@Component({
    selector: 'app-modal',
    templateUrl: './modal.component.html',
    styleUrls: ['./modal.component.scss']
})
export class ModalComponent implements OnInit {
    @Output() opened: EventEmitter<any> = new EventEmitter();
    @Output() closed: EventEmitter<any> = new EventEmitter();
    @Output() confirmed: EventEmitter<any> = new EventEmitter();
    @Input() isOpened: boolean = false;
    @Input() showConfirm: boolean = false;
    @Input() confirmText: string = 'OK';
    @Input() title: string = "";

    form!: FormGroup;

    ngOnInit() {
        this.form = new FormGroup([]);

        if (this.isOpened) {
            this.opened.emit();
        }
    }

    open() {
        this.isOpened = true;
        this.opened.emit();
    }

    close() {
        this.isOpened = false;
        this.closed.emit();
    }
}
