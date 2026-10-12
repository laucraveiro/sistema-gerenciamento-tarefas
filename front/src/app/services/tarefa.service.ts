import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Tarefa, TarefaCadastroRequest, TarefaAtualizacaoRequest } from '../models/tarefa.model';

@Injectable({
  providedIn: 'root',
})
export class TarefaService {
  private readonly apiUrl = environment.apiUrl;

  constructor(private readonly http: HttpClient) {}

  /**
   * GET /api/usuarios/{usuarioId}/tarefas
   * Busca somente as tarefas da usuária especificada
   */
  listarTarefas(usuarioId: number): Observable<Tarefa[]> {
    return this.http.get<Tarefa[]>(`${this.apiUrl}/usuarios/${usuarioId}/tarefas`);
  }

  /**
   * POST /api/usuarios/{usuarioId}/tarefas
   * Cadastra uma nova tarefa para a usuária
   */
  criarTarefa(usuarioId: number, request: TarefaCadastroRequest): Observable<Tarefa> {
    return this.http.post<Tarefa>(`${this.apiUrl}/usuarios/${usuarioId}/tarefas`, request);
  }

  /**
   * PUT /api/usuarios/{usuarioId}/tarefas/{tarefaId}
   * Atualiza uma tarefa existente
   */
  atualizarTarefa(usuarioId: number, tarefaId: number, request: TarefaAtualizacaoRequest): Observable<Tarefa> {
    return this.http.put<Tarefa>(`${this.apiUrl}/usuarios/${usuarioId}/tarefas/${tarefaId}`, request);
  }

  /**
   * PUT /api/usuarios/{usuarioId}/tarefas/{tarefaId}/concluir
   * Conclui a tarefa
   */
  concluirTarefa(usuarioId: number, tarefaId: number): Observable<Tarefa> {
    return this.http.put<Tarefa>(`${this.apiUrl}/usuarios/${usuarioId}/tarefas/${tarefaId}/concluir`, {});
  }

  /**
   * DELETE /api/usuarios/{usuarioId}/tarefas/{tarefaId}
   * Exclui a tarefa
   */
  excluirTarefa(usuarioId: number, tarefaId: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/usuarios/${usuarioId}/tarefas/${tarefaId}`);
  }
}
