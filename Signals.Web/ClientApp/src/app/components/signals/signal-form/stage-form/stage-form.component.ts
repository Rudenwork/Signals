import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ModalComponent } from 'src/app/components/modal/modal.component';
import { Stage, StageType } from 'src/app/models/signal.model';

@Component({
    selector: 'app-stage-form',
    templateUrl: './stage-form.component.html',
    styleUrls: ['./stage-form.component.scss']
})
export class StageFormComponent implements OnInit {
    constructor(private modal: ModalComponent) { }
    
    @Input() stage!: Stage;
    @Output() submitted: EventEmitter<Stage> = new EventEmitter();

    name!: FormControl;
    type!: FormControl;

    form!: FormGroup;
    isCreating: boolean = false;

    ngOnInit() {
        if (this.stage == undefined) {
            this.stage = new Stage();
            this.stage.type$ = StageType.Condition;
            this.isCreating = true;
        }
        else {
            this.stage = { ...this.stage };
        }

        this.name = new FormControl(this.stage.name, [
            Validators.required,
            Validators.maxLength(100)
        ]);

        this.type = new FormControl(this.stage.type$, [
            Validators.required
        ]);

        if(this.isCreating) {
            this.type.markAsDirty();
        }

        this.name.valueChanges.subscribe(name => this.stage.name = name);
        this.type.valueChanges.subscribe(type => this.stage.type$ = type);

        this.form = new FormGroup([
            this.name,
            this.type
        ]);

        this.modal.form.addControl('user-form', this.form);
        this.modal.submitted.subscribe(() => this.submitted.emit(this.stage));
    }

    getTypeOptions(): string[] {
        return Object.keys(StageType);
    }
}
