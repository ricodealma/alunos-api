-- Add Users table for authentication
CREATE TABLE IF NOT EXISTS "Users" (
    "Id" UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    "Email" VARCHAR(255) NOT NULL UNIQUE,
    "PasswordHash" VARCHAR(255) NOT NULL,
    "CreatedAt" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "UpdatedAt" TIMESTAMP NULL,
    "IsActive" BOOLEAN NOT NULL DEFAULT true
);

CREATE INDEX IF NOT EXISTS "IX_Users_Email" ON "Users"("Email");

-- Seed test user
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
