#!/bin/bash

# Script para build e push das imagens Docker

echo "=== Build e Push das Imagens Alunos System ==="
echo ""

# Configurações
DOCKER_USER="ricodealma"
API_IMAGE="${DOCKER_USER}/alunos-api"
WEB_IMAGE="${DOCKER_USER}/alunos-web"
VERSION="latest"

# Cores para output
GREEN='\033[0;32m'
BLUE='\033[0;34m'
RED='\033[0;31m'
NC='\033[0m' # No Color

echo -e "${BLUE}1. Building API image...${NC}"
cd "$(dirname "$0")"
docker build -t ${API_IMAGE}:${VERSION} . || {
    echo -e "${RED}Erro ao construir imagem da API${NC}"
    exit 1
}
echo -e "${GREEN}✓ API image built successfully${NC}"
echo ""

echo -e "${BLUE}2. Building Frontend image...${NC}"
cd ../alunos-web/frontend
docker build -t ${WEB_IMAGE}:${VERSION} . || {
    echo -e "${RED}Erro ao construir imagem do Frontend${NC}"
    exit 1
}
echo -e "${GREEN}✓ Frontend image built successfully${NC}"
echo ""

echo -e "${BLUE}3. Pushing API image to Docker Hub...${NC}"
docker push ${API_IMAGE}:${VERSION} || {
    echo -e "${RED}Erro ao fazer push da imagem da API${NC}"
    echo -e "${RED}Execute: docker login${NC}"
    exit 1
}
echo -e "${GREEN}✓ API image pushed successfully${NC}"
echo ""

echo -e "${BLUE}4. Pushing Frontend image to Docker Hub...${NC}"
docker push ${WEB_IMAGE}:${VERSION} || {
    echo -e "${RED}Erro ao fazer push da imagem do Frontend${NC}"
    exit 1
}
echo -e "${GREEN}✓ Frontend image pushed successfully${NC}"
echo ""

echo -e "${GREEN}=== Todas as imagens foram construídas e enviadas com sucesso! ===${NC}"
echo ""
echo "Para executar o sistema completo:"
echo "  cd $(dirname "$0")"
echo "  docker-compose up -d"
echo ""
echo "Imagens disponíveis:"
echo "  - ${API_IMAGE}:${VERSION}"
echo "  - ${WEB_IMAGE}:${VERSION}"
