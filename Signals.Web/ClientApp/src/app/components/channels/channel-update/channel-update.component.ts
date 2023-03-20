import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Channel, ChannelType } from 'src/app/models/channel.model';
import { DataService } from 'src/app/services/data.service';

@Component({
    selector: 'app-channel-update[channel]',
    templateUrl: './channel-update.component.html',
    styleUrls: ['./channel-update.component.scss']
})
export class ChannelUpdateComponent implements OnInit {
    constructor(private dataService: DataService) { }

    @Input() channel!: Channel;
    @Output() updated: EventEmitter<any> = new EventEmitter();

    ChannelType: typeof ChannelType = ChannelType;
    isUpdating: boolean = false;

    description!: FormControl;
    form!: FormGroup;

    ngOnInit() {
        this.channel = { ...this.channel };

        this.description = new FormControl(this.channel.description, Validators.maxLength(100));
        this.description.valueChanges.subscribe(description => this.channel.description = description);

        this.form = new FormGroup([
            this.description
        ]);
    }

    castChannel<T>(): T {
        return this.channel as T;
    }

    setChildForm(form: FormGroup) {
        this.form.setControl('childForm', form);
    }

    update() {
        this.isUpdating = true;
        this.dataService.updateChannel(this.channel.id ?? '', this.channel)
            .subscribe(() => this.updated.emit());
    }
}
