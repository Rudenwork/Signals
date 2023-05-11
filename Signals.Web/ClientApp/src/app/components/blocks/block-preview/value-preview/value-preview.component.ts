import { Component, HostBinding, Input } from '@angular/core';
import { OperatorEnum, ValueBlock } from 'src/app/models/signal.model';

@Component({
    selector: 'app-value-preview[block]',
    templateUrl: './value-preview.component.html',
    styleUrls: ['./value-preview.component.scss']
})
export class ValuePreviewComponent {
    @HostBinding('class.preview') isPreview: boolean = true;

    OperatorEnum: typeof OperatorEnum = OperatorEnum;

    @Input() block!: ValueBlock;
}
