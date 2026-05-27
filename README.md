# GatoLock 🐱🔒

## Integrantes

- Ana Clara Maiberg
- Jefferson Ademir Zimmermann

---

## Tema

Sistema para gerenciamento de solicitações de adoção de gatos.
O objetivo é permitir que usuários visualizem gatos disponíveis para adoção e realizem solicitações de interesse de forma organizada.

---

## Tecnologias Utilizadas

### Frontend
- HTML5
- CSS3
- JavaScript
- Bootstrap

### Backend
- ASP.NET Core Web API (C#)

### Ferramentas
- Git
- GitHub
- Visual Studio Code
- Visual Studio

---

## Arquitetura do Sistema

O sistema seguirá uma arquitetura cliente-servidor.

```text
Frontend (HTML/CSS/JS)
          ↓
      ASP.NET API
          ↓
    Regras de negócio
          ↓
    Serviços do sistema
```

Nesta primeira etapa o frontend utiliza dados simulados (mockados) em JavaScript.

---

## Organização das Pastas

```text
GatoLock
│
├── GatoLockAPI
│   ├── Properties
│   ├── wwwroot
│   │   ├── assets
│   │   ├── app.js
│   │   ├── index.html
│   │   └── style.css
│   │
│   ├── Program.cs
│   └── GatoLockAPI.csproj
│
└── GatoLock.sln
```

---

## Fluxo Geral do Sistema

1. Usuário acessa a página.
2. Visualiza os gatos disponíveis.
3. Seleciona um gato.
4. Preenche o formulário de adoção.
5. Solicitação é enviada ao sistema.
6. Solicitação entra em análise.
7. Usuário recebe o resultado da adoção.

