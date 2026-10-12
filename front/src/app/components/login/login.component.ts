import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { Usuaria } from '../../models/tarefa.model';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent implements OnInit {
  nome: string = 'maria';
  senha: string = '12345';
  carregando: boolean = false;
  mensagemErro: string = '';
  mensagemSucesso: string = '';
  usuariasDisponiveis: Usuaria[] = [];

  constructor(
    private readonly authService: AuthService,
    private readonly router: Router
  ) {}

  ngOnInit(): void {
    this.carregarUsuarias();
  }

  carregarUsuarias(): void {
    this.authService.listarUsuarias().subscribe({
      next: (usuarias) => {
        this.usuariasDisponiveis = usuarias;
      },
      error: () => {},
    });
  }

  fazerLogin(): void {
    this.mensagemErro = '';
    this.mensagemSucesso = '';

    if (!this.nome.trim() || !this.senha.trim()) {
      this.mensagemErro = 'Por favor, preencha o nome e a senha.';
      return;
    }

    this.carregando = true;
    this.authService.login(this.nome.trim(), this.senha.trim()).subscribe({
      next: (usuario) => {
        this.carregando = false;
        this.authService.salvarLogin(usuario);
        this.mensagemSucesso = `Bem-vinda, ${usuario.nome}! Redirecionando...`;
        setTimeout(() => {
          this.router.navigate(['/tarefas']);
        }, 800);
      },
      error: (err) => {
        this.carregando = false;
        const msg = err?.error?.mensagem || 'Falha no login. Verifique seu nome e senha.';
        this.mensagemErro = msg;
      },
    });
  }

  selecionarDireto(usuaria: Usuaria): void {
    this.nome = usuaria.nome;
    this.authService.salvarLogin(usuaria);
    this.mensagemSucesso = `Login como ${usuaria.nome} efetuado com sucesso! Redirecionando...`;
    setTimeout(() => {
      this.router.navigate(['/tarefas']);
    }, 600);
  }
}
