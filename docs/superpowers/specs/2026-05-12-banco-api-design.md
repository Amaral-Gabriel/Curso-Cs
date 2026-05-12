# Design — BancoAPI

**Data:** 2026-05-12  
**Tipo:** Projeto educacional — ASP.NET Core Web API  
**Contexto:** Trabalho de curso cobrindo REST, JWT, EF Core, Swagger, Bootstrap

---

## Visão Geral

API REST de banco simples com dois tipos de conta (Corrente e Poupança), operações de depósito, saque e PIX, autenticação JWT com perfis Cliente e Admin, banco de dados MySQL via EF Core, e frontend Bootstrap consumindo a API.

---

## Estrutura do Projeto

```
BancoAPI/
├── Controllers/
│   ├── AuthController.cs
│   ├── ClientesController.cs
│   ├── ContasController.cs
│   └── TransacoesController.cs
├── Models/
│   ├── Cliente.cs
│   ├── Conta.cs
│   ├── ContaCorrente.cs
│   ├── ContaPoupanca.cs
│   └── Transacao.cs
├── DTOs/
│   ├── LoginDto.cs
│   ├── RegisterDto.cs
│   ├── ClienteDto.cs
│   ├── ContaDto.cs
│   └── TransacaoDto.cs
├── Services/
│   ├── AuthService.cs
│   ├── ClienteService.cs
│   ├── ContaService.cs
│   └── TransacaoService.cs
├── Repositories/
│   ├── IClienteRepository.cs
│   ├── ClienteRepository.cs
│   ├── IContaRepository.cs
│   └── ContaRepository.cs
├── Data/
│   └── BancoContext.cs
├── wwwroot/
│   ├── index.html
│   ├── dashboard.html
│   └── admin.html
└── Program.cs
```

---

## Entidades

### Cliente
- `Id` int PK
- `Nome` string
- `Email` string (único)
- `SenhaHash` string (bcrypt)
- `Perfil` string ("Cliente" ou "Admin")
- `Contas` List<Conta>

### Conta (base para herança por TPH)
- `Id` int PK
- `NumeroConta` string (gerado automaticamente: 6 dígitos)
- `Tipo` string ("Corrente" ou "Poupanca")
- `Saldo` decimal
- `TaxaSaque` decimal (ContaCorrente: R$2,50 fixo; ContaPoupança: 0)
- `TaxaJuros` decimal (ContaPoupança: 0.005 = 0,5%/dia; ContaCorrente: 0)
- `UltimoRendimento` DateTime (ContaPoupança: controla quando aplicar juros)
- `ClienteId` int FK

### Transacao
- `Id` int PK
- `Tipo` string ("Deposito", "Saque", "Pix")
- `Valor` decimal
- `DataHora` DateTime
- `ContaOrigemId` int FK
- `ContaDestinoId` int? FK (null se não for Pix)

---

## Endpoints

### Auth (público)
| Método | Rota | Status |
|--------|------|--------|
| POST | `/api/auth/login` | 200 / 401 |
| POST | `/api/auth/register` | 201 / 400 |

### Clientes (Admin)
| Método | Rota | Status |
|--------|------|--------|
| GET | `/api/clientes` | 200 |
| GET | `/api/clientes/{id}` | 200 / 404 |
| PUT | `/api/clientes/{id}` | 200 / 404 |
| DELETE | `/api/clientes/{id}` | 204 / 404 |

### Contas (Cliente autenticado)
| Método | Rota | Status |
|--------|------|--------|
| GET | `/api/contas` | 200 |
| GET | `/api/contas/{id}` | 200 / 404 |
| POST | `/api/contas` | 201 / 400 |
| DELETE | `/api/contas/{id}` | 204 / 404 |

### Transações (Cliente autenticado)
| Método | Rota | Status |
|--------|------|--------|
| POST | `/api/transacoes/deposito` | 200 / 400 |
| POST | `/api/transacoes/saque` | 200 / 400 |
| POST | `/api/transacoes/pix` | 200 / 400 / 404 |
| GET | `/api/transacoes/{contaId}` | 200 / 404 |

---

## Regras de Negócio

- **ContaCorrente — Saque:** desconta `TaxaSaque` (R$2,50) além do valor solicitado. Saldo deve cobrir valor + taxa.
- **ContaPoupança — Juros:** a cada requisição na conta, se `UltimoRendimento.Date < DateTime.Today`, aplica `Saldo *= (1 + TaxaJuros)` e atualiza `UltimoRendimento`.
- **PIX:** transferência entre contas pelo `NumeroConta` de destino. Valida saldo suficiente antes de transferir.
- **NumeroConta:** gerado com 6 dígitos aleatórios únicos no momento de criação da conta.

---

## Segurança

- JWT com claims: `Id`, `Email`, `Perfil`
- Validade: 8 horas
- Secret key em `appsettings.json`
- `[Authorize(Roles = "Admin")]` — endpoints de clientes
- `[Authorize]` — endpoints de contas e transações
- Cliente só acessa suas próprias contas (validação no service pelo Id do token)

---

## Swagger

- Habilitado em desenvolvimento
- Suporte a Bearer JWT no Swagger UI
- Todos os endpoints com descrição e exemplos de resposta

---

## Frontend (Bootstrap 5 via CDN)

### `index.html` — Login / Registro
- Duas abas: Login e Cadastro
- Formulários simples com validação básica
- Salva JWT no `localStorage` e redireciona conforme perfil

### `dashboard.html` — Painel do Cliente
- Header com nome + logout
- Cards com saldo por conta
- Formulários: Depósito, Saque, PIX
- Tabela de extrato
- Botão abrir nova conta

### `admin.html` — Painel Admin
- Tabela de clientes com editar (modal PUT) e excluir (DELETE)
- Tabela de todas as contas

JavaScript puro com `fetch` — sem frameworks.

---

## Stack

- **Backend:** ASP.NET Core Web API .NET 8
- **ORM:** Entity Framework Core + Migrations
- **Banco:** MySQL (Pomelo.EntityFrameworkCore.MySql)
- **Auth:** Microsoft.AspNetCore.Authentication.JwtBearer
- **Hash:** BCrypt.Net-Next
- **Docs:** Swashbuckle (Swagger)
- **Frontend:** HTML + CSS + Bootstrap 5 + JS puro
