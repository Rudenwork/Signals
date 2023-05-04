import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ModalComponent } from 'src/app/components/modal/modal.component';
import { Block, BlockType, ChangeBlock, ChangeBlockType, ConstantIndicator, GroupBlock, GroupBlockType, OperatorEnum, TimeUnit, ValueBlock } from 'src/app/models/signal.model';

@Component({
    selector: 'app-block-form',
    templateUrl: './block-form.component.html',
    styleUrls: ['./block-form.component.scss']
})
export class BlockFormComponent implements OnInit {
    constructor(private modal: ModalComponent) { }

    @Input() block!: Block;
    @Output() submitted: EventEmitter<Block> = new EventEmitter();

    BlockType: typeof BlockType = BlockType;

    type!: FormControl;
    form!: FormGroup;

    ngOnInit() {
        if (this.block == undefined) {            
            this.block = this.getDefaultValueBlock();
        }
        else {
            this.block = { ...this.block };
        }
        
        this.type = new FormControl(this.block.$type, [
            Validators.required
        ]);

        this.type.valueChanges.subscribe(type => this.changeBlock(type));

        this.form = new FormGroup([
            this.type
        ]);

        this.modal.form.addControl('block-form', this.form);
        this.modal.submitted.subscribe(() => this.submitted.emit(this.block));
    }

    private getDefaultChangeBlock() : ChangeBlock {
        let changeBlock = new ChangeBlock();
        changeBlock.type = ChangeBlockType.Increase;
        changeBlock.operator = OperatorEnum.GreaterOrEqual;
        changeBlock.periodUnit = TimeUnit.Minute; 
        
        return changeBlock;
    }

    private getDefaultValueBlock() : ValueBlock {
        let valueBlock = new ValueBlock();
        valueBlock.operator = OperatorEnum.GreaterOrEqual;

        return valueBlock;
    }

    private getDefaultGroup() : GroupBlock {
        let groupBlock = new GroupBlock();
        groupBlock.type = GroupBlockType.And;
        groupBlock.children = [];

        return groupBlock
    }

    getTypeOptions(): string[] {
        return Object.keys(BlockType);
    }

    castBlock<T>(): T {
        return this.block as T;
    }

    changeBlock(type: string) {
        if (type == BlockType.Value) {            
            this.block = this.getDefaultValueBlock();
        }
        else if (type == BlockType.Change) {
            this.block = this.getDefaultChangeBlock();
        }
        else if (type == BlockType.Group) {
            this.block = this.getDefaultGroup();
        }
    }
}
