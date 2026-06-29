.PHONY: build test lint clean run validate demo init install coverage setup help \
        test-unit test-integration test-e2e lint-fix watch docker-build \
        restore setup-hooks check-structure

# === BUILD ===

build:
	dotnet build src/AiCompany.sln --nologo -v q

build-release:
	dotnet build src/AiCompany.sln --configuration Release --nologo -v q

restore:
	dotnet restore src/AiCompany.sln --nologo

# === TEST ===

test: test-unit test-integration

test-unit:
	dotnet test src/AiCompany.Tests.Unit --nologo --no-restore

test-integration:
	dotnet test src/AiCompany.Tests.Integration --nologo --no-restore

test-e2e:
	dotnet test src/AiCompany.Tests.E2E --nologo --no-restore

test-all:
	dotnet test src/AiCompany.sln --nologo

coverage:
	dotnet test src/AiCompany.Tests.Unit --nologo \
		--collect:"XPlat Code Coverage" \
		--settings coverlet.runsettings
	dotnet test src/AiCompany.Tests.Integration --nologo \
		--collect:"XPlat Code Coverage" \
		--settings coverlet.runsettings
	reportgenerator -reports:*/TestResults/*/coverage.cobertura.xml \
		-targetdir:coverage -reporttypes:Html

# === LINT ===

lint:
	dotnet format --verify-no-changes --nologo

lint-fix:
	dotnet format --nologo

# === CLEAN ===

clean:
	dotnet clean --nologo
	rm -rf src/**/bin src/**/obj
	rm -rf coverage TestResults

# === RUN ===

run:
	dotnet run --project src/AiCompany.Cli --no-build

run-idea-to-prd:
	dotnet run --project src/AiCompany.Cli -- run idea-to-prd

# === VALIDATE ===

validate:
	dotnet run --project src/AiCompany.Cli -- validate

# === SETUP ===

setup: restore setup-hooks
	@echo "✓ Setup complete. Run 'make validate' to check configs."

setup-hooks:
	git config core.hooksPath .githooks

# === AGENT FILE CHECK ===

check-structure:
	@scripts/check-agent-files.sh

# === WATCH ===

watch:
	dotnet watch --project src/AiCompany.Cli

# === DOCKER ===

docker-build:
	docker build -t ai-company -f deploy/Dockerfile .

# === HELP ===

help:
	@echo "╔══════════════════════════════════════════╗"
	@echo "║  AiCompany — Make Targets                ║"
	@echo "╠══════════════════════════════════════════╣"
	@echo "║ build          Build all projects        ║"
	@echo "║ test           Run unit + integration    ║"
	@echo "║ test-all       Run all tests             ║"
	@echo "║ lint           Check code formatting     ║"
	@echo "║ lint-fix       Auto-fix formatting       ║"
	@echo "║ clean          Remove build artifacts    ║"
	@echo "║ validate       Validate AI configs       ║"
	@echo "║ setup          Install dependencies      ║"
	@echo "║ coverage       Generate coverage report  ║"
	@echo "║ run            Run CLI                   ║"
	@echo "║ watch          Dev hot-reload            ║"
	@echo "║ docker-build   Build container           ║"
	@echo "╚══════════════════════════════════════════╝"
