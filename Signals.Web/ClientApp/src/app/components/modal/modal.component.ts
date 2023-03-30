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
    @Output() submitted: EventEmitter<any> = new EventEmitter();

    @Input() isOpened: boolean = false;

    @Input() showClose: boolean = true;
    @Input() title: string = "";

    @Input() showSubmit: boolean = true;
    @Input() submitText: string = 'Okay';
    @Input() submitColor: string = 'var(--color-neutral)';

    form!: FormGroup;
    private state!: string;

    ngOnInit() {
        this.form = new FormGroup([]);

        if (this.isOpened) {
            this.opened.emit();
        }

        window.addEventListener('popstate', () => {
            if (this.isOpened && history.state == this.state) {
                this.markClosed();
            }
        });
    }

    open() {
        this.state = (Math.random() + 1).toString(36).substring(7);

        history.replaceState(this.state, '', location.href);
        history.pushState(null, '', location.href);

        this.isOpened = true;
        this.opened.emit();
    }

    private markClosed() {
        this.isOpened = false;
        this.form = new FormGroup([]);
        this.closed.emit();
    }

    close() {
        history.back();
    }
}
