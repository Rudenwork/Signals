import { Component, EventEmitter, Input, Output } from '@angular/core';
import { DataService } from 'src/app/services/data.service';

@Component({
    selector: 'app-channel-delete[id]',
    templateUrl: './channel-delete.component.html',
    styleUrls: ['./channel-delete.component.scss']
})
export class ChannelDeleteComponent {
    constructor(private dataService: DataService) { }

    @Output() deleted: EventEmitter<any> = new EventEmitter();
    @Input() id?: string;
    isDeleting: boolean = false;

    del() {
        this.isDeleting = true;
        this.dataService.deleteChannel(this.id)
            .subscribe(() => this.deleted.emit());
    }
}
