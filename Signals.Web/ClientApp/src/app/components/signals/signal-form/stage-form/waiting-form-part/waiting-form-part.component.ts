import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { TimeUnit, WaitingStage } from 'src/app/models/signal.model';
import { StageFormComponent } from '../stage-form.component';

@Component({
    selector: 'app-waiting-form-part[stage]',
    templateUrl: './waiting-form-part.component.html',
    styleUrls: ['./waiting-form-part.component.scss']
})
export class WaitingFormPartComponent implements OnInit, OnDestroy {
    constructor(private stageForm: StageFormComponent) { }

    @Input() stage!: WaitingStage;

    unit!: FormControl;
    length!: FormControl;

    form!: FormGroup;

    ngOnInit() {
        this.unit = new FormControl(this.stage.unit, [
            Validators.required
        ]);

        this.length = new FormControl(this.stage.length, [
            Validators.required,
            Validators.min(0),
            Validators.max(1000)
        ]);

        this.unit.valueChanges.subscribe(unit => this.stage.unit = unit);
        this.length.valueChanges.subscribe(length => this.stage.length = length);

        this.form = new FormGroup([
            this.unit,
            this.length
        ]);

        this.stageForm.form.addControl('waiting-form-part', this.form);
    }

    ngOnDestroy() {
        this.stageForm.form.removeControl('waiting-form-part');
    }

    getUnitOptions(): string[] {
        return Object.keys(TimeUnit);
    }
}
