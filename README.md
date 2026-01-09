# Alunos API

API RESTful para gerenciamento de alunos construída com .NET 9.0, seguindo arquitetura limpa (Clean Architecture) e padrões DDD.

## 🏗️ Arquitetura

O projeto está organizado em camadas seguindo os princípios de Clean Architecture:

- **Alunos.Api.Domain**: Entidades, interfaces e lógica de negócio
- **Alunos.Api.Infra**: Implementação de persistência e infraestrutura
- **Alunos.Api.App**: API endpoints e configuração da aplicação

## 🚀 Tecnologias

- .NET 9.0
- Entity Framework Core
- PostgreSQL
- Swagger/OpenAPI
- Docker & Docker Compose

## 📋 Pré-requisitos

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Docker](https://www.docker.com/get-started)
- [Docker Compose](https://docs.docker.com/compose/install/)

## ⚙️ Configuração

### Variáveis de Ambiente

Configure as seguintes variáveis no `appsettings.Development.json`:

```json
{
  "SQL_SERVER": "localhost",
  "SQL_USER": "postgres",
  "SQL_PASSWORD": "postgres",
  "SQL_DATABASE": "alunos_db",
  "X_API_HEADER": "your-secret-header-key"
}
```

## 🐳 Executando com Docker

```bash
docker-compose up -d
```

A API estará disponível em: `http://localhost:8080`

## 💻 Executando Localmente

1. Iniciar o banco de dados: `docker-compose up -d postgres`
2. Restaurar dependências: `dotnet restore`
3. Executar: `cd src/Alunos.Api.App && dotnet run`

## 📚 API Endpoints

- **GET** `/health` - Health Check
- **GET** `/v1/alunos` - Listar alunos (com filtros e paginação)
- **POST** `/v1/alunos` - Criar aluno
- **PUT** `/v1/alunos/{id}` - Atualizar aluno
- **DELETE** `/v1/alunos/{id}` - Deletar aluno

## 📖 Documentação

Acesse o Swagger UI em: `http://localhost:8080`

## Estrutura
- src/: Código fonte da aplicação
- test/: Projetos de teste
