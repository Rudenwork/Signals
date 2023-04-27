import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { TimeUnit, WaitingStage } from 'src/app/models/signal.model';
import { StageFormComponent } from '../stage-form.component';

@Component({
    selector: 'app-waiting-part[stage]',
    templateUrl: './waiting-part.component.html',
    styleUrls: ['./waiting-part.component.scss']
})
export class WaitingPartComponent implements OnInit, OnDestroy {
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

        this.stageForm.form.addControl('waiting-part', this.form);
    }

    ngOnDestroy() {
        this.stageForm.form.removeControl('waiting-part');
    }

    getUnitOptions(): string[] {
        return Object.keys(TimeUnit);
    }
}
