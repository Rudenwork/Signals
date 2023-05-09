import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Crons, Signal, Stage } from 'src/app/models/signal.model';
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

        this.schedule = new FormControl(this.convertCronToOption(this.signal.schedule), [
            Validators.required,
            Validators.maxLength(50)
        ]);

        this.stages = new FormControl(this.signal.stages, [
            Validators.required
        ]);

        this.isDisabled = new FormControl(this.signal.isDisabled);

        this.name.valueChanges.subscribe(name => this.signal.name = name);
        this.schedule.valueChanges.subscribe(schedule => this.signal.schedule = this.convertOptionToCron(schedule));
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

    private convertCronToOption(cron? : string) : string {
        if(cron == undefined) {
            return '';
        }
        else if(cron == Crons.Never) {
            return this.scheduleOptions['Never'];
        }
        else if(cron == Crons.EveryMinute) {
            return this.scheduleOptions['EveryMinute'];
        }
        else if(cron == Crons.EveryFiveMinutes) {
            return this.scheduleOptions['EveryFiveMinutes'];
        }
        else if(cron == Crons.EveryTenMinutes) {
            return this.scheduleOptions['EveryTenMinutes'];
        }
        else if(cron == Crons.EveryFifteenMinutes) {
            return this.scheduleOptions['EveryFifteenMinutes'];
        }
        else if(cron == Crons.EveryThirtyMinutes) {
            return this.scheduleOptions['EveryThirtyMinutes'];
        }
        else if(cron == Crons.EveryHour) {
            return this.scheduleOptions['EveryHour'];
        }
        else if(cron == Crons.EveryDay) {
            return this.scheduleOptions['EveryDay'];
        }
        else if(cron == Crons.EveryMonth) {
            return this.scheduleOptions['EveryMonth'];
        }

        return '';
    }

    private convertOptionToCron(option? : string) : Crons {
        if(option == this.scheduleOptions['Never']) {
            return Crons.Never;
        }
        else if(option == this.scheduleOptions['EveryMinute']) {
            return Crons.EveryMinute;
        }
        else if(option == this.scheduleOptions['EveryFiveMinutes']) {
            return Crons.EveryFiveMinutes;
        }
        else if(option == this.scheduleOptions['EveryTenMinutes']) {
            return Crons.EveryTenMinutes;
        }
        else if(option == this.scheduleOptions['EveryFifteenMinutes']) {
            return Crons.EveryFifteenMinutes;
        }
        else if(option == this.scheduleOptions['EveryThirtyMinutes']) {
            return Crons.EveryThirtyMinutes;
        }
        else if(option == this.scheduleOptions['EveryHour']) {
            return Crons.EveryHour;
        }
        else if(option == this.scheduleOptions['EveryDay']) {
            return Crons.EveryDay;
        }
        else if(option == this.scheduleOptions['EveryMonth']) {
            return Crons.EveryMonth;
        }
        
        return Crons.Never;
    }

    private scheduleOptions: Record<string, string> = {
        Never: 'Never',
        EveryMinute: 'Every Minute',
        EveryFiveMinutes: 'Every Five Minutes',
        EveryTenMinutes: 'Every Ten Minutes',
        EveryFifteenMinutes: 'Every Fifteen Minutes',
        EveryThirtyMinutes: 'Every Thirty Minutes',
        EveryHour: 'Every Hour',
        EveryDay: 'Every Day',
        EveryMonth: 'Every Month',
        EveryYear: 'Every Year'
    }

    getScheduleOptions() : string[] {
        return Object.values(this.scheduleOptions);
    }
}
