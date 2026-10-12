import { Routes } from '@angular/router';
import { TarefasComponent } from './components/tarefas/tarefas.component';
import { LoginComponent } from './components/login/login.component';

export const routes: Routes = [
  { path: '', component: TarefasComponent },
  { path: 'tarefas', component: TarefasComponent },
  { path: 'login', component: LoginComponent },
  { path: '**', redirectTo: '' },
];
