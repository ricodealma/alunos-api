# Guia Rápido - Build e Deploy Docker

## Passo 1: Build das Imagens

### API Backend
```bash
cd d:/Rafael/Desktop/Projetos/teste/alunos-api
docker build -t rafaeldev/alunos-api:latest .
```

### Frontend
```bash
cd d:/Rafael/Desktop/Projetos/teste/alunos-web/frontend
docker build -t rafaeldev/alunos-web:latest .
```

## Passo 2: Login no Docker Hub

```bash
docker login
# Digite seu username e password
```

## Passo 3: Push das Imagens

```bash
docker push rafaeldev/alunos-api:latest
docker push rafaeldev/alunos-web:latest
```

## Passo 4: Executar o Sistema Completo

```bash
cd d:/Rafael/Desktop/Projetos/teste/alunos-api
docker-compose up -d
```

## Verificar Status

```bash
docker-compose ps
docker-compose logs -f
```

## Acessar Aplicação

- Frontend: http://localhost:3000
- API: http://localhost:5000
- Swagger: http://localhost:5000/swagger

Login: admin@example.com / admin123

## Comandos Úteis

```bash
# Parar tudo
docker-compose down

# Ver logs
docker-compose logs -f alunos-api
docker-compose logs -f alunos-web
docker-compose logs -f postgres

# Reiniciar um serviço
docker-compose restart alunos-api

# Remover tudo (incluindo volumes)
docker-compose down -v
```

## Troubleshooting

### Erro de porta em uso
Altere as portas no docker-compose.yml

### Rebuild sem cache
```bash
docker-compose build --no-cache
docker-compose up -d
```

### Ver o que está rodando
```bash
docker ps
```
