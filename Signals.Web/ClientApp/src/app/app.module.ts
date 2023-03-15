import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from './components/login/login.component';
import { LogoutComponent } from './components/logout/logout.component';
import { MenuComponent } from './components/menu/menu.component';
import { ChannelsComponent } from './components/channels/channels.component';
import { HttpClientModule } from '@angular/common/http';
import { OAuthModule } from 'angular-oauth2-oidc';
import { FormsModule } from '@angular/forms';
import { ChannelItemComponent } from './components/channels/channel-item/channel-item.component';
import { ChannelItemTelegramComponent } from './components/channels/channel-item/channel-item-telegram/channel-item-telegram.component';
import { ChannelItemEmailComponent } from './components/channels/channel-item/channel-item-email/channel-item-email.component';
import { ModalComponent } from './components/modal/modal.component';
import { ChannelAddComponent } from './components/channels/channel-add/channel-add.component';
import { ChannelAddTelegramComponent } from './components/channels/channel-add/channel-add-telegram/channel-add-telegram.component';
import { ChannelAddEmailComponent } from './components/channels/channel-add/channel-add-email/channel-add-email.component';
import { ChannelDeleteComponent } from './components/channels/channel-delete/channel-delete.component';
import { ChannelDeleteTelegramComponent } from './components/channels/channel-delete/channel-delete-telegram/channel-delete-telegram.component';
import { ChannelDeleteEmailComponent } from './components/channels/channel-delete/channel-delete-email/channel-delete-email.component';

@NgModule({
    declarations: [
        AppComponent,
        HomeComponent,
        LoginComponent,
        LogoutComponent,
        MenuComponent,
        ChannelsComponent,
        ChannelItemComponent,
        ChannelItemTelegramComponent,
        ChannelItemEmailComponent,
        ModalComponent,
        ChannelAddComponent,
        ChannelAddTelegramComponent,
        ChannelAddEmailComponent,
        ChannelDeleteComponent,
        ChannelDeleteTelegramComponent,
        ChannelDeleteEmailComponent
    ],
    providers: [],
    bootstrap: [AppComponent],
    imports: [
        BrowserModule,
        FormsModule,
        AppRoutingModule,
        HttpClientModule,
        OAuthModule.forRoot()
    ]
})
export class AppModule { }
