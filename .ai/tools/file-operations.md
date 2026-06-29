# File Operations Tool

## Name
`file-operations`

## Description
Reads, writes, edits, and searches files in the repository.

## Input Schema
```json
{
  "type": "object",
  "required": ["operation", "path"],
  "properties": {
    "operation": {
      "type": "string",
      "enum": ["read", "write", "edit", "delete", "list", "search", "copy", "move"]
    },
    "path": { "type": "string", "description": "File or directory path" },
    "content": { "type": "string", "description": "Content for write/edit operations" },
    "pattern": { "type": "string", "description": "Search pattern (regex)" },
    "destination": { "type": "string", "description": "Destination path for copy/move" }
  }
}
```

## Output Schema
```json
{
  "type": "object",
  "properties": {
    "success": { "type": "boolean" },
    "content": { "type": "string" },
    "files": { "type": "array", "items": { "type": "string" } },
    "matches": { "type": "array" },
    "error": { "type": "string" }
  }
}
```

## Side Effects
- `filesystem_write`: Modifies files on disk
- Confirmation required for: `delete`, `move`, overwriting existing files

## Allowed Agents
- All agents

## Usage Rules
- Max file read size: 1MB
- Max search results: 100 matches
- Write operations must not exceed 500KB
- File paths must be within the repository (no path traversal)
- Sensitive files (secrets, configs with passwords) have restricted access
