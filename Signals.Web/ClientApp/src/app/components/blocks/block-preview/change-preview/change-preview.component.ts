import { Component, Input } from '@angular/core';
import { ChangeBlock, ChangeBlockType, OperatorEnum } from 'src/app/models/signal.model';

@Component({
    selector: 'app-change-preview[block]',
    templateUrl: './change-preview.component.html',
    styleUrls: ['./change-preview.component.scss']
})
export class ChangePreviewComponent {
    ChangeBlockType: typeof ChangeBlockType = ChangeBlockType;
    OperatorEnum: typeof OperatorEnum = OperatorEnum;

    @Input() block!: ChangeBlock;
}
