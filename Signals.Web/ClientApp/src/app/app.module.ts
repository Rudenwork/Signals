import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomePageComponent } from './components/home/home-page/home-page.component';
import { LoginPageComponent } from './components/login/login-page/login-page.component';
import { LogoutComponent } from './components/logout/logout-page/logout-page.component';
import { MenuComponent } from './components/menu/menu.component';
import { ChannelsPageComponent } from './components/channels/channels-page/channels-page.component';
import { HttpClientModule } from '@angular/common/http';
import { OAuthModule } from 'angular-oauth2-oidc';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ModalComponent } from './components/modal/modal.component';
import { SignalsPageComponent } from './components/signals/signals-page/signals-page.component';
import { SignalComponent } from './components/signals/signal/signal.component';
import { StageComponent } from './components/stages/stage/stage.component';
import { UsersPageComponent } from './components/users/users-page/users-page.component';
import { UserComponent } from './components/users/user/user.component';
import { UserFormComponent } from './components/users/user-form/user-form.component';
import { SignalFormComponent } from './components/signals/signal-form/signal-form.component';
import { UserPreviewComponent } from './components/users/user-preview/user-preview.component';
import { LoginFormComponent } from './components/login/login-form/login-form.component';
import { ChannelComponent } from './components/channels/channel/channel.component';
import { ChannelPreviewComponent } from './components/channels/channel-preview/channel-preview.component';
import { ChannelFormComponent } from './components/channels/channel-form/channel-form.component';
import { ChannelFormVerifyComponent } from './components/channels/channel-form-verify/channel-form-verify.component';
import { StageFormComponent } from './components/stages/stage-form/stage-form.component';
import { SignalPreviewComponent } from './components/signals/signal-preview/signal-preview.component';
import { StagePreviewComponent } from './components/stages/stage-preview/stage-preview.component';
import { WaitingFormPartComponent } from './components/stages/stage-form/waiting-form-part/waiting-form-part.component';
import { NotificationFormPartComponent } from './components/stages/stage-form/notification-form-part/notification-form-part.component';
import { ConditionFormPartComponent } from './components/stages/stage-form/condition-form-part/condition-form-part.component';
import { BlockPreviewComponent } from './components/blocks/block-preview/block-preview.component';
import { ConditionPreviewComponent } from './components/stages/stage-preview/condition-preview/condition-preview.component';
import { NotificationPreviewComponent } from './components/stages/stage-preview/notification-preview/notification-preview.component';
import { WaitingPreviewComponent } from './components/stages/stage-preview/waiting-preview/waiting-preview.component';
import { GroupPreviewComponent } from './components/blocks/block-preview/group-preview/group-preview.component';
import { ValuePreviewComponent } from './components/blocks/block-preview/value-preview/value-preview.component';
import { ChangePreviewComponent } from './components/blocks/block-preview/change-preview/change-preview.component';
import { BlockFormComponent } from './components/blocks/block-form/block-form.component';
import { GroupFormPartComponent } from './components/blocks/block-form/group-form-part/group-form-part.component';
import { ValueFormPartComponent } from './components/blocks/block-form/value-form-part/value-form-part.component';
import { ChangeFormPartComponent } from './components/blocks/block-form/change-form-part/change-form-part.component';
import { IndicatorFormComponent } from './components/signals/signal-form/stage-form/block-form/indicator-form/indicator-form.component';
import { IndicatorPreviewComponent } from './components/signals/signal-form/stage-form/block-form/indicator-preview/indicator-preview.component';
import { BbFormPartComponent } from './components/signals/signal-form/stage-form/block-form/indicator-form/bb-form-part/bb-form-part.component';
import { CandleFormPartComponent } from './components/signals/signal-form/stage-form/block-form/indicator-form/candle-form-part/candle-form-part.component';
import { ConstantFormPartComponent } from './components/signals/signal-form/stage-form/block-form/indicator-form/constant-form-part/constant-form-part.component';
import { EmaFormPartComponent } from './components/signals/signal-form/stage-form/block-form/indicator-form/ema-form-part/ema-form-part.component';
import { RsiFormPartComponent } from './components/signals/signal-form/stage-form/block-form/indicator-form/rsi-form-part/rsi-form-part.component';
import { SmaFormPartComponent } from './components/signals/signal-form/stage-form/block-form/indicator-form/sma-form-part/sma-form-part.component';
import { BbPreviewComponent } from './components/signals/signal-form/stage-form/block-form/indicator-preview/bb-preview/bb-preview.component';
import { CandlePreviewComponent } from './components/signals/signal-form/stage-form/block-form/indicator-preview/candle-preview/candle-preview.component';
import { ConstantPreviewComponent } from './components/signals/signal-form/stage-form/block-form/indicator-preview/constant-preview/constant-preview.component';
import { EmaPreviewComponent } from './components/signals/signal-form/stage-form/block-form/indicator-preview/ema-preview/ema-preview.component';
import { RsiPreviewComponent } from './components/signals/signal-form/stage-form/block-form/indicator-preview/rsi-preview/rsi-preview.component';
import { SmaPreviewComponent } from './components/signals/signal-form/stage-form/block-form/indicator-preview/sma-preview/sma-preview.component';

@NgModule({
    declarations: [
        AppComponent,
        HomePageComponent,
        LoginPageComponent,
        LogoutComponent,
        MenuComponent,
        ChannelsPageComponent,
        ModalComponent,
        SignalsPageComponent,
        SignalComponent,
        StageComponent,
        UsersPageComponent,
        UserComponent,
        UserFormComponent,
        SignalFormComponent,
        UserPreviewComponent,
        LoginFormComponent,
        ChannelComponent,
        ChannelPreviewComponent,
        ChannelFormComponent,
        ChannelFormVerifyComponent,
        StageFormComponent,
        SignalPreviewComponent,
        StagePreviewComponent,
        WaitingFormPartComponent,
        NotificationFormPartComponent,
        ConditionFormPartComponent,
        BlockPreviewComponent,
        ConditionPreviewComponent,
        NotificationPreviewComponent,
        WaitingPreviewComponent,
        GroupPreviewComponent,
        ValuePreviewComponent,
        ChangePreviewComponent,
        BlockFormComponent,
        GroupFormPartComponent,
        ValueFormPartComponent,
        ChangeFormPartComponent,
        IndicatorFormComponent,
        IndicatorPreviewComponent,
        BbFormPartComponent,
        CandleFormPartComponent,
        ConstantFormPartComponent,
        EmaFormPartComponent,
        RsiFormPartComponent,
        SmaFormPartComponent,
        BbPreviewComponent,
        CandlePreviewComponent,
        ConstantPreviewComponent,
        EmaPreviewComponent,
        RsiPreviewComponent,
        SmaPreviewComponent
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
