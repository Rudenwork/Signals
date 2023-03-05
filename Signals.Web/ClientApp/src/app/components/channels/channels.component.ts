import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-channels',
  templateUrl: './channels.component.html',
  styleUrls: ['./channels.component.scss']
})
export class ChannelsComponent implements OnInit {
  constructor(private authService: AuthService, private http: HttpClient) { }

  test!: string;

  ngOnInit() {

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${this.authService.getToken()}`
    })

    this.http.get(`${window.origin}/api/channels`, { headers: headers })
      .subscribe(response => this.test = JSON.stringify(response));
  }
}
