import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Signal } from 'src/app/models/signal.model';
import { DataService } from 'src/app/services/data.service';

@Component({
    selector: 'app-signal',
    templateUrl: './signal.component.html',
    styleUrls: ['./signal.component.scss']
})
export class SignalComponent implements OnInit {
    constructor(private dataService: DataService) { }
    
    @Input() signal!: Signal;
    @Output() submitted: EventEmitter<any> = new EventEmitter();

    name!: FormControl;
    schedule!: FormControl;

    form!: FormGroup;

    ngOnInit() {
        if (this.signal == undefined) {
            this.signal = new Signal();
            this.signal.stages = [];
        }
        else {
            this.signal = { ...this.signal };
            this.signal.stages = this.signal.stages?.slice();
        }

        this.name = new FormControl(this.signal.name, [
            Validators.required,
            Validators.maxLength(100)
        ]);

        this.schedule = new FormControl(this.signal.schedule, [
            Validators.required,
            Validators.maxLength(50)
        ]);

        this.name.valueChanges.subscribe(name => this.signal.name = name);
        this.schedule.valueChanges.subscribe(schedule => this.signal.schedule = schedule);

        this.form = new FormGroup([
            this.name,
            this.schedule
        ]);
    }

    create() {
        this.submitted.emit();
    }

    update() {
        this.submitted.emit();
    }
}
