import { Component } from '@angular/core';

@Component({
  selector: 'app-menu',
  template: `
    <div>
      <ul>
          <li><a routerLink="/" routerLinkActive="active" ariaCurrentWhenActive="page">Home</a></li>
          <li><a routerLink="/channels" routerLinkActive="active" ariaCurrentWhenActive="page">Channels</a></li>
          <li><a routerLink="/logout" routerLinkActive="active" ariaCurrentWhenActive="page">Logout</a></li>
      </ul>
    </div>
  `
})
export class MenuComponent {

}
