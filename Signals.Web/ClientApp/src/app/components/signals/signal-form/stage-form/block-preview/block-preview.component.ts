import { Component, HostBinding, Input } from '@angular/core';
import { Block, BlockType } from 'src/app/models/signal.model';

@Component({
  selector: 'app-block-preview[block]',
  templateUrl: './block-preview.component.html',
  styleUrls: ['./block-preview.component.scss']
})
export class BlockPreviewComponent {
  @HostBinding('class.preview') isPreview: boolean = true;

  @Input() block!: Block;

  BlockType: typeof BlockType = BlockType;

  castBlock<T>(): T {
    return this.block as T;
  }
}
