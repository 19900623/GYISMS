import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AppRouteGuard } from '@shared/auth/auth-route-guard';
import { HomeComponent } from '@app/home/home.component';
//import { AppComponent } from '@app/app.component';
import { TenantsComponent } from '@app/tenants/tenants.component';
import { RolesComponent } from '@app/roles/roles.component';
import { UsersComponent } from '@app/users/users.component';
import { DefaultLayoutComponent } from 'layout/default-layout.component';
import { ACLGuard } from '@delon/acl';

const routes: Routes = [
  {
    path: 'app',
    component: DefaultLayoutComponent,
    canActivate: [AppRouteGuard],
    canActivateChild: [AppRouteGuard],
    children: [
      {
        path: 'home',
        component: HomeComponent,
        canActivate: [AppRouteGuard],
      },
      {
        path: 'tenants',
        component: TenantsComponent,
        canActivate: [AppRouteGuard, ACLGuard],
        data: { guard: 'Admin' },
      },
      {
        path: 'roles',
        component: RolesComponent,
        canActivate: [AppRouteGuard, ACLGuard],
        data: { guard: 'Admin' },
      },
      {
        path: 'users',
        component: UsersComponent,
        canActivate: [AppRouteGuard, ACLGuard],
        data: { guard: 'Admin' },
      },
      {
        path: 'basic',
        loadChildren: 'basic-data/basic-data.module#BasicDataModule', // Lazy load account module
        data: { preload: true },
      },
      {
        path: 'meeting',
        loadChildren: 'meeting-management/meeting-management-module#MeetingManagementModule', // Lazy load account module
        data: { preload: true },
      },
      {
        path: 'task',
        loadChildren: 'tobacco-management/tobacco-management-module#TobaccoManagementModule', // Lazy load account module
        data: { preload: true },
      },
      {
        path: 'config',
        loadChildren: 'configs/configs.module#ConfigModule', // Lazy load account module
        data: { preload: true },
      },
      {
        path: 'doc',
        loadChildren: 'documents/documents.module#DocumentsModule', // Lazy load account module
        data: { preload: true },
      },
      {
        path: '**',
        redirectTo: 'home',
      },
    ],
  },
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule { }
