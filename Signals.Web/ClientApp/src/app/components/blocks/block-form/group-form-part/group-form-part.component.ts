import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Block, GroupBlock, GroupBlockType } from 'src/app/models/signal.model';
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
    children!: FormControl;

    form!: FormGroup;

    ngOnInit() {
        this.block.children = this.block.children?.slice();

        this.type = new FormControl(this.block?.type ?? '', [
            Validators.required
        ]);

        this.children = new FormControl(this.block.children, [
            Validators.required
        ]);

        if(this.type.value != '') {
            this.type.markAsDirty();
        }

        this.type.valueChanges.subscribe(type => {
            this.block.type = type;
            this.type.markAsDirty();
        });

        this.children.valueChanges.subscribe(children => {
            this.block.children = children;
            this.type.markAsDirty();
        } );

        this.form = new FormGroup([
            this.type,
            this.children
        ]);

        this.blockForm.form.addControl('group-form-part', this.form);
    }

    ngOnDestroy() {
        this.blockForm.form.removeControl('group-form-part');
    }

    getTypeOptions(): string[] {
        return Object.keys(GroupBlockType);
    }

    createBlock(block: Block) {
        this.children.value.push(block);
        this.children.markAsDirty();
        this.children.updateValueAndValidity();
    }

    updateBlock(index: number, block: Block) {
        this.children.value[index] = block;
        this.children.markAsDirty();
        this.children.updateValueAndValidity();
    }

    deleteBlock(index: number) {
        this.children.value.splice(index, 1);
        this.children.markAsDirty();
        this.children.updateValueAndValidity();
    }
}
