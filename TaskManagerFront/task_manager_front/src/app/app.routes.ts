import { Routes } from '@angular/router';

export const routes: Routes = [
    {path: '', redirectTo: 'home', pathMatch: 'full'},
    {path: 'home', loadComponent: () => import('./features/home/home.component').then(m => m.HomeComponent)},
    {path: 'users/new', loadComponent: () => import('./features/forms/pages/add-user-form/add-user-form.component').then(m => m.AddUserFormComponent)},
    {path: 'tasks/new', loadComponent: () => import('./features/forms/pages/add-user-task-form/add-user-task-form.component').then(m => m.AddUserTaskFormComponent)}, 
];
