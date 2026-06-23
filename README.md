# GatoLock 🐱🔒

Sistema distribuído para gerenciamento de solicitações de adoção de gatos, desenvolvido para a disciplina de Programação Concorrente e Distribuída.

---

# Integrantes

- Ana Clara Maiberg
- Jefferson Ademir Zimmermann

---

# Tema

O GatoLock é um sistema que simula o processo de adoção de gatos.

O usuário pode visualizar os animais disponíveis, preencher uma solicitação de adoção e acompanhar seu processamento em tempo real. 

---

# Tecnologias Utilizadas

## Frontend

- HTML5
- CSS3
- JavaScript (ES6)
- Bootstrap 5

## Backend

- ASP.NET Core Web API
- C#
- gRPC
- SignalR

## Comunicação

- REST API
- gRPC
- SignalR (WebSockets)

## Concorrência

- Mutex
- BackgroundService
- Channel<T>

## Ferramentas

- Git
- GitHub
- Visual Studio
- Visual Studio Code
- .NET 10

---

# Arquitetura do Sistema

O projeto utiliza arquitetura Cliente-Servidor com comunicação distribuída utilizando REST e gRPC.

```text
               Frontend
        (HTML / CSS / JavaScript)
                    │
              Requisições REST
                    │
                    ▼
            MensagensController
                    │
                    ▼
           GrpcMensagensGateway
                    │
               Chamada gRPC
                    │
                    ▼
            MensagensGrpcService
                    │
                    ▼
              MensagemService
                    │
                    ▼
         SolicitacoesQueueService
         (Fila protegida por Mutex)
                    │
                    ▼
     ProcessamentoSolicitacoesService
           (Background Service)
                    │
         Atualizações em tempo real
                    │
                    ▼
               SignalR Hub
                    │
                    ▼
                Frontend
```

---

# Funcionalidades Implementadas

- Visualização dos gatos disponíveis
- Cadastro de solicitação de adoção
- Comunicação REST
- Comunicação gRPC
- Consulta de solicitações
- Processamento assíncrono das solicitações
- Atualização em tempo real utilizando SignalR
- Controle de concorrência utilizando Mutex
- Armazenamento temporário em memória
- Fila de processamento das solicitações

---

# Organização das Pastas

```text
GatoLock-PCD
│
├── GatoLockAPI
│
├── Controllers
│   └── MensagensController.cs
│
├── Hubs
│   └── SolicitacoesHub.cs
│
├── Models
│   ├── Mensagem.cs
│   └── SolicitacaoAdocao.cs
│
├── Protos
│   └── mensagens.proto
│
├── Services
│   ├── GrpcMensagensGateway.cs
│   ├── MensagemService.cs
│   ├── MensagensGrpcService.cs
│   ├── ProcessamentoSolicitacoesService.cs
│   └── SolicitacoesQueueService.cs
│
├── wwwroot
│   ├── assets
│   │   └── imagens
│   │
│   ├── app.js
│   ├── style.css
│   └── index.html
│
├── Program.cs
├── appsettings.json
├── GatoLockAPI.csproj
│
└── GatoLock.sln
```

---

# Fluxo Geral do Sistema

1. O usuário acessa a página inicial.
2. O frontend exibe os gatos disponíveis para adoção.
3. O usuário seleciona um gato.
4. O formulário de adoção é preenchido.
5. O frontend envia uma requisição REST.
6. O Controller encaminha a solicitação ao Gateway gRPC.
7. O Gateway realiza uma chamada ao serviço gRPC.
8. O serviço adiciona a solicitação na fila protegida por Mutex.
9. O Background Service processa as solicitações em ordem de chegada.
10. O SignalR notifica o frontend sobre alterações na fila e no processamento.
11. O usuário acompanha em tempo real:
    - Solicitações na fila;
    - Solicitação em processamento;
    - Solicitações concluídas.

---

# Comunicação entre Componentes

## REST

Utilizado pelo frontend para enviar e consultar solicitações.

### Endpoints

- `POST /api/mensagens`
- `GET /api/mensagens`
- `GET /api/mensagens/fila`
- `GET /api/mensagens/grpc`

---

## gRPC

Responsável pela comunicação entre serviços internos.

### Métodos

- `Adicionar()`
- `ObterTodas()`

---

## SignalR

Responsável por atualizar automaticamente o frontend durante o processamento das solicitações.

Eventos enviados:

- `filaAtualizada`
- `solicitacaoProcessando`
- `solicitacaoConcluida`

---

# Controle de Concorrência

Para evitar condições de corrida durante o acesso à fila de solicitações, o sistema utiliza um **Mutex**, garantindo que apenas uma thread por vez possa acessar ou modificar os recursos compartilhados.

Os recursos protegidos são:

- fila de solicitações;
- lista de solicitações em espera;
- lista de solicitações processadas.


✔ Fila de processamento

✔ Armazenamento temporário em memória
