#!/bin/bash

# Generate OpenAPI specification from running API
# Usage: ./scripts/generate-openapi.sh

set -e

# Colors for output
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
RED='\033[0;31m'
NC='\033[0m' # No Color

echo -e "${YELLOW}Generating OpenAPI specification...${NC}"

# Check if API is running
if ! curl -s -f http://localhost:9090/health > /dev/null; then
    echo -e "${RED}Error: API is not running at http://localhost:9090${NC}"
    echo -e "${YELLOW}Please start the API first with: docker compose up -d${NC}"
    exit 1
fi

# Create out directory if it doesn't exist
mkdir -p out

# Generate OpenAPI JSON
echo -e "${YELLOW}Downloading OpenAPI specification...${NC}"
if curl -s -f http://localhost:9090/openapi/v1.json > ../out/openapi.json; then
    echo -e "${GREEN}✓ OpenAPI specification saved to ../out/openapi.json${NC}"
    
    # Show file info
    FILE_SIZE=$(wc -c < out/openapi.json)
    echo -e "${GREEN}  File size: ${FILE_SIZE} bytes${NC}"
    
    # Pretty print summary
    if command -v jq &> /dev/null; then
        ENDPOINTS=$(jq '.paths | length' out/openapi.json)
        echo -e "${GREEN}  Endpoints documented: ${ENDPOINTS}${NC}"
    fi
else
    echo -e "${RED}Error: Failed to download OpenAPI specification${NC}"
    echo -e "${YELLOW}Make sure the API is running and accessible at http://localhost:9090${NC}"
    exit 1
fi
