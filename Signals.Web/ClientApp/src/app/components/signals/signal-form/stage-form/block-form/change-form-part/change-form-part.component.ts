import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { BlockFormComponent } from '../block-form.component';
import { ChangeBlock, ChangeBlockType, OperatorEnum, TimeUnit } from 'src/app/models/signal.model';
import { FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
    selector: 'app-change-form-part[block]',
    templateUrl: './change-form-part.component.html',
    styleUrls: ['./change-form-part.component.scss']
})
export class ChangeFormPartComponent implements OnInit, OnDestroy {
    constructor(private blockForm: BlockFormComponent) { }

    ChangeBlockType: typeof ChangeBlockType = ChangeBlockType;
    OperatorEnum: typeof OperatorEnum = OperatorEnum;
    
    @Input() block!: ChangeBlock;

    type!: FormControl;
    operator!: FormControl;
    target!: FormControl;
    periodUnit!: FormControl;
    periodLength!: FormControl;

    form!: FormGroup;

    ngOnInit() {
        this.type = new FormControl(this.block.type, [
            Validators.required
        ]);

        this.operator = new FormControl(this.block.operator, [
            Validators.required
        ]);

        this.target = new FormControl(this.block.target, [
            Validators.required
        ]);

        this.periodUnit = new FormControl(this.block.periodUnit, [
            Validators.required
        ]);

        this.periodLength = new FormControl(this.block.periodLength, [
            Validators.required,
            Validators.min(0),
            Validators.max(1000)
        ]);

        this.type.valueChanges.subscribe(type => this.block.type = type);
        this.operator.valueChanges.subscribe(operator => this.block.operator = operator);
        this.target.valueChanges.subscribe(target => this.block.target = target);
        this.periodUnit.valueChanges.subscribe(periodUnit => this.block.periodUnit = periodUnit);
        this.periodLength.valueChanges.subscribe(periodLength => this.block.periodLength = periodLength);

        this.form = new FormGroup([
            this.type,
            this.operator,
            this.target,
            this.periodUnit,
            this.periodLength
        ]);
        
        this.blockForm.form.addControl('change-form-part', this.form);
    }

    ngOnDestroy() {
        this.blockForm.form.removeControl('change-form-part');
    }

    getTypeOptions(): string[] {
        return Object.keys(ChangeBlockType);
    }

    getOperatorOptions(): string[] {
        return Object.keys(OperatorEnum);
    }

    getUnitOptions(): string[] {
        return Object.keys(TimeUnit);
    }
}
