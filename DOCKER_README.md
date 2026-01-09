# Alunos System - Docker Deployment

Sistema completo de gestão de alunos com Backend (.NET 8), Frontend (Next.js 16) e PostgreSQL.

## 🚀 Início Rápido

### Executar o sistema completo

```bash
docker-compose up -d
```

Isso iniciará:
- ✅ PostgreSQL (porta 5432)
- ✅ API Backend (porta 5000)
- ✅ Frontend Web (porta 3000)

### Acessar a aplicação

- **Frontend**: http://localhost:3000
- **API**: http://localhost:5000
- **Swagger**: http://localhost:5000/swagger

### Credenciais de teste

- **Email**: admin@example.com
- **Senha**: admin123

## 📦 Serviços

### PostgreSQL
- **Container**: alunos_postgres
- **Imagem**: postgres:15
- **Porta**: 5432
- **Database**: alunos_db
- **User**: postgres
- **Password**: postgres

### API Backend
- **Container**: alunos-api
- **Imagem**: rafaeldev/alunos-api:latest
- **Porta**: 5000 (mapeada para 8080 internamente)
- **Tecnologia**: .NET 8, ASP.NET Core Minimal APIs
- **Autenticação**: JWT Bearer

### Frontend Web
- **Container**: alunos-web
- **Imagem**: rafaeldev/alunos-web:latest
- **Porta**: 3000
- **Tecnologia**: Next.js 16, React 19, TypeScript

## 🛠️ Comandos Úteis

### Ver logs
```bash
# Todos os serviços
docker-compose logs -f

# Apenas API
docker-compose logs -f alunos-api

# Apenas Frontend
docker-compose logs -f alunos-web

# Apenas Database
docker-compose logs -f postgres
```

### Parar serviços
```bash
docker-compose down
```

### Parar e remover volumes (dados do banco)
```bash
docker-compose down -v
```

### Recriar serviços
```bash
docker-compose up -d --force-recreate
```

### Atualizar imagens
```bash
docker-compose pull
docker-compose up -d
```

## 🔧 Build e Push das Imagens

### Opção 1: Script automatizado (Linux/Mac)
```bash
chmod +x docker-build-push.sh
./docker-build-push.sh
```

### Opção 2: Comandos manuais

#### Build das imagens
```bash
# API
docker build -t rafaeldev/alunos-api:latest .

# Frontend
cd ../alunos-web/frontend
docker build -t rafaeldev/alunos-web:latest .
```

#### Push para Docker Hub
```bash
# Login (primeiro acesso)
docker login

# Push API
docker push rafaeldev/alunos-api:latest

# Push Frontend
docker push rafaeldev/alunos-web:latest
```

## 🔐 Variáveis de Ambiente

### API (alunos-api)
- `APP_ENV`: DEV
- `SQL_SERVER`: postgres
- `SQL_USER`: postgres
- `SQL_PASSWORD`: postgres
- `SQL_DATABASE`: alunos_db
- `JwtSettings__SecretKey`: Chave secreta JWT (mínimo 32 caracteres)
- `JwtSettings__Issuer`: alunos-api
- `JwtSettings__Audience`: alunos-web
- `JwtSettings__ExpirationHours`: 24
- `AllowedOrigins__0`: http://localhost:3000
- `AllowedOrigins__1`: http://alunos-web:3000

### Frontend (alunos-web)
- `NEXT_PUBLIC_API_URL`: http://localhost:5000/api
- `NODE_ENV`: production

## 📊 Healthcheck

```bash
# API
curl http://localhost:5000/health

# Database
docker exec alunos_postgres pg_isready -U postgres
```

## 🗄️ Banco de Dados

### Conectar ao PostgreSQL
```bash
docker exec -it alunos_postgres psql -U postgres -d alunos_db
```

### Tabelas criadas automaticamente
- `Users` - Usuários do sistema (autenticação)
- `aluno` - Alunos cadastrados
- `usuario` - Usuários legados (compatibilidade)

### Dados de exemplo
O banco é inicializado com:
- 1 usuário admin (admin@example.com / admin123)
- 5 alunos de exemplo

## 🔍 Troubleshooting

### Porta já em uso
```bash
# Verificar o que está usando a porta
netstat -ano | grep :3000
netstat -ano | grep :5000
netstat -ano | grep :5432

# Alterar portas no docker-compose.yml
```

### Rebuild sem cache
```bash
docker-compose build --no-cache
docker-compose up -d
```

### Limpar tudo e recomeçar
```bash
docker-compose down -v
docker system prune -a
docker-compose up -d
```

## 📝 Notas

- As imagens estão disponíveis publicamente no Docker Hub
- O banco de dados PostgreSQL tem um volume persistente
- O script de inicialização (`init.sql`) roda automaticamente na primeira vez
- A API aguarda o PostgreSQL estar "healthy" antes de iniciar
- O Frontend aguarda a API estar disponível

## 🏗️ Arquitetura

```
┌─────────────────┐
│   Frontend      │
│   (Next.js)     │
│   Port: 3000    │
└────────┬────────┘
         │
         │ HTTP
         │
┌────────▼────────┐
│   Backend API   │
│   (.NET 8)      │
│   Port: 5000    │
└────────┬────────┘
         │
         │ SQL
         │
┌────────▼────────┐
│   PostgreSQL    │
│   Port: 5432    │
└─────────────────┘
```

## 📄 Licença

Este projeto é de uso educacional.
