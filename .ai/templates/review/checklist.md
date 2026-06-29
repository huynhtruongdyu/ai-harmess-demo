# Code Review Checklist

## 1. Functionality
- [ ] Does the code implement the acceptance criteria?
- [ ] Are edge cases handled (null, empty, invalid input)?
- [ ] Are error conditions handled gracefully?
- [ ] Does the code handle concurrent access correctly?

## 2. Architecture
- [ ] Follows Clean Architecture layering?
- [ ] Proper separation of concerns?
- [ ] SOLID principles followed?
- [ ] No circular dependencies?

## 3. Security
- [ ] Input validation on all user-facing inputs?
- [ ] SQL injection prevented (parameterized queries)?
- [ ] XSS prevented (output encoding)?
- [ ] Authentication enforced? Authorization checked?
- [ ] No secrets in code?

## 4. Performance
- [ ] No N+1 query problems?
- [ ] Async/await used correctly (no sync-over-async)?
- [ ] Caching considered for expensive operations?
- [ ] Pagination implemented for list endpoints?

## 5. Testing
- [ ] Unit tests for all business logic?
- [ ] Integration tests for API endpoints?
- [ ] Edge cases and error paths covered?
- [ ] Tests are meaningful (not just coverage padding)?

## 6. Readability
- [ ] Clear naming throughout?
- [ ] Methods under 30 lines?
- [ ] No commented-out code?
- [ ] No magic numbers/strings?
- [ ] Complex logic explained?

## 7. Documentation
- [ ] Public APIs documented?
- [ ] Architecture changes documented?
- [ ] Configuration changes documented?

## Approval Decision
- [ ] APPROVE — No issues, or only minor suggestions
- [ ] APPROVE WITH COMMENTS — Non-blocking suggestions
- [ ] REQUEST CHANGES — Blocking issues found
