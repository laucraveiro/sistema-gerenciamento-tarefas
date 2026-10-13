import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { TarefaService } from '../../services/tarefa.service';
import { AuthService } from '../../services/auth.service';
import { Tarefa, TarefaCadastroRequest, TarefaAtualizacaoRequest } from '../../models/tarefa.model';

@Component({
  selector: 'app-tarefas',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './tarefas.component.html',
  styleUrls: ['./tarefas.component.css'],
})
export class TarefasComponent implements OnInit {
  usuarioId = signal<number | null>(null);
  usuarioNome = signal('');
  tarefas = signal<Tarefa[]>([]);
  carregando = signal(false);

  // Feedback messages
  mensagemSucesso = signal('');
  mensagemErro = signal('');

  // Formulário Nova Tarefa (objeto comum, por causa do ngModel)
  novaTarefa: TarefaCadastroRequest = {
    titulo: '',
    descricao: '',
    dataVencimento: '',
  };

  // Formulário / Modal Edição
  tarefaEmEdicao = signal<Tarefa | null>(null);
  dadosEdicao: TarefaAtualizacaoRequest = {
    titulo: '',
    descricao: '',
    dataVencimento: '',
  };

  constructor(
    private readonly tarefaService: TarefaService,
    private readonly authService: AuthService,
    private readonly router: Router,
  ) {}

  ngOnInit(): void {
    this.obterUsuarioEListar();
  }

  /**
   * Recupera o usuarioId salvo no login e busca as tarefas
   */
  obterUsuarioEListar(): void {
    this.usuarioId.set(this.authService.getUsuarioId());
    this.usuarioNome.set(this.authService.getUsuarioNome());

    if (this.usuarioId()) {
      this.carregarTarefas();
    }
  }

  /**
   * GET /api/usuarios/{usuarioId}/tarefas
   */
  carregarTarefas(): void {
    const usuarioId = this.usuarioId();

    if (!usuarioId) {
      this.mensagemErro.set('Nenhum ID de usuária identificado. Faça login para continuar.');
      return;
    }

    this.carregando.set(true);
    this.tarefaService.listarTarefas(usuarioId).subscribe({
      next: (tarefas) => {
        this.tarefas.set(tarefas);
        this.carregando.set(false);
      },
      error: (err) => {
        this.carregando.set(false);
        this.exibirErro(err, 'Erro ao carregar lista de tarefas.');
      },
    });
  }

  /**
   * POST /api/usuarios/{usuarioId}/tarefas
   */
  salvarNovaTarefa(): void {
    this.limparMensagens();

    const usuarioId = this.usuarioId();

    if (!usuarioId) {
      this.mensagemErro.set('Usuária não identificada para cadastrar tarefa.');
      return;
    }

    if (!this.novaTarefa.titulo.trim()) {
      this.mensagemErro.set('Por favor, informe o título da tarefa.');
      return;
    }

    if (!this.novaTarefa.descricao.trim()) {
      this.mensagemErro.set('Por favor, informe a descrição da tarefa.');
      return;
    }

    if (!this.novaTarefa.dataVencimento) {
      this.mensagemErro.set('Por favor, informe a data de vencimento da tarefa.');
      return;
    }

    this.carregando.set(true);
    this.tarefaService.criarTarefa(usuarioId, this.novaTarefa).subscribe({
      next: () => {
        this.carregando.set(false);
        this.mensagemSucesso.set('Tarefa cadastrada com sucesso!');
        this.limparFormularioNovaTarefa();
        this.carregarTarefas();
      },
      error: (err) => {
        this.carregando.set(false);
        this.exibirErro(err, 'Erro ao cadastrar tarefa.');
      },
    });
  }

  /**
   * Abre formulário de edição pré-preenchido
   */
  iniciarEdicao(tarefa: Tarefa): void {
    this.limparMensagens();
    this.tarefaEmEdicao.set(tarefa);

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
    this.tarefaEmEdicao.set(null);
  }

  /**
   * PUT /api/usuarios/{usuarioId}/tarefas/{tarefaId}
   */
  salvarEdicao(): void {
    this.limparMensagens();

    const usuarioId = this.usuarioId();
    const tarefaEmEdicao = this.tarefaEmEdicao();

    if (!usuarioId || !tarefaEmEdicao) {
      return;
    }

    if (!this.dadosEdicao.titulo.trim()) {
      this.mensagemErro.set('O título não pode ficar vazio.');
      return;
    }

    if (!this.dadosEdicao.descricao.trim()) {
      this.mensagemErro.set('A descrição não pode ficar vazia.');
      return;
    }

    if (!this.dadosEdicao.dataVencimento) {
      this.mensagemErro.set('A data de vencimento é obrigatória.');
      return;
    }

    this.carregando.set(true);
    this.tarefaService.atualizarTarefa(usuarioId, tarefaEmEdicao.id, this.dadosEdicao).subscribe({
      next: () => {
        this.carregando.set(false);
        this.mensagemSucesso.set('Tarefa atualizada com sucesso!');
        this.tarefaEmEdicao.set(null);
        this.carregarTarefas();
      },
      error: (err) => {
        this.carregando.set(false);
        this.exibirErro(err, 'Erro ao atualizar a tarefa.');
      },
    });
  }

  /**
   * PUT /api/usuarios/{usuarioId}/tarefas/{tarefaId}/concluir
   */
  concluirTarefa(tarefa: Tarefa): void {
    this.limparMensagens();

    const usuarioId = this.usuarioId();

    if (!usuarioId) {
      return;
    }

    this.carregando.set(true);
    this.tarefaService.concluirTarefa(usuarioId, tarefa.id).subscribe({
      next: () => {
        this.carregando.set(false);
        this.mensagemSucesso.set(`Tarefa "${tarefa.titulo}" marcada como concluída!`);
        this.carregarTarefas();
      },
      error: (err) => {
        this.carregando.set(false);
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

    const usuarioId = this.usuarioId();

    if (!usuarioId) {
      return;
    }

    const confirmou = window.confirm(
      `Tem certeza que deseja excluir a tarefa "${tarefa.titulo}"? Esta ação não pode ser desfeita.`,
    );

    if (!confirmou) {
      return;
    }

    this.carregando.set(true);
    this.tarefaService.excluirTarefa(usuarioId, tarefa.id).subscribe({
      next: () => {
        this.carregando.set(false);
        this.mensagemSucesso.set(`Tarefa "${tarefa.titulo}" excluída com sucesso!`);
        this.carregarTarefas();
      },
      error: (err) => {
        this.carregando.set(false);
        this.exibirErro(err, 'Erro ao excluir tarefa.');
      },
    });
  }

  sair(): void {
    this.authService.logout();
    this.usuarioId.set(null);
    this.usuarioNome.set('');
    this.tarefas.set([]);
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
    this.mensagemSucesso.set('');
    this.mensagemErro.set('');
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

    this.mensagemErro.set(detalhe ? `${mensagemPadrao} (${detalhe})` : mensagemPadrao);
  }
}
