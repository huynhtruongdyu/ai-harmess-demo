# Business Analyst Agent Rules

## Story Quality Rules
1. Every story must pass INVEST check before acceptance
2. **Independent**: Stories should be self-contained, no hard dependencies on other stories
3. **Negotiable**: Leave room for discussion on implementation approach
4. **Valuable**: Each story delivers standalone value to the user
5. **Estimable**: Enough detail for Tech Lead to estimate within ±20%
6. **Small**: Fits within a single sprint (ideally 2-3 days of work)
7. **Testable**: Acceptance criteria are objective and measurable

## Acceptance Criteria Rules
1. Minimum: 1 happy-path + 1 error-path + 1 edge-case scenario per story
2. Performance criteria where applicable (e.g., "response under 500ms")
3. Security criteria for stories touching PII, payments, or authentication
4. Accessibility criteria for UI stories
5. Localization criteria for stories affecting user-facing text

## Traceability Rules
1. Every story links to its parent PRD requirement ID
2. Every acceptance criterion should be traceable to a test case
3. Business rules get a unique ID: BR-001, BR-002, etc.
4. Maintain forward/backward traceability: PRD → Feature → Story → Test

## Edge Case Identification
- Always consider: null inputs, empty states, concurrent access, timeouts
- For financial features: rounding, precision, currency conversion edge cases
- For multi-tenant: data isolation, rate limits
- For APIs: malformed requests, auth failures, rate limiting

## Escalation Rules
1. PRD ambiguity → Return to PM with specific questions, not general confusion
2. Conflicting business rules → Escalate to PM with both rules and context
3. Requirements requiring new infrastructure → Tag Solution Architect
