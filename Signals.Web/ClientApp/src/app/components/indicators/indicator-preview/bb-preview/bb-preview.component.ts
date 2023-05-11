import { Component, HostBinding, Input } from '@angular/core';
import { BBIndicator } from 'src/app/models/signal.model';

@Component({
    selector: 'app-bb-preview[indicator]',
    templateUrl: './bb-preview.component.html',
    styleUrls: ['./bb-preview.component.scss']
})
export class BbPreviewComponent {
    @HostBinding('class.preview') isPreview: boolean = true;

    @Input() indicator!: BBIndicator;
}
