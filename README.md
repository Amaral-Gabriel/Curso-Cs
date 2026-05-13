# Banco Master

API REST de banco digital com operações de depósito, saque e PIX. Construída em ASP.NET Core com banco de dados MySQL e autenticação JWT.

## Requisitos

- [.NET SDK 10](https://dotnet.microsoft.com/download)
- MySQL 8.0 com usuário `root` e senha `cimatec`

## Como rodar

1. Clone o repositório:
   ```
   git clone https://github.com/Amaral-Gabriel/Curso-Cs.git
   cd Curso-Cs
   ```

2. Crie o banco de dados:
   ```
   dotnet ef database update --project BancoAPI
   ```

3. Rode a API:
   ```
   dotnet run --project BancoAPI
   ```

4. Acesse no navegador: `http://localhost:5118`

> O MySQL deve estar rodando com usuário `root` e senha `cimatec`. Se sua senha for diferente, altere em `BancoAPI/appsettings.json`.

## Endpoints principais

| Método | Rota | Descrição |
|--------|------|-----------|
| POST | `/api/auth/register` | Cadastro de usuário |
| POST | `/api/auth/login` | Login (retorna token JWT) |
| GET | `/api/contas` | Listar contas do usuário |
| POST | `/api/transacoes/deposito` | Depósito |
| POST | `/api/transacoes/saque` | Saque |
| POST | `/api/transacoes/pix` | Transferência PIX |

Documentação completa disponível em `http://localhost:5118/swagger`.
