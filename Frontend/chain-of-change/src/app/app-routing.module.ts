import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

// has lazy loading. Read: https://malcoded.com/posts/angular-fundamentals-modules/
const routes: Routes = [{ path: 'auth', loadChildren: () => import('./auth/auth.module').then(m => m.AuthModule) }, { path: 'maps', loadChildren: () => import('./maps/maps.module').then(m => m.MapsModule) }, { path: 'admin', loadChildren: () => import('./back-office/back-office.module').then(m => m.BackOfficeModule) }];

@NgModule({
  imports: [RouterModule.forRoot(routes, {
    initialNavigation: 'enabled'
})],
  exports: [RouterModule]
})
export class AppRoutingModule { }
