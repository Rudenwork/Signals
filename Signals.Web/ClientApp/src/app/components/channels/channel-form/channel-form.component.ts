import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ModalComponent } from '../../modal/modal.component';
import { Channel, ChannelType } from 'src/app/models/channel.model';
import { FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
    selector: 'app-channel-form',
    templateUrl: './channel-form.component.html',
    styleUrls: ['./channel-form.component.scss']
})
export class ChannelFormComponent implements OnInit {
    constructor(private modal: ModalComponent) { }

    @Input() channel!: Channel;
    @Output() submitted: EventEmitter<Channel> = new EventEmitter();

    type!: FormControl;
    destination!: FormControl;
    description!: FormControl;

    form!: FormGroup;
    isCreating: boolean = false;

    ngOnInit() {
        if (this.channel == undefined) {
            this.channel = new Channel();
            this.channel.type = ChannelType.Telegram;
            this.isCreating = true;
        }
        else {
            this.channel = { ...this.channel };
            delete this.channel.id;
            delete this.channel.userId;
            delete this.channel.isVerified;
        }

        this.type = new FormControl(this.channel.type, [
            Validators.required
        ]);

        if(this.isCreating) {
            this.type.markAsDirty();
        }

        this.destination = new FormControl(this.channel.destination, [
            Validators.required
        ]);
        
        this.setDestinationValidator(this.channel.type ?? ChannelType.Telegram);

        this.description = new FormControl(this.channel.description, [
            Validators.maxLength(100)
        ]);

        this.type.valueChanges.subscribe(type => {
            this.channel.type = type;
            this.setDestinationValidator(type);
            this.destination.reset();
            this.destination.markAsTouched();
        });

        this.destination.valueChanges.subscribe(destination => this.channel.destination = destination);
        this.description.valueChanges.subscribe(description => this.channel.description = description);

        this.form = new FormGroup([
            this.type,
            this.destination,
            this.description
        ]);

        this.modal.form.addControl('user-form', this.form);
        this.modal.submitted.subscribe(() => this.submit());
    }

    submit() {
        if (this.form.pristine) {
            return;
        }

        if (this.type.pristine) {
            delete this.channel.type;
        }
            
        if (this.destination.pristine) {
            delete this.channel.destination;
        }

        if (this.description.pristine) {
            delete this.channel.description;
        }

        this.submitted.emit(this.channel);
    }

    getTypeOptions(): string[] {
        return Object.keys(ChannelType);
    }

    private setDestinationValidator(type: ChannelType) {
        if(type == ChannelType.Telegram) {
            this.destination.setValidators([
                Validators.required,
                Validators.pattern('^[a-zA-Z0-9]+([_ -]?[a-zA-Z0-9])*$')
            ])
        }
        else if (type == ChannelType.Email) {
            this.destination.setValidators([
                Validators.required,
                Validators.email
            ])
        }

        this.destination.updateValueAndValidity();
    }
}
