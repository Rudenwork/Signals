import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Stage } from 'src/app/models/signal.model';

@Component({
    selector: 'app-stage',
    templateUrl: './stage.component.html',
    styleUrls: ['./stage.component.scss']
})
export class StageComponent implements OnInit {
    @Input() stage!: Stage;
    @Output() submitted: EventEmitter<Stage> = new EventEmitter();

    name!: FormControl;
    form!: FormGroup;

    ngOnInit() {
        if (this.stage == undefined) {
            this.stage = new Stage();
        }
        else {
            this.stage = { ...this.stage };
        }

        this.name = new FormControl(this.stage.name, [
            Validators.required,
            Validators.maxLength(100)
        ]);

        this.name.valueChanges.subscribe(name => this.stage.name = name);

        this.form = new FormGroup([
            this.name
        ]);
    }

    submit() {
        this.submitted.emit(this.stage);
    }
}
