import { Component, Input } from '@angular/core';
import { NotificationStage } from 'src/app/models/signal.model';

@Component({
    selector: 'app-notification-preview',
    templateUrl: './notification-preview.component.html',
    styleUrls: ['./notification-preview.component.scss']
})
export class NotificationPreviewComponent {
    @Input() stage!: NotificationStage;
}
