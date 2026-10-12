import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { AuthService, SENHA_PADRAO } from '../../services/auth.service';

@Component({
  selector: 'app-cadastro',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './cadastro.component.html',
  styleUrls: ['../login/login.component.css'],
})
export class CadastroComponent {
  nome = '';
  email = '';

  carregando = signal(false);
  mensagemErro = signal('');
  mensagemSucesso = signal('');

  constructor(private readonly authService: AuthService) {}

  cadastrar(): void {
    this.mensagemErro.set('');
    this.mensagemSucesso.set('');

    if (!this.nome.trim() || !this.email.trim()) {
      this.mensagemErro.set('Preencha o nome e o e-mail.');
      return;
    }

    this.carregando.set(true);
    this.authService.cadastrar(this.nome, this.email).subscribe({
      next: (usuaria) => {
        this.carregando.set(false);
        this.mensagemSucesso.set(
          `Cadastro feito, ${usuaria.nome}! Para entrar, use seu nome e a senha ${SENHA_PADRAO}.`,
        );
        this.nome = '';
        this.email = '';
      },
      error: (err) => {
        this.carregando.set(false);
        this.mensagemErro.set(
          err?.error?.mensagem || 'Não foi possível fazer o cadastro. Tente novamente.',
        );
      },
    });
  }
}