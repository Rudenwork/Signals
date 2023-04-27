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

    shouldRetry!: boolean;

    retryCount!: FormControl;
    retryDelayUnit!: FormControl;
    retryDelayLength!: FormControl;
    block!: FormControl;

    form!: FormGroup;

    ngOnInit() {

        this.shouldRetry = this.stage.retryCount != undefined;

        this.retryCount = new FormControl(this.stage.retryCount, [
            Validators.required,
            Validators.min(0),
            Validators.max(100)
        ]);

        this.retryDelayUnit = new FormControl(this.stage.retryDelayUnit, [
            Validators.required
        ]);

        this.retryDelayLength = new FormControl(this.stage.retryDelayLength, [
            Validators.required,
            Validators.min(0),
            Validators.max(1000)
        ]);

        this.block = new FormControl(this.stage.block, [
            Validators.required
        ]);

        this.retryCount.valueChanges.subscribe(retryCount => this.stage.retryCount = retryCount);
        this.retryDelayUnit.valueChanges.subscribe(retryDelayUnit => this.stage.retryDelayUnit = retryDelayUnit);
        this.retryDelayLength.valueChanges.subscribe(retryDelayLength => this.stage.retryDelayLength = retryDelayLength);
        this.block.valueChanges.subscribe(block => this.stage.block = block);
        
        this.form = new FormGroup([this.block]);
        
        this.shouldRetryChanged();

        this.stageForm.form.addControl('condition-part', this.form);
    }

    shouldRetryChanged() {
        if(this.shouldRetry) {
            this.form.addControl('retryCount', this.retryCount);
            this.form.addControl('retryDelayUnit', this.retryDelayUnit);
            this.form.addControl('retryDelayLength', this.retryDelayLength);
        }
        else {
            this.form.removeControl('retryCount');
            this.form.removeControl('retryDelayUnit');
            this.form.removeControl('retryDelayLength');
        }
    } 

    ngOnDestroy() {
        this.stageForm.form.removeControl('condition-part');
    }

    getUnitOptions(): string[] {
        return Object.keys(TimeUnit);
    }
}
