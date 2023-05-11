import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { IndicatorFormComponent } from '../indicator-form.component';
import { ConstantIndicator } from 'src/app/models/signal.model';
import { FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
    selector: 'app-constant-form-part[indicator]',
    templateUrl: './constant-form-part.component.html',
    styleUrls: ['./constant-form-part.component.scss']
})
export class ConstantFormPartComponent implements OnInit, OnDestroy {
    constructor(private indicatorForm: IndicatorFormComponent) { }

    @Input() indicator!: ConstantIndicator;
    
    value!: FormControl;

    form!: FormGroup;

    ngOnInit() {
        this.value = new FormControl(this.indicator.value, [
            Validators.required
        ]);

        this.value.valueChanges.subscribe(value => this.indicator.value = value);

        this.form = new FormGroup([
            this.value
        ]);
        
        this.indicatorForm.form.addControl('constant-form-part', this.form);
    }

    ngOnDestroy() {
        this.indicatorForm.form.removeControl('constant-form-part');
    }
}
