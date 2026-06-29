# Hướng dẫn sử dụng — Harness AI Software Company

Nền tảng phát triển phần mềm đa tác nhân tự động, vận hành hoàn toàn bằng AI.

---

## Yêu cầu hệ thống

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Git
- Make (tuỳ chọn, có sẵn trên Linux/macOS; Windows cần cài qua `winget install GnuWin32.Make` hoặc dùng PowerShell)

---

## Cài đặt

```bash
# 1. Clone dự án
git clone <repo-url>
cd ai-harmess-demo

# 2. Cài đặt dependencies + Git hooks
make setup

# 3. Build toàn bộ solution
make build

# (Không có Make? Chạy trực tiếp:)
dotnet restore src/AiCompany.slnx
dotnet build src/AiCompany.slnx --nologo
```

---

## Kiểm tra nhanh

```bash
# Validate toàn bộ cấu hình AI (agents, workflows, routing...)
make validate
# hoặc:
dotnet run --project src/AiCompany.Cli -- validate

# Chạy thử workflow mà không thực thi (dry-run)
dotnet run --project src/AiCompany.Cli -- run bug-fix --dry-run

# Xem danh sách agents / workflows
dotnet run --project src/AiCompany.Cli -- list agents
dotnet run --project src/AiCompany.Cli -- list workflows
```

---

## CLI Commands

### `validate` — Kiểm tra cấu hình

```bash
dotnet run --project src/AiCompany.Cli -- validate
```

Kiểm tra: `config.yaml`, `routing.yaml`, 13 agents, 17 workflows. Báo cáo lỗi + cảnh báo nếu có.

### `run` — Chạy workflow

```bash
dotnet run --project src/AiCompany.Cli -- run <workflow_name> [options]
```

**Tuỳ chọn:**
- `--dry-run` — Kiểm tra mà không thực thi
- `--input key=value` — Truyền tham số đầu vào (có thể dùng nhiều lần)

**Ví dụ:**
```bash
# Dry-run kiểm tra
dotnet run --project src/AiCompany.Cli -- run feature-request --dry-run

# Chạy thật (cần OPENAI_API_KEY)
set OPENAI_API_KEY=sk-...  # Windows PowerShell
export OPENAI_API_KEY=sk-...  # Linux/macOS
dotnet run --project src/AiCompany.Cli -- run bug-fix
```

> **Lưu ý:** Chạy thật cần biến môi trường `OPENAI_API_KEY` hoặc cấu hình trong `.ai/config.yaml`.

### `agent` — Tương tác trực tiếp với agent

```bash
dotnet run --project src/AiCompany.Cli -- agent <name> "<prompt>" [options]
```

**Ví dụ:**
```bash
# Hỏi ý kiến Solution Architect
dotnet run --project src/AiCompany.Cli -- agent solution-architect "Thiết kế kiến trúc cho hệ thống chat real-time"

# Kèm context files
dotnet run --project src/AiCompany.Cli -- agent code-reviewer "Review code này" --context src/AiCompany.Core/Configuration/ConfigLoader.cs
```

### `list` — Liệt kê agents / workflows

```bash
dotnet run --project src/AiCompany.Cli -- list agents
dotnet run --project src/AiCompany.Cli -- list workflows
```

### `status` — Xem trạng thái dự án

```bash
dotnet run --project src/AiCompany.Cli -- status
```

Hiển thị: cấu hình, số lượng agents, workflows, dung lượng memory store.

### `init` — Khởi tạo cấu trúc AI company

```bash
dotnet run --project src/AiCompany.Cli -- init [--force]
```

Tạo thư mục `.ai/` chuẩn nếu chưa có. Dùng `--force` để ghi đè.

---

## Make Targets

| Target | Mô tả |
|--------|-------|
| `make build` | Build toàn bộ solution |
| `make test` | Chạy unit + integration tests |
| `make test-all` | Chạy tất cả tests (unit + integration + e2e) |
| `make validate` | Validate cấu hình AI |
| `make run` | Chạy CLI |
| `make lint` | Kiểm tra code formatting |
| `make lint-fix` | Tự động sửa formatting |
| `make clean` | Xoá build artifacts |
| `make coverage` | Tạo báo cáo code coverage |
| `make setup` | Cài đặt dependencies + Git hooks |
| `make watch` | Dev hot-reload |
| `make docker-build` | Build Docker image |
| `make help` | Hiển thị danh sách targets |

---

## Cấu trúc thư mục

