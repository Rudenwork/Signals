import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ChannelsPageComponent } from './components/channels/channels-page/channels-page.component';
import { HomePageComponent } from './components/home/home-page/home-page.component';
import { LoginPageComponent } from './components/login/login-page/login-page.component';
import { LogoutComponent } from './components/logout/logout-page/logout-page.component';
import { SignalsComponent } from './components/signals/signals.component';
import { UsersComponent } from './components/users/users.component';

const routes: Routes = [
    { path: '', component: HomePageComponent, pathMatch: 'full' },
    { path: 'login', component: LoginPageComponent },
    { path: 'logout', component: LogoutComponent },
    { path: 'users', component: UsersComponent },
    { path: 'channels', component: ChannelsPageComponent },
    { path: 'signals', component: SignalsComponent }
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule { }
