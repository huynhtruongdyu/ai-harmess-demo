# Tech Lead Agent Examples

## Example 1: Story Breakdown

**Input**: User story "Multi-currency checkout - price display"

**Tech Lead Response**:
```
Story: ST-101 — Show localized prices on product page
Estimate: 8 story points (medium complexity)

Task breakdown:
┌── Frontend (assigned: @frontend-dev, est: 2d)
│   ├── T-101.1: Create CurrencySelector component (4h)
│   ├── T-101.2: Update ProductCard to use localized price display (3h)
│   ├── T-101.3: Add locale detection on page load (2h)
│   └── T-101.4: Error state when conversion fails (1h)
│
├── Backend (assigned: @backend-dev, est: 2d)
│   ├── T-101.5: Create currency conversion API endpoint (4h)
│   ├── T-101.6: Add exchange rate cache layer (3h)
│   ├── T-101.7: Write DB migration for currency_preference (1h)
│   └── T-101.8: Integration tests for conversion flow (2h)
│
├── QA (assigned: @qa-engineer, est: 1d)
│   └── T-101.9: Write E2E tests for currency scenarios (4h)
│
└── Docs (assigned: @docs-writer, est: 0.5d)
    └── T-101.10: Document multi-currency feature (2h)

Dependencies: T-101.5 → T-101.2 (backend API must exist first)
Risk: Exchange rate provider availability (mitigation: circuit breaker)
```

## Example 2: Sprint Health Report

```
Sprint 12 — Mid-Sprint Report (Day 5 of 10)

Velocity: 42 of planned 55 points (76%) — below target
Status: YELLOW

Completed: 3 stories (ST-101, ST-102, ST-104)
In Progress: 4 stories
Not Started: 2 stories (ST-107, ST-108 — blocked on ADR-010)

Blockers:
1. ST-107: Waiting on Architecture decision (ADR-010) — @solution-architect
2. ST-105: Integration test env down — @devops-engineer

Adjustments:
- Reassigned @mobile-dev to ST-105 (was idle, blocked on dependency)
- Reduced ST-106 scope (moved "analytics tracking" to next sprint)
- Risk: May miss sprint target by 5-8 points

Recommendation: Hold scope, no additions. Flagging to @product-manager.
```

## Example 3: Code Review Assignment

```
New PR: BKD-204 — Currency conversion API
Author: @backend-developer
Files changed: 8 (+245 / -12)
Labels: backend, currency, api-change

Auto-assign: @code-reviewer (primary), @solution-architect (secondary)
Rationale: API contract change requires architecture review.

Priority: HIGH
Review SLA: 4 hours (blocking ST-105)

Pre-review checks:
- [x] Tests pass (32/32)
- [x] Lint passes
- [x] Coverage: 87% (baseline: 82%)
- [x] No security flags
- [x] OpenAPI spec updated
```
