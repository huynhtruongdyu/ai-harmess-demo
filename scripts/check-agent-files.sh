#!/bin/bash
# Validate that all 13 agents have the required 5 files

set -e

AGENTS_DIR=".ai/agents"
REQUIRED_FILES=("README.md" "system.md" "rules.md" "examples.md" "config.yaml")
ALL_OK=true
COUNT=0

echo "Checking agent files..."
echo ""

for agent_dir in "$AGENTS_DIR"/*/; do
    agent_name=$(basename "$agent_dir")
    COUNT=$((COUNT + 1))
    MISSING=()

    for file in "${REQUIRED_FILES[@]}"; do
        if [ ! -f "$agent_dir$file" ]; then
            MISSING+=("$file")
        fi
    done

    if [ ${#MISSING[@]} -eq 0 ]; then
        echo "  ✓ $agent_name"
    else
        echo "  ✗ $agent_name (missing: ${MISSING[*]})"
        ALL_OK=false
    fi
done

echo ""
echo "Checked $COUNT agents."

if [ "$ALL_OK" = true ]; then
    echo "All agent files valid."
    exit 0
else
    echo "Some agents have missing files."
    exit 1
fi
