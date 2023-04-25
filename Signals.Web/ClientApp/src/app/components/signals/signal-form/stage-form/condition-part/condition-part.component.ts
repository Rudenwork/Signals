import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { ConditionStage, TimeUnit } from 'src/app/models/signal.model';
import { StageFormComponent } from '../stage-form.component';
import { FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
    selector: 'app-condition-part[stage]',
    templateUrl: './condition-part.component.html',
    styleUrls: ['./condition-part.component.scss']
})
export class ConditionPartComponent implements OnInit, OnDestroy {
    constructor(private stageForm: StageFormComponent) { }
    
    @Input() stage!: ConditionStage;

    retryCount!: FormControl;
    delayUnit!: FormControl;
    delayAmount!: FormControl;

    form!: FormGroup;

    ngOnInit(): void {
        this.retryCount = new FormControl(this.stage.retryCount, [
            Validators.required,
            Validators.min(0),
            Validators.max(100)
        ]);

        this.delayUnit = new FormControl(this.stage.delayUnit, [
            Validators.required
        ]);

        this.delayAmount = new FormControl(this.stage.delayAmount, [
            Validators.required
        ]);

        this.retryCount.valueChanges.subscribe(retryCount => this.stage.retryCount = retryCount);
        this.delayUnit.valueChanges.subscribe(delayUnit => this.stage.delayUnit = delayUnit);
        this.delayAmount.valueChanges.subscribe(delayAmount => this.stage.delayAmount = delayAmount);
        
        this.form = new FormGroup([
            this.retryCount,
            this.delayUnit,
            this.delayAmount
        ]);

        this.stageForm.form.addControl('condition-part', this.form);
    }

    ngOnDestroy(): void {
        this.stageForm.form.removeControl('condition-part');
    }

    getUnitOptions(): string[] {
        return Object.keys(TimeUnit);
    }
}
