# Banco Master

Projeto da 3ª Sprint do curso de Back-End do Ford Enter.

## Requisitos

- [.NET SDK 10](https://dotnet.microsoft.com/download)
- MySQL 8.0 com usuário `root` e senha `cimatec`

## Como rodar

1. Clone o repositório:
   ```
   git clone https://github.com/Amaral-Gabriel/Curso-Cs.git
   cd Curso-Cs
   ```

2. Restaure as dependências:                                                                                                               
   ```
   dotnet restore BancoAPI/BancoAPI.csproj 
   ```

3. Crie o banco de dados:
   ```
   dotnet ef database update --project BancoAPI
   ```

4. Rode a API:
   ```
   dotnet run --project BancoAPI
   ```

5. Acesse no navegador: `http://localhost:5118`

> O MySQL deve estar rodando com usuário `root` e senha `cimatec`. Se sua senha for diferente, altere em `BancoAPI/appsettings.json`.


## Imagens

<img width="1114" height="570" alt="Captura de tela 2026-05-13 150344" src="https://github.com/user-attachments/assets/9d1912ee-c806-4ad3-ac2b-f3c9b4ec1c08" />
Dashboard principal

---

<img width="1111" height="216" alt="Captura de tela 2026-05-13 150357" src="https://github.com/user-attachments/assets/2b17c6a3-a539-4d32-9e18-6a80c610ba13" />
Extrato

---

<img width="1140" height="582" alt="Captura de tela 2026-05-13 150316" src="https://github.com/user-attachments/assets/6cb648c2-cad1-45ad-aabf-39e388f25330" />
Tela do administrador

---

<img width="1128" height="577" alt="Captura de tela 2026-05-13 145747" src="https://github.com/user-attachments/assets/c5222b7f-4cea-4c45-89df-8cef8aadd15c" />
Tela de login

---

<img width="1131" height="575" alt="Captura de tela 2026-05-13 150412" src="https://github.com/user-attachments/assets/d90ded4a-dc27-4b5b-8ca1-4cb717948dc8" />
Tela de cadastro

---






