import { Routes } from '@angular/router';
import { LogInComponent } from './log-in/log-in.component';
import { AdvertisementsComponent } from './advertisements/advertisements.component';
import { RegisterComponent } from './register/register.component';
import { ProfileComponent } from './profile/profile.component';
import { NewAdvertisementComponent } from './new-advertisement/new-advertisement.component';
import { UserSettingsComponent } from './user-settings/user-settings.component';
import { AdvertisementEditorComponent } from './advertisement-editor/advertisement-editor.component';

export const routes: Routes = [
    {
        path: "log-in",
        component: LogInComponent
    },
    {
        path: "register",
        component: RegisterComponent
    },
    {
        path: "",
        redirectTo:"ads",
        pathMatch: "prefix"
    },
    {
        path: "ads",
        component: AdvertisementsComponent
    },
    {
        path: "profile",
        component: ProfileComponent
    },
    {
        path: "new-advertisement",
        component: NewAdvertisementComponent
    },
    {
        path: "settings",
        component: UserSettingsComponent
    },
    {
        path: "edit-advertisement",
        component: AdvertisementEditorComponent
    }
];
