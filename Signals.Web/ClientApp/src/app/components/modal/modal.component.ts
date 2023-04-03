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
    @Input() closeOnSubmit: boolean = false;

    form!: FormGroup;
    private state!: string;
    isSubmitted: boolean = false;
    isError: boolean = false;

    ngOnInit() {
        this.form = new FormGroup([]);

        if (this.isOpened) {
            this.isOpened = false;
            this.open();
        }

        window.addEventListener('popstate', () => this.onBack());
    }

    private createHistory() {
        this.state = (Math.random() + 1).toString(36).substring(7);
        history.replaceState(this.state, '', location.href);
        history.pushState(null, '', location.href);
    }

    error() {
        this.isError = true;
    }

    submit() {
        if (!this.isSubmitted) {
            this.isSubmitted = true;
            this.submitted.emit();
            
            if (this.closeOnSubmit) {
                this.close();
            }
        }
    }

    open() {
        if (!this.isOpened) {
            this.createHistory();
            this.isOpened = true;
            this.opened.emit();
        }
    }

    close() {
        if (this.isOpened) {
            history.back();
        }
    }

    private onBack() {
        if (this.isOpened && history.state == this.state) {
            this.isOpened = false;
            this.isSubmitted = false;
            this.isError = false;
            this.form = new FormGroup([]);
            this.closed.emit();
        }
    }
}
