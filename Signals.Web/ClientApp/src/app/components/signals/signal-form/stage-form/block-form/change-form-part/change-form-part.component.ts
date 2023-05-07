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

    indicator!: FormControl;
    type!: FormControl;
    operator!: FormControl;
    target!: FormControl;
    isPercentage!: FormControl;
    periodUnit!: FormControl;
    periodLength!: FormControl;

    form!: FormGroup;

    ngOnInit() {
        this.indicator = new FormControl(this.block.indicator, [
            Validators.required
        ]);

        this.type = new FormControl(this.block.type || '', [
            Validators.required
        ]);

        this.operator = new FormControl(this.block.operator || '', [
            Validators.required
        ]);

        this.target = new FormControl(this.block.target, [
            Validators.required
        ]);

        this.isPercentage = new FormControl(this.block.isPercentage || false, []);

        this.periodUnit = new FormControl(this.block.periodUnit || '', [
            Validators.required
        ]);

        this.periodLength = new FormControl(this.block.periodLength, [
            Validators.required,
            Validators.min(0),
            Validators.max(1000)
        ]);

        if(this.type.value != '') {
            this.type.markAsDirty();
        }

        if(this.operator.value != '') {
            this.operator.markAsDirty();
        }

        if(this.periodUnit.value != '') {
            this.periodUnit.markAsDirty();
        }

        this.indicator.valueChanges.subscribe(indicator => {
            this.block.indicator = indicator
            this.indicator.markAsDirty();
        });
        this.operator.valueChanges.subscribe(operator => { 
            this.block.operator = operator; 
            this.operator.markAsDirty();
        });
        this.type.valueChanges.subscribe(type => {
            this.block.type = type;
            if(type == ChangeBlockType.Cross) {
                this.operator.setValue(OperatorEnum.Crossed);
            } else if (type != ChangeBlockType.Cross && this.block.operator == OperatorEnum.Crossed){
                this.operator.setValue(OperatorEnum.GreaterOrEqual);
            }
            this.type.markAsDirty();
        });
        this.target.valueChanges.subscribe(target => this.block.target = target);
        this.isPercentage.valueChanges.subscribe(isPercentage => {
            this.block.isPercentage = isPercentage
            this.isPercentage.markAsDirty();
        });
        this.periodUnit.valueChanges.subscribe(periodUnit => {
            this.block.periodUnit = periodUnit;
            this.periodUnit.markAsDirty();
        });
        this.periodLength.valueChanges.subscribe(periodLength => this.block.periodLength = periodLength);

        this.form = new FormGroup([
            this.indicator,
            this.type,
            this.operator,
            this.target,
            this.isPercentage,
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
        if(this.type.value == ChangeBlockType.Cross) {
            return [OperatorEnum.Crossed];
        }

        return Object.keys(OperatorEnum).filter(x => x != OperatorEnum.Crossed);
    }

    getUnitOptions(): string[] {
        return Object.keys(TimeUnit);
    }
}
