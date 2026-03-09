# Curso-Cs (Aplicação Bancária)

Aplicação de console em C# que simula um sistema bancário simples para fins de estudo. Suporta clientes com múltiplas contas (corrente e poupança), operações de saque/depósito e persistência em JSON.

Recursos principais
- Login de cliente (usuário + senha)
- Suporte a `ContaCorrente` e `ContaPoupanca`
- Saque (com taxa para conta corrente) e depósito
- Simulação de rendimento para poupança
- Persistência em `ConsoleApp1/Resources/banco_dados.json` (o aplicativo lê/grava diretamente esse arquivo)
- Usuário Admin padrão: `Admin` / `123` (caso exista no JSON)

Requisitos
- .NET SDK (versão usada no projeto: `net8.0`)

Como clonar
- Verifique se o Git está instalado: `git --version`
- Via HTTPS:
  - `git clone https://github.com/Amaral-Gabriel/Curso-Cs.git`
- Via SSH (se configurado):
  - `git clone git@github.com:Amaral-Gabriel/Curso-Cs.git`

Após clonar:
- `cd Curso-Cs`
- Abra no VS Code: `code .` ou na IDE de sua preferência.

Como rodar
1. Pela CLI (recomendado):
   - Na raiz do repositório, execute:
     - `dotnet run --project .\ConsoleApp1`
   - Ou entre na pasta do projeto e rode:
     - `cd ConsoleApp1`
     - `dotnet run`

2. No Visual Studio:
   - Abra a solução `Curso-Cs` e defina `ConsoleApp1` como projeto de inicialização.
   - Execute com Debug (F5) ou sem Debug (Ctrl+F5).

Observações sobre o arquivo de dados
- O arquivo de dados do projeto fica em `ConsoleApp1/Resources/banco_dados.json`.
- A aplicação foi configurada para ler e gravar diretamente nesse arquivo na pasta do projeto (não usa cópia em `bin/Debug`), portanto alterações são persistidas diretamente lá.
- Evite manter várias cópias do `banco_dados.json` para não ter divergências.

Estrutura principal dos arquivos
- `Program.cs` — ponto de entrada e fluxo de login.
- `Menu.cs` — interface de console e navegação.
- `Cliente.cs` — modelo de cliente e listas de contas.
- `Conta.cs`, `ContaCorrente.cs`, `ContaPoupanca.cs` — modelos de conta e operações.
- `BancoDeDados.cs` — leitura/escrita do JSON.

Contribuição
- Pull requests e issues são bem-vindos.

Usuários de teste (para login rápido)
- `Admin` / `123` — Conta Corrente Nº 1001 — Saldo: 10000
- `Gabriel` / `1601` — Conta Corrente Nº 2001 — Saldo: 30000
- `Vorcado` / `171` — Conta Corrente Nº 3001 — Saldo: 20000000
- `Alexandre de Moraes` / `456` — Conta Poupança Nº 4001 (Saldo: 500) e Conta Corrente Nº 4002 (Saldo: 30000)

Observações
- Os logins são sensíveis a maiúsculas e espaços. Digite exatamente como na lista ou remova espaços acidentais.
- O arquivo de dados está em `ConsoleApp1/Resources/banco_dados.json`. Alterações realizadas na aplicação serão gravadas nesse arquivo.
