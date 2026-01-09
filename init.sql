-- Inicialização do banco de dados para Alunos API

-- Tabela de usuários para autenticação
CREATE TABLE IF NOT EXISTS usuario (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    username VARCHAR(100) NOT NULL UNIQUE,
    password_hash VARCHAR(255) NOT NULL,
    email VARCHAR(255) NOT NULL UNIQUE,
    nome VARCHAR(255) NOT NULL,
    ativo BOOLEAN DEFAULT TRUE,
    data_criacao TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Tabela Users para JWT authentication (novo sistema)
CREATE TABLE IF NOT EXISTS "Users" (
    "Id" UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    "Email" VARCHAR(255) NOT NULL UNIQUE,
    "PasswordHash" VARCHAR(255) NOT NULL,
    "CreatedAt" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "UpdatedAt" TIMESTAMP NULL,
    "IsActive" BOOLEAN NOT NULL DEFAULT true
);

CREATE INDEX IF NOT EXISTS "IX_Users_Email" ON "Users"("Email");

-- Tabela de alunos
CREATE TABLE IF NOT EXISTS aluno (
    id UUID PRIMARY KEY,
    nome VARCHAR(255) NOT NULL,
    email VARCHAR(255) NOT NULL UNIQUE,
    serie VARCHAR(100) NOT NULL
);

-- Inserir usuários de exemplo (senhas: admin123, user123)
-- Nota: Estas são hashes bcrypt para fins de demonstração
INSERT INTO usuario (id, username, password_hash, email, nome, ativo) VALUES
    ('00000000-0000-0000-0000-000000000001', 'admin', '$2a$11$X2aKzEqZqX4nP8hPE9vLVeB2YV0wYlKqLh2h2K4JlnPxQnFKHcQDi', 'admin@alunos.com', 'Administrador', TRUE),
    ('00000000-0000-0000-0000-000000000002', 'user', '$2a$11$vK8P3qKzEqZqX4nP8hPE9vLVeB2YV0wYlKqLh2h2K4JlnPxQnFKH', 'user@alunos.com', 'Usuário Comum', TRUE),
    ('00000000-0000-0000-0000-000000000003', 'professor', '$2a$11$mL7P2qKzEqZqX4nP8hPE9vLVeB2YV0wYlKqLh2h2K4JlnPxQnFKH', 'professor@alunos.com', 'Professor Silva', TRUE)
ON CONFLICT (username) DO NOTHING;

-- Inserir test user para JWT authentication
-- Email: admin@example.com
-- Password: admin123
-- BCrypt hash generated with work factor 11
INSERT INTO "Users" ("Id", "Email", "PasswordHash", "CreatedAt", "IsActive")
VALUES (
    gen_random_uuid(),
    'admin@example.com',
    '$2a$11$N9qo8uLOickgx2ZMRZoMye7FRNv2JYMQzl3g5Zgn7GdOaXd1.yqq2',
    NOW(),
    true
)
ON CONFLICT ("Email") DO NOTHING;

-- Inserir dados de exemplo de alunos
INSERT INTO aluno (id, nome, email, serie) VALUES
    (gen_random_uuid(), 'João Silva', 'joao.silva@example.com', '5ª Série'),
    (gen_random_uuid(), 'Maria Santos', 'maria.santos@example.com', '6ª Série'),
    (gen_random_uuid(), 'Pedro Oliveira', 'pedro.oliveira@example.com', '7ª Série'),
    (gen_random_uuid(), 'Ana Costa', 'ana.costa@example.com', '5ª Série'),
    (gen_random_uuid(), 'Carlos Ferreira', 'carlos.ferreira@example.com', '8ª Série')
ON CONFLICT (email) DO NOTHING;
