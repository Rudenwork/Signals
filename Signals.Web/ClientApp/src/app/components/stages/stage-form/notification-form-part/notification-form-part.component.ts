import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { NotificationStage } from 'src/app/models/signal.model';
import { StageFormComponent } from '../stage-form.component';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { DataService } from 'src/app/services/data.service';
import { Channel } from 'src/app/models/channel.model';

@Component({
    selector: 'app-notification-form-part[stage]',
    templateUrl: './notification-form-part.component.html',
    styleUrls: ['./notification-form-part.component.scss']
})
export class NotificationFormPartComponent implements OnInit, OnDestroy {
    constructor(private stageForm: StageFormComponent, private dataService: DataService) { }
    
    @Input() stage!: NotificationStage;

    channels: Channel[] = [];

    channelId!: FormControl;
    text!: FormControl;

    form!: FormGroup;

    ngOnInit() {
        this.dataService.getChannels()
            .subscribe(channels => {
                this.channels = channels.filter(x => x.isVerified);
            });

        this.channelId = new FormControl(this.stage.channelId ?? '', [
            Validators.required
        ]);
        
        this.text = new FormControl(this.stage.text, [
            Validators.required,
            Validators.minLength(1),
            Validators.maxLength(1000)
        ]);

        this.channelId.valueChanges.subscribe(channelId => this.stage.channelId = channelId);
        this.text.valueChanges.subscribe(text => this.stage.text = text);

        this.form = new FormGroup([
            this.channelId,
            this.text
        ]);

        this.stageForm.form.addControl('notification-form-part', this.form);
    }

    ngOnDestroy() {
        this.stageForm.form.removeControl('notification-form-part');
    }
}
