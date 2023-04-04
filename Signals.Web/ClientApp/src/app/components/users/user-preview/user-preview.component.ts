import { Component, Input } from '@angular/core';
import { User } from 'src/app/models/user.model';

@Component({
    selector: 'app-user-preview[user]',
    templateUrl: './user-preview.component.html',
    styleUrls: ['./user-preview.component.scss']
})
export class UserPreviewComponent {
    @Input() user!: User;
}
