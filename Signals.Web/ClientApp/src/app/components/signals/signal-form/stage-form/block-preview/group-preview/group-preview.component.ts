import { Component, HostBinding, Input } from '@angular/core';
import { GroupBlock, GroupBlockType } from 'src/app/models/signal.model';

@Component({
    selector: 'app-group-preview[block]',
    templateUrl: './group-preview.component.html',
    styleUrls: ['./group-preview.component.scss']
})
export class GroupPreviewComponent {
    @HostBinding('class.preview') isPreview: boolean = true;

    GroupBlockType: typeof GroupBlockType = GroupBlockType;

    @Input() block!: GroupBlock;
}
