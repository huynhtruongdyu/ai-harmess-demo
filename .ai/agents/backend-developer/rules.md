# Backend Developer Agent Rules

## Coding Rules
1. All public methods must have XML doc comments
2. Use primary constructors for simple DI; explicit constructors for complex setup
3. Never swallow exceptions — use structured error handling via Result pattern
4. Mark all query parameters with explicit [FromQuery] attributes
5. Use CancellationToken in all async method signatures
6. Repository pattern: one repository per aggregate root
7. No magic strings — use constants or configuration

## Database Rules
1. Every migration must have an Up() and Down() method
2. Indexes must be explicitly defined (no relying on EF Core conventions for performance)
3. All text columns must specify max length
4. Soft deletes preferred over hard deletes for core entities
5. Audit fields (CreatedAt, CreatedBy, UpdatedAt, UpdatedBy) on all tables
6. N+1 query prevention: use .Include() or projection queries

## API Rules
1. Always validate input before processing (FluentValidation)
2. Return standardized error responses (RFC 7807 ProblemDetails)
3. Rate limiting headers on all public endpoints
4. Pagination on all list endpoints (page, pageSize, totalCount)
5. Idempotency keys on mutation operations
6. OpenAPI annotations for all endpoints ([SwaggerOperation], [ProducesResponseType])

## Testing Rules
1. Test names follow pattern: MethodName_Scenario_ExpectedResult
2. Use realistic data, not "test", "foo", "bar"
3. Mock external dependencies only; test internal logic with real implementations
4. Integration tests must clean up after themselves (dispose, rollback)
5. Performance regression tests for critical paths

## Security Rules
1. Validate all input — never trust client data
2. SQL injection prevention: use parameterized queries, never string concatenation
3. No secrets in code — use environment variables or secret manager
4. Apply principle of least privilege for service accounts
5. Log security events (failed auth, unauthorized access attempts)

## Escalation Rules
1. Architecture pattern conflict → Tag @solution-architect with specific question and proposed approach
2. Requirement ambiguity → Tag @tech-lead with specific clarification needed
3. External dependency bug → Document with repro steps, assign to @tech-lead for triage
