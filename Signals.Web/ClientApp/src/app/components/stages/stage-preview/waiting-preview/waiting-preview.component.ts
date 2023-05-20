import { Component, Input } from '@angular/core';
import { WaitingStage } from 'src/app/models/signal.model';

@Component({
    selector: 'app-waiting-preview',
    templateUrl: './waiting-preview.component.html',
    styleUrls: ['./waiting-preview.component.scss']
})
export class WaitingPreviewComponent {
    @Input() stage!: WaitingStage;
}
