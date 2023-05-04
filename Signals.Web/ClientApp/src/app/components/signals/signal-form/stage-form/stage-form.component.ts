import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ModalComponent } from 'src/app/components/modal/modal.component';
import { ChangeBlock, ChangeBlockType, ConditionStage, NotificationStage, OperatorEnum, Stage, StageType, TimeUnit, WaitingStage } from 'src/app/models/signal.model';

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
            this.stage = this.getDefaultConditionStage();
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

        this.modal.form.addControl('stage-form', this.form);
        this.modal.submitted.subscribe(() => this.submitted.emit(this.stage));
    }

    private getDefaultConditionStage() : ConditionStage {
        let stage = new ConditionStage();
        stage.retryDelayUnit = TimeUnit.Minute;

        return stage;
    }

    private getDefaultNotificationStage() : NotificationStage {
        let notificationStage = new NotificationStage();
        return notificationStage;
    }

    private getDefaultWaitingStage() : WaitingStage {
        let waitingStage = new WaitingStage();
        waitingStage.unit = TimeUnit.Minute;

        return waitingStage
    }

    getTypeOptions(): string[] {
        return Object.keys(StageType);
    }

    castStage<T>(): T {
        return this.stage as T;
    }

    changeStage(type: string) {
        if (type == StageType.Condition) {            
            this.stage = this.getDefaultConditionStage();
        }
        else if (type == StageType.Notification) {
            this.stage = this.getDefaultNotificationStage();
        }
        else if (type == StageType.Waiting) {
            this.stage = this.getDefaultWaitingStage();
        }
    }
}
