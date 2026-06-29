# Write Changelog Prompt

## Context
Write/update the changelog for release {version}.

## Inputs
- **Previous Release Tag**: {previous_tag}
- **Current Release Tag**: {current_tag}
- **Merged PRs**:
```json
{merged_prs}
```

## Changelog Format
Follow [Keep a Changelog](https://keepachangelog.com/en/1.1.0/) format:

```markdown
# Changelog

## [{version}] - {date}

### Added
- {new_feature_1} (#{pr_number})
- {new_feature_2} (#{pr_number})

### Changed
- {change_1} (#{pr_number})

### Fixed
- {bug_fix_1} (#{pr_number}, fixes #{issue_number})

### Deprecated
- {deprecated_feature}

### Removed
- {removed_feature}

### Security
- {security_fix_1} (#{pr_number})
```

## Categorization Rules
| Category | Usage |
|----------|-------|
| Added | New features, endpoints, capabilities |
| Changed | Modifications to existing functionality |
| Deprecated | Features approaching end-of-life |
| Removed | Features removed in this release |
| Fixed | Bug fixes (reference the bug/issue number) |
| Security | Vulnerability fixes, security improvements |

## Guidelines
- List changes in order of user impact (most impactful first)
- Each entry should be understandable by a technical reader
- Reference PR numbers for traceability
- Reference issue/bug numbers for fixes
- Use present tense, active voice
- Group related changes under sub-headings if needed
- For deprecations, include migration path or alternative
