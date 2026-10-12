# 🚀sistema-gerenciamento-tarefas

Este é um projeto de treinamento desenvolvido como parte do aprendizado prático no **Bootcamp WomakersCode**. A aplicação simula um cenário real composto por um ecossistema dividido em duas partes: um **Backend (API)** construído em .NET/C# integrado a um banco de dados leve, e uma interface **Frontend** desenvolvida em Angular.

---

## 🛠️ Tecnologias Utilizadas

*   **Backend:** .NET / C#
*   **Banco de Dados:** SQLite (com Entity Framework Core)
*   **Frontend:** Angular
*   **Gerenciador de Pacotes:** NPM / Node.js

---

## 📁 Estrutura do Repositório

```text
sistema-gerenciamento-tarefas2/
├── back/         # Código fonte do Backend (.NET)
├── front/        # Código fonte do Frontend (Angular)
└── README.md     # Documentação do projeto
```

---

## 🚀 Como Executar o Projeto

Para rodar a aplicação completa na sua máquina local, você precisará de **dois terminais abertos simultaneamente**: um para a API e outro para o Frontend.

### 1. Configurando o Banco de Dados e Rodando o Backend (API)

O backend utiliza o **SQLite**. Abra o primeiro terminal na raiz do projeto e execute:

```bash
# 1. Navegue até a pasta da API
cd back

# 2. Execute o servidor do backend
dotnet run
```
> 💡 *A API inicializará no endereço `http://localhost:5050` (Swagger disponível em `http://localhost:5050/swagger`).*

---

### 2. Configurando e Rodando o Frontend (Angular)

Abra um segundo terminal na raiz do projeto e execute os seguintes comandos:

```bash
# 1. Navegue até a pasta do frontend
cd front

# 2. Instale todas as dependências necessárias do projeto
npm install

# 3. Inicie o servidor de desenvolvimento do Angular
npm start
# ou: ng serve
```
> 🌐 *Após a inicialização do Angular, abra o seu navegador e acesse: **`http://localhost:4200`***

---

## 🛑 Pré-requisitos mínimos

Antes de começar, certifique-se de ter instalado em sua máquina:
*   [SDK do .NET](https://microsoft.com)
*   [Node.js & NPM](https://nodejs.org)
*   [Angular CLI](https://angular.io) (instalado globalmente via `npm install -g @angular/cli`)

---

## 💜 Agradecimentos

Projeto desenvolvido com fins educacionais sob a mentoria e apoio da comunidade **WomakersCode** 🚀
