# Git Operations Tool

## Name
`git-operations`

## Description
Performs Git version control operations including commit, branch, merge, rebase, and PR management.

## Input Schema
```json
{
  "type": "object",
  "required": ["operation", "path"],
  "properties": {
    "operation": {
      "type": "string",
      "enum": ["commit", "branch", "merge", "rebase", "push", "pull", "status", "diff", "log", "cherry-pick"]
    },
    "path": { "type": "string", "description": "Repository path" },
    "message": { "type": "string", "description": "Commit message (required for commit)" },
    "branch": { "type": "string", "description": "Branch name (required for branch/merge)" },
    "files": { "type": "array", "items": { "type": "string" }, "description": "Files to commit" }
  }
}
```

## Output Schema
```json
{
  "type": "object",
  "properties": {
    "success": { "type": "boolean" },
    "output": { "type": "string" },
    "error": { "type": "string" }
  }
}
```

## Side Effects
- `git_commit`: Modifies Git history, requires confirmation for main branch operations

## Allowed Agents
- All developer agents
- DevOps Engineer
- Tech Lead
- Documentation Writer

## Usage Rules
- Never commit directly to main — always use feature branches
- Commit messages follow conventional commits: `type(scope): description`
- PR creation requires passing local tests first
- Force push only on feature branches (never on shared branches)
