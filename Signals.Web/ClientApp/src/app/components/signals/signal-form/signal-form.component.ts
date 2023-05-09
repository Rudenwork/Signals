import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Signal, Stage } from 'src/app/models/signal.model';
import { ModalComponent } from '../../modal/modal.component';
import { FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
    selector: 'app-signal-form',
    templateUrl: './signal-form.component.html',
    styleUrls: ['./signal-form.component.scss']
})
export class SignalFormComponent implements OnInit {
    constructor(private modal: ModalComponent) { }

    @Input() signal!: Signal;
    @Output() submitted: EventEmitter<Signal> = new EventEmitter();

    name!: FormControl;
    schedule!: FormControl;
    isDisabled!: FormControl;
    stages!: FormControl;

    form!: FormGroup;
    isCreating: boolean = false;

    ngOnInit() {
        if (this.signal == undefined) {
            this.signal = new Signal();
            this.signal.stages = [];
            this.isCreating = true;
        }
        else {
            this.signal = { ...this.signal };
            this.signal.stages = this.signal.stages?.slice();

            delete this.signal.id;
            delete this.signal.userId;
            delete this.signal.isDisabled;
            delete this.signal.execution;
        }

        this.name = new FormControl(this.signal.name, [
            Validators.required,
            Validators.maxLength(100)
        ]);

        this.schedule = new FormControl(this.signal.schedule ?? 'never', [
            Validators.required,
            Validators.maxLength(50)
        ]);

        this.stages = new FormControl(this.signal.stages, [
            Validators.required
        ]);

        this.isDisabled = new FormControl(this.signal.isDisabled);

        this.name.valueChanges.subscribe(name => this.signal.name = name);
        this.schedule.valueChanges.subscribe(schedule => this.signal.schedule = schedule);
        this.isDisabled.valueChanges.subscribe(isDisabled => this.signal.isDisabled = isDisabled);
        this.stages.valueChanges.subscribe(stages => this.signal.stages = stages);

        this.form = new FormGroup([
            this.name,
            this.schedule,
            this.stages
        ]);

        if (this.isCreating) {
            this.form.addControl('isDisabled', this.isDisabled);
        }

        this.modal.form.addControl('signal-form', this.form);
        this.modal.submitted.subscribe(() => this.submit());
    }

    createStage(stage: Stage) {
        this.stages.value.push(stage);
        this.stages.markAsDirty();
        this.stages.updateValueAndValidity();
    }

    updateStage(index: number, stage: Stage) {
        this.stages.value[index] = stage;
        this.stages.markAsDirty();
        this.stages.updateValueAndValidity();
    }

    deleteStage(index: number) {
        this.stages.value.splice(index, 1);
        this.stages.markAsDirty();
        this.stages.updateValueAndValidity();
    }

    submit() {
        if (this.form.pristine) {
            return;
        }

        if (this.name.pristine) {
            delete this.signal.name;
        }

        if (this.schedule.pristine) {
            delete this.signal.schedule;
        }

        if (this.isDisabled.pristine) {
            delete this.signal.isDisabled;
        }

        if (this.stages.pristine) {
            delete this.signal.stages;
        }

        this.submitted.emit(this.signal);
    }
}
