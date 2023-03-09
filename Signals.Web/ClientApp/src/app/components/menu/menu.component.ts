import { Component } from '@angular/core';

@Component({
  selector: 'app-menu',
  template: `
    <ul>
      <li>
        <a routerLink="/" routerLinkActive="active" ariaCurrentWhenActive="page">Home</a>
      </li>
      <li>
        <a routerLink="/channels" routerLinkActive="active" ariaCurrentWhenActive="page">Channels</a>
      </li>
      <li>
        <a routerLink="/logout" routerLinkActive="active" ariaCurrentWhenActive="page">Logout</a>
      </li>
    </ul>
  `,
  styles:[`
    :host {
      background-color: grey;
      min-width: 150px;

      ul li a {
        color: white;
      }
    }
  `]
})
export class MenuComponent {

}
