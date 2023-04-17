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
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ModalComponent } from './components/modal/modal.component';
import { SignalsComponent } from './components/signals/signals.component';
import { SignalComponent } from './components/signals/signal/signal.component';
import { StageComponent } from './components/signals/stage/stage.component';
import { BlockComponent } from './components/signals/block/block.component';
import { IndicatorComponent } from './components/signals/indicator/indicator.component';
import { StageItemComponent } from './components/signals/stage-item/stage-item.component';
import { UsersComponent } from './components/users/users.component';
import { UserComponent } from './components/users/user/user.component';
import { UserFormComponent } from './components/users/user-form/user-form.component';
import { SignalFormComponent } from './components/signals/signal-form/signal-form.component';
import { UserPreviewComponent } from './components/users/user-preview/user-preview.component';
import { LoginFormComponent } from './components/login/login-form/login-form.component';
import { ChannelComponent } from './components/channels/channel/channel.component';
import { ChannelPreviewComponent } from './components/channels/channel-preview/channel-preview.component';
import { ChannelFormComponent } from './components/channels/channel-form/channel-form.component';
import { ChannelFormVerifyComponent } from './components/channels/channel-form-verify/channel-form-verify.component';

@NgModule({
    declarations: [
        AppComponent,
        HomeComponent,
        LoginComponent,
        LogoutComponent,
        MenuComponent,
        ChannelsComponent,
        ModalComponent,
        SignalsComponent,
        SignalComponent,
        StageComponent,
        BlockComponent,
        IndicatorComponent,
        StageItemComponent,
        UsersComponent,
        UserComponent,
        UserFormComponent,
        SignalFormComponent,
        UserPreviewComponent,
        LoginFormComponent,
        ChannelComponent,
        ChannelPreviewComponent,
        ChannelFormComponent,
        ChannelFormVerifyComponent
    ],
    providers: [],
    bootstrap: [AppComponent],
    imports: [
        BrowserModule,
        FormsModule,
        ReactiveFormsModule,
        AppRoutingModule,
        HttpClientModule,
        OAuthModule.forRoot()
    ]
})
export class AppModule { }
