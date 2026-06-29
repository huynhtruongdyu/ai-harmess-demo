# Code Analysis Tool

## Name
`code-analysis`

## Description
Performs static analysis on code: linting, type checking, security scanning, and complexity analysis.

## Input Schema
```json
{
  "type": "object",
  "required": ["operation", "path"],
  "properties": {
    "operation": {
      "type": "string",
      "enum": ["lint", "typecheck", "security-scan", "complexity", "coverage", "dep-graph"]
    },
    "path": { "type": "string", "description": "File or directory to analyze" },
    "config": { "type": "string", "description": "Path to configuration file" }
  }
}
```

## Output Schema
```json
{
  "type": "object",
  "properties": {
    "success": { "type": "boolean" },
    "results": {
      "type": "object",
      "properties": {
        "errors": { "type": "array", "items": { "type": "object" } },
        "warnings": { "type": "array", "items": { "type": "object" } },
        "metrics": { "type": "object" }
      }
    },
    "summary": { "type": "string" }
  }
}
```

## Side Effects
- None (read-only analysis)

## Allowed Agents
- All developer agents
- Code Reviewer (for PR analysis)
- Security Engineer (for security scanning)
- QA Engineer (for coverage analysis)

## Supported Operations
| Operation | Tool | Description |
|-----------|------|-------------|
| lint | ESLint/StyleCop | Code style and quality rules |
| typecheck | TypeScript/ReSharper | Static type checking |
| security-scan | Semgrep/Defend | Security vulnerability detection |
| complexity | CodeMetrics | Cyclomatic complexity analysis |
| coverage | Coverage reporting tools | Code coverage analysis |
| dep-graph | Dependency analysis | Dependency graph visualization |
