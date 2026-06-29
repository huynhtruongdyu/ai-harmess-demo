# Documentation Writer Agent Rules

## API Documentation Rules
1. Every endpoint must document: description, path, method, parameters, request body, response schemas, error codes, authentication
2. Include a working curl example for each endpoint
3. Document rate limits and pagination behavior
4. Show example responses (success + error)
5. Use OpenAPI 3.1 spec as source of truth, generate markdown from it

## Changelog Rules
1. Follow Keep a Changelog format: Unreleased → [Version] → Archived
2. Categorize: Added, Changed, Deprecated, Removed, Fixed, Security
3. Every changelog entry must reference: PR number, issue number (if applicable), author
4. Changelog updated with every PR that changes user-facing behavior
5. Release notes summarize the changelog for the current version

## User Guide Rules
1. Start with a "what you can do" overview, not technical setup
2. Use numbered steps for procedures
3. Include screenshots for UI-based features (annotated with callouts)
4. Provide a troubleshooting section for common issues
5. Link to related topics and API docs where relevant

## Formatting Rules
1. Use ATX headings (## for H2, ### for H3, etc.)
2. Code blocks specify language for syntax highlighting
3. Tables for structured data
4. Use admonitions for notes, warnings, and tips:
   > **Note**: Important context
   > **Warning**: Potential issue
   > **Tip**: Best practice
5. Keep lines under 100 characters for readability
6. Use relative links within the repository

## Review Rules
1. Self-review: read through docs as if you're the target audience
2. Technical review: tag @tech-lead or relevant developer to verify accuracy
3. Never publish with broken links (validate before commit)
4. Remove deprecated documentation (or clearly mark as deprecated with link to replacement)

## Escalation Rules
1. Technical accuracy question → Tag subject matter expert (backend/frontend/mobile)
2. Feature not yet stable enough to document → Tag @tech-lead for clarification on stability
3. Documentation gaps requiring product decisions → Tag @product-manager
