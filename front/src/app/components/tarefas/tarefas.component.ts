import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { TarefaService } from '../../services/tarefa.service';
import { AuthService } from '../../services/auth.service';
import { Tarefa, TarefaCadastroRequest, TarefaAtualizacaoRequest, Usuaria } from '../../models/tarefa.model';

@Component({
  selector: 'app-tarefas',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './tarefas.component.html',
  styleUrls: ['./tarefas.component.css'],
})
export class TarefasComponent implements OnInit {
  usuarioId: number | null = null;
  usuarioNome: string = '';
  tarefas: Tarefa[] = [];
  carregando: boolean = false;
  usuariasDisponiveis: Usuaria[] = [];

  // Feedback messages
  mensagemSucesso: string = '';
  mensagemErro: string = '';

  // Formulário Nova Tarefa
  novaTarefa: TarefaCadastroRequest = {
    titulo: '',
    descricao: '',
    dataVencimento: '',
  };

  // Formulário / Modal Edição
  tarefaEmEdicao: Tarefa | null = null;
  dadosEdicao: TarefaAtualizacaoRequest = {
    titulo: '',
    descricao: '',
    dataVencimento: '',
  };

  constructor(
    private readonly tarefaService: TarefaService,
    private readonly authService: AuthService,
    private readonly router: Router
  ) {}

  ngOnInit(): void {
    this.obterUsuarioEListar();
  }

  /**
   * Recupera o usuarioId salvo no login e busca as tarefas
   */
  obterUsuarioEListar(): void {
    this.usuarioId = this.authService.getUsuarioId();
    this.usuarioNome = this.authService.getUsuarioNome();

    if (this.usuarioId) {
      this.carregarTarefas();
    } else {
      // Carrega usuárias do sistema caso queira selecionar uma para testar
      this.carregarUsuariasParaSelecao();
    }
  }

  carregarUsuariasParaSelecao(): void {
    this.authService.listarUsuarias().subscribe({
      next: (usuarias) => {
        this.usuariasDisponiveis = usuarias;
      },
      error: () => {
        // Se a API ainda estiver iniciando ou sem usuárias
      },
    });
  }

  selecionarUsuaria(usuaria: Usuaria): void {
    this.authService.salvarLogin(usuaria);
    this.usuarioId = usuaria.id;
    this.usuarioNome = usuaria.nome;
    this.limparMensagens();
    this.carregarTarefas();
  }

  /**
   * GET /api/usuarios/{usuarioId}/tarefas
   */
  carregarTarefas(): void {
    if (!this.usuarioId) {
      this.mensagemErro = 'Nenhum ID de usuária identificado. Faça login ou selecione uma usuária.';
      return;
    }

    this.carregando = true;
    this.tarefaService.listarTarefas(this.usuarioId).subscribe({
      next: (tarefas) => {
        this.tarefas = tarefas;
        this.carregando = false;
      },
      error: (err) => {
        this.carregando = false;
        this.exibirErro(err, 'Erro ao carregar lista de tarefas.');
      },
    });
  }

  /**
   * POST /api/usuarios/{usuarioId}/tarefas
   */
  salvarNovaTarefa(): void {
    this.limparMensagens();

    if (!this.usuarioId) {
      this.mensagemErro = 'Usuária não identificada para cadastrar tarefa.';
      return;
    }

    if (!this.novaTarefa.titulo.trim()) {
      this.mensagemErro = 'Por favor, informe o título da tarefa.';
      return;
    }

    if (!this.novaTarefa.descricao.trim()) {
      this.mensagemErro = 'Por favor, informe a descrição da tarefa.';
      return;
    }

    if (!this.novaTarefa.dataVencimento) {
      this.mensagemErro = 'Por favor, informe a data de vencimento da tarefa.';
      return;
    }

    this.carregando = true;
    this.tarefaService.criarTarefa(this.usuarioId, this.novaTarefa).subscribe({
      next: () => {
        this.carregando = false;
        this.mensagemSucesso = 'Tarefa cadastrada com sucesso!';
        this.limparFormularioNovaTarefa();
        this.carregarTarefas();
      },
      error: (err) => {
        this.carregando = false;
        this.exibirErro(err, 'Erro ao cadastrar tarefa.');
      },
    });
  }

  /**
   * Abre formulário de edição pré-preenchido
   */
  iniciarEdicao(tarefa: Tarefa): void {
    this.limparMensagens();
    this.tarefaEmEdicao = tarefa;

    // Formata a data para YYYY-MM-DD para o input type="date"
    let dataFormatada = '';
    if (tarefa.dataVencimento) {
      dataFormatada = tarefa.dataVencimento.split('T')[0];
    }

    this.dadosEdicao = {
      titulo: tarefa.titulo,
      descricao: tarefa.descricao,
      dataVencimento: dataFormatada,
    };
  }

