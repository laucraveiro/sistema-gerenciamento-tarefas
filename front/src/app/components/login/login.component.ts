import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent {
  nome = '';
  senha = '';

  carregando = signal(false);
  mensagemErro = signal('');
  mensagemSucesso = signal('');

  constructor(
    private readonly authService: AuthService,
    private readonly router: Router,
  ) {}

  fazerLogin(): void {
    this.mensagemErro.set('');
    this.mensagemSucesso.set('');

    if (!this.nome.trim() || !this.senha) {
      this.mensagemErro.set('Por favor, preencha o nome e a senha.');
      return;
    }

    this.carregando.set(true);
    this.authService.login(this.nome, this.senha).subscribe({
      next: (usuaria) => {
        this.carregando.set(false);
        this.authService.salvarLogin(usuaria);
        this.mensagemSucesso.set(`Bem-vinda, ${usuaria.nome}! Redirecionando...`);
        setTimeout(() => this.router.navigate(['/tarefas']), 800);
      },
      error: (err) => {
        this.carregando.set(false);
        this.mensagemErro.set(
          err?.status === 0
            ? 'Não foi possível conectar ao servidor. Verifique se a API está rodando.'
            : err?.error?.mensagem || 'Falha no login. Verifique seu nome e senha.',
        );
      },
    });
  }
}