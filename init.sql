-- Inicialização do banco de dados para Alunos API

-- Feedback de início
DO $$ BEGIN
    RAISE NOTICE 'Iniciando o seed do banco de dados...';
END $$;

-- Tabela de usuários para autenticação (Unificada)
CREATE TABLE IF NOT EXISTS usuario (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    username VARCHAR(100), -- Opcional, mantido para compatibilidade
    email VARCHAR(255) NOT NULL UNIQUE,
    password_hash VARCHAR(255) NOT NULL,
    nome VARCHAR(255), -- Opcional
    ativo BOOLEAN NOT NULL DEFAULT TRUE,
    data_criacao TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    data_atualizacao TIMESTAMP NULL
);

CREATE INDEX IF NOT EXISTS "IX_usuario_email" ON usuario(email);

-- Tabela de alunos
CREATE TABLE IF NOT EXISTS aluno (
    id UUID PRIMARY KEY,
    nome VARCHAR(255) NOT NULL,
    email VARCHAR(255) NOT NULL UNIQUE,
    serie VARCHAR(100) NOT NULL
);

-- Inserir usuário Admin (Senha: admin123)
INSERT INTO usuario (id, username, email, password_hash, nome, ativo, data_criacao)
VALUES (
    gen_random_uuid(),
    'admin',
    'admin@example.com',
    '$2a$11$OI9H1I1o3PXhYcgGrz8fX.cNaffcsCBFt5FC/saUZAxdBOYLNFOSO',
    'Administrador',
    true,
    NOW()
)
ON CONFLICT (email) DO NOTHING;

-- Feedback de usuário criado
DO $$ BEGIN
    RAISE NOTICE 'Usuário admin@example.com criado/verificado.';
END $$;

-- Inserir dados de exemplo de alunos
INSERT INTO aluno (id, nome, email, serie) VALUES
    (gen_random_uuid(), 'João Silva', 'joao.silva@example.com', '5ª Série'),
    (gen_random_uuid(), 'Maria Santos', 'maria.santos@example.com', '6ª Série'),
    (gen_random_uuid(), 'Pedro Oliveira', 'pedro.oliveira@example.com', '7ª Série'),
    (gen_random_uuid(), 'Ana Costa', 'ana.costa@example.com', '5ª Série'),
    (gen_random_uuid(), 'Carlos Ferreira', 'carlos.ferreira@example.com', '8ª Série')
ON CONFLICT (email) DO NOTHING;

-- Feedback final
DO $$ BEGIN
    RAISE NOTICE 'Seed do banco de dados concluído com sucesso!';
END $$;
