import { Component, HostBinding, Input } from '@angular/core';
import { ConstantIndicator } from 'src/app/models/signal.model';

@Component({
    selector: 'app-constant-preview[indicator]',
    templateUrl: './constant-preview.component.html',
    styleUrls: ['./constant-preview.component.scss']
})
export class ConstantPreviewComponent {
    @HostBinding('class.preview') isPreview: boolean = true;

    @Input() indicator!: ConstantIndicator;
}
