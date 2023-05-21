import { Component, Input } from '@angular/core';
import { OperatorEnum, ValueBlock } from 'src/app/models/signal.model';

@Component({
    selector: 'app-value-preview[block]',
    templateUrl: './value-preview.component.html',
    styleUrls: ['./value-preview.component.scss']
})
export class ValuePreviewComponent {
    OperatorEnum: typeof OperatorEnum = OperatorEnum;

    @Input() block!: ValueBlock;
}