  cancelarEdicao(): void {
    this.tarefaEmEdicao = null;
  }

  /**
   * PUT /api/usuarios/{usuarioId}/tarefas/{tarefaId}
   */
  salvarEdicao(): void {
    this.limparMensagens();

    if (!this.usuarioId || !this.tarefaEmEdicao) {
      return;
    }

    if (!this.dadosEdicao.titulo.trim()) {
      this.mensagemErro = 'O título não pode ficar vazio.';
      return;
    }

    if (!this.dadosEdicao.descricao.trim()) {
      this.mensagemErro = 'A descrição não pode ficar vazia.';
      return;
    }

    if (!this.dadosEdicao.dataVencimento) {
      this.mensagemErro = 'A data de vencimento é obrigatória.';
      return;
    }

    this.carregando = true;
    this.tarefaService
      .atualizarTarefa(this.usuarioId, this.tarefaEmEdicao.id, this.dadosEdicao)
      .subscribe({
        next: () => {
          this.carregando = false;
          this.mensagemSucesso = 'Tarefa atualizada com sucesso!';
          this.tarefaEmEdicao = null;
          this.carregarTarefas();
        },
        error: (err) => {
          this.carregando = false;
          this.exibirErro(err, 'Erro ao atualizar a tarefa.');
        },
      });
  }

  /**
   * PUT /api/usuarios/{usuarioId}/tarefas/{tarefaId}/concluir
   */
  concluirTarefa(tarefa: Tarefa): void {
    this.limparMensagens();

    if (!this.usuarioId) {
      return;
    }

    this.carregando = true;
    this.tarefaService.concluirTarefa(this.usuarioId, tarefa.id).subscribe({
      next: () => {
        this.carregando = false;
        this.mensagemSucesso = `Tarefa "${tarefa.titulo}" marcada como concluída!`;
        this.carregarTarefas();
      },
      error: (err) => {
        this.carregando = false;
        this.exibirErro(err, 'Erro ao concluir tarefa.');
      },
    });
  }

  /**
   * DELETE /api/usuarios/{usuarioId}/tarefas/{tarefaId}
   * Pede confirmação antes de excluir
   */
  excluirTarefa(tarefa: Tarefa): void {
    this.limparMensagens();

    if (!this.usuarioId) {
      return;
    }

    const confirmou = window.confirm(
      `Tem certeza que deseja excluir a tarefa "${tarefa.titulo}"? Esta ação não pode ser desfeita.`
    );

    if (!confirmou) {
      return;
    }

    this.carregando = true;
    this.tarefaService.excluirTarefa(this.usuarioId, tarefa.id).subscribe({
      next: () => {
        this.carregando = false;
        this.mensagemSucesso = `Tarefa "${tarefa.titulo}" excluída com sucesso!`;
        this.carregarTarefas();
      },
      error: (err) => {
        this.carregando = false;
        this.exibirErro(err, 'Erro ao excluir tarefa.');
      },
    });
  }

  sair(): void {
    this.authService.logout();
    this.usuarioId = null;
    this.usuarioNome = '';
    this.tarefas = [];
    this.limparMensagens();
    this.router.navigate(['/login']);
  }

  limparFormularioNovaTarefa(): void {
    this.novaTarefa = {
      titulo: '',
      descricao: '',
      dataVencimento: '',
    };
  }

  limparMensagens(): void {
    this.mensagemSucesso = '';
    this.mensagemErro = '';
  }

  formatarData(data: string): string {
    if (!data) return '-';
    try {
      const parteData = data.split('T')[0];
      const partes = parteData.split('-');
      if (partes.length === 3) {
        return `${partes[2]}/${partes[1]}/${partes[0]}`;
      }
      return data;
    } catch {
      return data;
    }
  }

  isConcluida(status: string): boolean {
    if (!status) return false;
    return status.toLowerCase() === 'concluida' || status === '2';
  }

  private exibirErro(err: any, mensagemPadrao: string): void {
    let detalhe = '';
    if (err?.error?.mensagem) {
      detalhe = err.error.mensagem;
    } else if (typeof err?.error === 'string') {
      detalhe = err.error;
    } else if (err?.message) {
      detalhe = err.message;
    }

    this.mensagemErro = detalhe ? `${mensagemPadrao} (${detalhe})` : mensagemPadrao;
  }
}
