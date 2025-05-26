
# ✅ Desafio Técnico - Amcom - Ailos

## 📋 Descrição

Neste repositório contém as implementações/soluções para as **Questões 1, 2 , 3, 4 e 5** do desafio técnico, que consiste nos 5 exercícios abaixo:

## ✅ Questões

### ✅ Exercício 1

Foi desenvolvida a classe **Consulta de saldo** para ser consumida pelo **Program.cs**, classe foi construída seguindo as seguintes regras:


--**O número da conta é imutável depois de criada**

--**O nome do titular pode ser alterado**
--**O saldo não pode ser alterado livremente -> ele só aumenta com depósitos e só diminui com saques.**
--**Para cada saque, a instituição cobra uma taxa de $3.50**
--**A conta pode ficar com saldo negativo**

### ✅ Exercício 2

Implementei um programa em C# que faz **chamadas HTTP para a API disponibilizada**, buscando as partidas por ano e por time, e calculando o total de gols marcados.

Fluxo:
--**O programa consulta a API para quando o time joga como team1 e team2**
--**Soma os gols marcados nas duas posições**

Usei a biblioteca **HttpClient** para fazer as requisições e **System.Text.Json** para deserializar os dados.

### ✅ Exercício 3

Ao final da sequência de comandos informada, o único arquivo presente no diretório (além do README.md) será	:
➡️ **style.css**

A resposta correta é:
**[ X ] style.css, apenas**

### ✅ Exercício 4

Implementei o comando SQL conforme solicitado:

Ele agrupa os **assuntos e anos**, contando a quantidade de **ocorrências** de cada um, e filtra apenas os que têm mais de 3 ocorrências.

Além disso, a consulta ordena os registros por:
--**Ano, de forma decrescente.**
--**Quantidade, também de forma decrescente.**

✅ Comando SQL:
```
SELECT 
    assunto, 
    ano, 
    COUNT(*) AS quantidade
FROM 
    atendimentos
GROUP BY 
    assunto, 
    ano
HAVING 
    COUNT(*) > 3
ORDER BY 
    ano DESC, 
    quantidade DESC;
```
### ✅ Exercício 5

✅ Criar uma **API REST** com:
- **Consulta de saldo** de conta corrente.
- **Movimentação de conta** (débito e crédito), com controle de **idempotência**.

✅ Utilizando as seguintes tecnologias e padrões:
- **ASP.NET Core** (.NET 6).
- **MediatR** para implementar **CQRS**.
- **Dapper** para acessar o banco de dados **SQLite**.
- **Testes unitários** com **xUnit**, **NSubstitute** e **FluentAssertions**.

API feita com ASP.NET Core, MediatR, Dapper e SQLite. Implementa as funcionalidades de movimentação e consulta de conta corrente, com controle de idempotência para não permitir a mesma movimentação duas vezes.

Os testes foram focados na camada que contém as regras de negócio, garantindo que os fluxos de movimentação e consulta funcionem como esperado.

## 🚀 Tecnologias utilizadas

- ✅ ASP.NET Core 6
- ✅ MediatR
- ✅ Dapper
- ✅ SQLite
- ✅ xUnit
- ✅ NSubstitute
- ✅ FluentAssertions
- ✅ Swagger

## 📂 Estrutura do Projeto

```
├── Application/       # Camada de aplicação: Commands, Queries e Handlers
├── Domain/            # Entidades, Enums e Exceptions
├── Infrastructure/    # Persistência de dados: CommandStore e QueryStore
├── Tests/             # Testes unitários dos Handlers
└── Program.cs         # Configuração do ASP.NET Core
```

## ✅ Como executar a aplicação

1. Clone o repositório:

```bash
git clone https://github.com/wesleylg01/desafio-tecnico-AMcom.git
```

2. Restaure as dependências:

```bash
dotnet restore
```

3. Execute a aplicação:

```bash
dotnet run
```

4. Acesse o Swagger:

```
https://localhost:7140/swagger/
```

## 🧪 Testes

Para rodar os testes:

```bash
dotnet test
```

Os testes foram feitos usando xUnit, NSubstitute e FluentAssertions.

## 🚀 Melhorias Futuras

- Criar testes de integração com SQLite in-memory.
- Adicionar validações automáticas com FluentValidation.
- Padronizar ainda mais as respostas de erro.
- Implementar autenticação e autorização.
