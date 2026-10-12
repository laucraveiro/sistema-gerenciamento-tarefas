export interface Tarefa {
  id: number;
  titulo: string;
  descricao: string;
  dataVencimento: string;
  status: string;
  usuariaId: number;
}

export interface TarefaCadastroRequest {
  titulo: string;
  descricao: string;
  dataVencimento: string;
}

export interface TarefaAtualizacaoRequest {
  titulo: string;
  descricao: string;
  dataVencimento: string;
}

export interface Usuaria {
  id: number;
  nome: string;
  email: string;
}
