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
import { ChannelCreateComponent } from './components/channels/channel-create/channel-create.component';
import { ChannelCreateTelegramComponent } from './components/channels/channel-create/channel-create-telegram/channel-create-telegram.component';
import { ChannelCreateEmailComponent } from './components/channels/channel-create/channel-create-email/channel-create-email.component';
import { ChannelDeleteComponent } from './components/channels/channel-delete/channel-delete.component';
import { ChannelDeleteTelegramComponent } from './components/channels/channel-delete/channel-delete-telegram/channel-delete-telegram.component';
import { ChannelDeleteEmailComponent } from './components/channels/channel-delete/channel-delete-email/channel-delete-email.component';
import { ChannelUpdateComponent } from './components/channels/channel-update/channel-update.component';
import { ChannelUpdateEmailComponent } from './components/channels/channel-update/channel-update-email/channel-update-email.component';
import { ChannelUpdateTelegramComponent } from './components/channels/channel-update/channel-update-telegram/channel-update-telegram.component';

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
        ChannelCreateComponent,
        ChannelCreateTelegramComponent,
        ChannelCreateEmailComponent,
        ChannelDeleteComponent,
        ChannelDeleteTelegramComponent,
        ChannelDeleteEmailComponent,
        ChannelUpdateComponent,
        ChannelUpdateEmailComponent,
        ChannelUpdateTelegramComponent
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