```
.ai/                          # Cấu hình AI platform
  config.yaml                 #   Cấu hình chung (platform, providers, logging...)
  routing.yaml                #   Routing rules (task → agent)
  agents/                     #   13 agents, mỗi agent 1 thư mục
    <agent-name>/
      config.yaml             #     Cấu hình agent (model, tools, instructions)
      prompts.yaml            #     Prompt templates
      README.md               #     Mô tả agent
      tools.yaml              #     Tools agent có thể dùng
      memory/                 #     Bộ nhớ riêng của agent
  workflows/                  #   17 workflow YAML files
    <workflow-name>.yaml      #     Định nghĩa workflow (steps, gates, error_handling)
  prompts/                    #   33 prompt templates dùng chung
  memory/                     #   Bộ nhớ chia sẻ (Git-backed)
  templates/                  #   Template files
  schemas/                    #   JSON schema cho validation
  tools/                      #   Định nghĩa tools dùng chung
  _runtime/                   #   Runtime persistence (tự động tạo)
src/                          # Mã nguồn C#
  AiCompany.Core/             #   Core: Config, Workflows, Memory, Agents
  AiCompany.Cli/              #   CLI: 6 commands
  AiCompany.Integrations/     #   Tích hợp: LLM, Git, GitHub
  AiCompany.Tests.Unit/       #   Unit tests (22 tests)
  AiCompany.Tests.Integration/ #   Integration tests (5 tests)
  AiCompany.Tests.E2E/        #   E2E tests
```

---

## 13 Agents

| Agent | Vai trò |
|-------|---------|
| `ceo` | Chiến lược, roadmap, quyết định cấp cao |
| `product-manager` | Quản lý backlog, feature, release |
| `business-analyst` | Phân tích nghiệp vụ, story, acceptance criteria |
| `solution-architect` | Thiết kế kiến trúc, ADR, tech-stack |
| `tech-lead` | Breakdown task, sprint planning, code quality |
| `backend-developer` | API, service, database |
| `frontend-developer` | UI, component, frontend |
| `mobile-developer` | iOS, Android, mobile |
| `qa-engineer` | Testing, regression, E2E |
| `security-engineer` | Bảo mật, vulnerability, compliance |
| `devops-engineer` | CI/CD, deploy, infrastructure |
| `code-reviewer` | Review code, quality gates |
| `documentation-writer` | Tài liệu, changelog, user guide |

---

## 17 Workflows

| Workflow | Mô tả |
|----------|-------|
| `idea-to-prd` | Từ ý tưởng → Product Requirements Document |
| `prd-to-stories` | PRD → User stories + acceptance criteria |
| `architecture-to-tasks` | Kiến trúc → Task breakdown |
| `stories-to-architecture` | Stories → Architecture Design Records |
| `tasks-to-development` | Task assignment → Implementation |
| `development-to-testing` | Development → QA testing |
| `testing-to-review` | Testing → Code review |
| `code-review` | Code review workflow |
| `review-to-deployment` | Review → CI/CD → Production |
| `bug-fix` | Bug triage → Fix → Verify → Deploy |
| `hotfix` | Emergency production incident response |
| `feature-request` | Feature request → Feasibility → Prioritization |
| `refactoring` | Technical debt assessment → Refactoring |
| `release` | Release branch → Changelog → Security scan → Deploy |
| `api-design` | API requirements → Design → SDK generation |
| `database-migration` | Impact assessment → Migration → Verify |
| `deployment-to-documentation` | Deploy → Update docs, changelog, runbooks |

---

## Environment Variables

| Biến | Bắt buộc | Mô tả |
|------|----------|-------|
| `OPENAI_API_KEY` | Có (cho chạy thật) | API key OpenAI |
| `GITHUB_TOKEN` | Tuỳ chọn | GitHub token cho tích hợp |

Có thể đặt trong `.env` file hoặc cấu hình trong `.ai/config.yaml`.

---

## Xử lý sự cố thường gặp

**Build lỗi:**
```bash
dotnet clean src/AiCompany.slnx
dotnet build src/AiCompany.slnx
```

**Test lỗi:**
```bash
dotnet test src/AiCompany.Tests.Unit -v n  # Xem chi tiết
dotnet test src/AiCompany.Tests.Integration -v n
```

**YAML parse error khi chạy workflow:**
Kiểm tra file `.ai/workflows/<name>.yaml`:
- Giá trị trong `constraints` có dấu `:` không? Nếu có, bọc trong `"double quotes"`
- Đúng indent (2 spaces per level)

**OpenAI API key không hoạt động:**
```bash
# Kiểm tra biến môi trường
echo $env:OPENAI_API_KEY  # PowerShell
echo %OPENAI_API_KEY%     # CMD
```
Hoặc kiểm tra `.ai/config.yaml` mục `providers.openai.api_key`.
