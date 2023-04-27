import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ModalComponent } from 'src/app/components/modal/modal.component';
import { ConditionStage, NotificationStage, Stage, StageType, TimeUnit, WaitingStage } from 'src/app/models/signal.model';

@Component({
    selector: 'app-stage-form',
    templateUrl: './stage-form.component.html',
    styleUrls: ['./stage-form.component.scss']
})
export class StageFormComponent implements OnInit {
    constructor(private modal: ModalComponent) { }
    
    @Input() stage!: Stage;
    @Output() submitted: EventEmitter<Stage> = new EventEmitter();
    
    StageType: typeof StageType = StageType;

    type!: FormControl;
    form!: FormGroup;

    isCreating: boolean = false;

    ngOnInit() {
        if (this.stage == undefined) {
            let stage = new ConditionStage();
            stage.retryDelayUnit = TimeUnit.Minute;
            
            this.stage = stage;
            this.isCreating = true;
        }
        else {
            this.stage = { ...this.stage };
        }

        this.type = new FormControl(this.stage.$type, [
            Validators.required
        ]);

        if(this.isCreating) {
            this.type.markAsDirty();
        }

        this.type.valueChanges.subscribe(type => this.changeStage(type));

        this.form = new FormGroup([
            this.type
        ]);

        this.modal.form.addControl('user-form', this.form);
        this.modal.submitted.subscribe(() => this.submitted.emit(this.stage));
    }

    getTypeOptions(): string[] {
        return Object.keys(StageType);
    }

    castStage<T>(): T {
        return this.stage as T;
    }

    changeStage(type: string) {
        if (type == StageType.Condition) {
            let conditionStage = new ConditionStage();
            conditionStage.retryDelayUnit = TimeUnit.Minute;
            
            this.stage = conditionStage;
        }
        else if (type == StageType.Notification) {
            let notificationStage = new NotificationStage();

            this.stage = notificationStage;
        }
        else if (type == StageType.Waiting) {
            let waitingStage = new WaitingStage();
            waitingStage.unit = TimeUnit.Minute;

            this.stage = waitingStage;
        }
    }
}
