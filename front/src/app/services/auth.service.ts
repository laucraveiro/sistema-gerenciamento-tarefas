import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Usuaria } from '../models/tarefa.model';

export const SENHA_PADRAO = '12345';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly apiUrl = environment.apiUrl;

  constructor(private readonly http: HttpClient) {}

  /**
   * Recupera o usuarioId salvo no login (armazenado no localStorage).
   */
  getUsuarioId(): number | null {
    const rawId = localStorage.getItem('usuarioId');
    if (rawId) {
      const parsed = parseInt(rawId, 10);
      if (!isNaN(parsed) && parsed > 0) {
        return parsed;
      }
    }

    // Fallback: se o objeto completo da usuária foi salvo em 'usuario'
    const rawUser = localStorage.getItem('usuario');
    if (rawUser) {
      try {
        const userObj = JSON.parse(rawUser);
        if (userObj?.id) {
          return parseInt(userObj.id, 10);
        }
      } catch {
        // Ignora erro de JSON mal formatado
      }
    }

    return null;
  }

  getUsuarioNome(): string {
    return localStorage.getItem('usuarioNome') || 'Usuária';
  }

  salvarLogin(usuario: Usuaria): void {
    localStorage.setItem('usuarioId', usuario.id.toString());
    localStorage.setItem('usuarioNome', usuario.nome);
    localStorage.setItem('usuario', JSON.stringify(usuario));
  }

  logout(): void {
    localStorage.removeItem('usuarioId');
    localStorage.removeItem('usuarioNome');
    localStorage.removeItem('usuario');
  }


  cadastrar(nome: string, email: string): Observable<Usuaria> {
    return this.http.post<Usuaria>(`${this.apiUrl}/usuarias`, {
      nome: nome.trim().toLowerCase(),
      email: email.trim(),
      senha: SENHA_PADRAO,
    });
  }

  login(nome: string, senha: string): Observable<Usuaria> {
    return this.http.post<Usuaria>(`${this.apiUrl}/usuarias/login`, {
      nome: nome.trim().toLowerCase(),
      senha,
    });
  }
}

