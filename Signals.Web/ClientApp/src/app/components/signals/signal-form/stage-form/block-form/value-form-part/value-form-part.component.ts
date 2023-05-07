import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { BlockFormComponent } from '../block-form.component';
import { OperatorEnum, ValueBlock } from 'src/app/models/signal.model';
import { FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
    selector: 'app-value-form-part[block]',
    templateUrl: './value-form-part.component.html',
    styleUrls: ['./value-form-part.component.scss']
})
export class ValueFormPartComponent implements OnInit, OnDestroy {
    constructor(private blockForm: BlockFormComponent) { }

    OperatorEnum: typeof OperatorEnum = OperatorEnum;

    @Input() block!: ValueBlock;

    operator!: FormControl;
    leftIndicator!: FormControl;
    rightIndicator!: FormControl;

    form!: FormGroup;

    ngOnInit() {
        this.leftIndicator = new FormControl(this.block.leftIndicator, [
            Validators.required
        ]);

        this.rightIndicator = new FormControl(this.block.rightIndicator, [
            Validators.required
        ]);

        this.operator = new FormControl(this.block.operator || '', [
            Validators.required
        ]);

        if(this.operator.value != '') {
            this.operator.markAsDirty();
        }

        this.leftIndicator.valueChanges.subscribe(leftIndicator => {
            this.block.leftIndicator = leftIndicator
            this.leftIndicator.markAsDirty();

        });
        this.rightIndicator.valueChanges.subscribe(rightIndicator => {
            this.block.rightIndicator = rightIndicator
            this.rightIndicator.markAsDirty();
        });
        this.operator.valueChanges.subscribe(operator => this.block.operator = operator);

        this.form = new FormGroup([
            this.leftIndicator,
            this.rightIndicator,
            this.operator
        ]);

        this.blockForm.form.addControl('value-form-part', this.form);
    }

    ngOnDestroy() {
        this.blockForm.form.removeControl('value-form-part');
    }

    getOperatorOptions(): string[] {
        return Object.keys(OperatorEnum).filter(x => x != OperatorEnum.Crossed);
    }
}
