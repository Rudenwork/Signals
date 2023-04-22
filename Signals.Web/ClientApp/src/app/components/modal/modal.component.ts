import { Component, EventEmitter, HostBinding, Input, OnInit, Output } from '@angular/core';
import { FormGroup } from '@angular/forms';

@Component({
    selector: 'app-modal',
    templateUrl: './modal.component.html',
    styleUrls: ['./modal.component.scss']
})
export class ModalComponent implements OnInit {
    constructor() {
        ModalComponent.init();
    }

    public static init() {
        if (this.isInitiated) {
            return;
        }

        window.addEventListener('keyup', event => {
            if (this.modals.length == 0) {
                return;
            }

            if (event.key == 'Enter') {
                this.modals.at(-1)?.submit();
            }

            if (event.key == 'Escape') {
                this.modals.at(-1)?.close();
            }
        });

        window.addEventListener('popstate', event => {
            if (this.modals.length == 0) {
                return;
            }

            this.modals.at(-1)?.onPopstate();
        });

        this.isInitiated = true;
    }

    public static isInitiated: boolean = false;
    public static modals: ModalComponent[] = [];

    @HostBinding('class.opened')
    @Input() isOpened: boolean = false;

    @Output() opened: EventEmitter<any> = new EventEmitter();
    @Output() closed: EventEmitter<any> = new EventEmitter();
    @Output() submitted: EventEmitter<any> = new EventEmitter();

    @Input() isWide: boolean = false;
    @Input() showClose: boolean = true;
    @Input() title: string = "";

    @Input() showSubmit: boolean = true;
    @Input() submitText: string = 'Okay';
    @Input() submitClass: string = '';
    @Input() closeOnSubmit: boolean = false;

    form!: FormGroup;
    isSubmitted: boolean = false;
    isError: boolean = false;

    ngOnInit() {
        this.form = new FormGroup([]);

        if (this.isOpened) {
            this.isOpened = false;
            this.open();
        }
    }

    error() {
        this.isError = true;
    }

    isSubmitDisabled(): boolean {
        return (Object.keys(this.form.controls).length > 0 && this.form.pristine) || this.form.invalid || this.isSubmitted;
    }

    submit() {
        if (!this.isSubmitted && this.form.valid) {
            this.isSubmitted = true;
            this.submitted.emit();

            if (this.closeOnSubmit) {
                this.close();
            }
        }
    }

    open() {
        if (this.isOpened) {
            return;
        }

        ModalComponent.modals.push(this);
        history.pushState(null, '', location.href);

        this.isOpened = true;
        this.opened.emit();
    }

    close() {
        if (this.isOpened && ModalComponent.modals.at(-1) == this) {
            history.back();
        }
    }

    onPopstate() {
        ModalComponent.modals.pop();

        this.isOpened = false;
        this.isSubmitted = false;
        this.isError = false;
        this.form = new FormGroup([]);

        this.closed.emit();
    }
}
