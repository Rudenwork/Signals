import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { GroupBlock, GroupBlockType } from 'src/app/models/signal.model';
import { BlockFormComponent } from '../block-form.component';

@Component({
    selector: 'app-group-form-part[block]',
    templateUrl: './group-form-part.component.html',
    styleUrls: ['./group-form-part.component.scss']
})
export class GroupFormPartComponent implements OnInit, OnDestroy {
    constructor(private blockForm: BlockFormComponent) { }

    GroupBlockType: typeof GroupBlockType = GroupBlockType;

    @Input() block!: GroupBlock;

    type!: FormControl;

    form!: FormGroup;

    ngOnInit() {
        this.type = new FormControl(this.block.type, [
            Validators.required
        ]);

        this.type.valueChanges.subscribe(type => this.block.type = type);

        this.form = new FormGroup([
            this.type
        ]);

        this.blockForm.form.addControl('group-form-part', this.form);
    }

    ngOnDestroy() {
        this.blockForm.form.removeControl('group-form-part');
    }

    getTypeOptions(): string[] {
        return Object.keys(GroupBlockType);
    }
}
