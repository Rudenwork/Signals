import { Component, HostBinding, Input } from '@angular/core';
import { Stage } from 'src/app/models/signal.model';

@Component({
    selector: 'app-stage-preview[stage]',
    templateUrl: './stage-preview.component.html',
    styleUrls: ['./stage-preview.component.scss']
})
export class StagePreviewComponent {
    @HostBinding('class.preview') isPreview: boolean = true;

    @Input() stage!: Stage;
}
