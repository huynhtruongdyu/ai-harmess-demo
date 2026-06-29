# Open Issues

## Critical (P0) — None

## High (P1)
| ID | Title | Component | Reported | Status | Owner |
|----|-------|-----------|----------|--------|-------|
| — | None currently | | | | |

## Medium (P2)
| ID | Title | Component | Reported | Status | Owner |
|----|-------|-----------|----------|--------|-------|
| BUG-482 | Currency rounding penny discrepancy | Currency Service | 2024-11-19 | Fix Ready | Backend Dev |
| BUG-483 | Mobile cart scroll position reset after API call | Mobile App | 2024-11-18 | Triaged | Mobile Dev |
| BUG-484 | Product search indexing delay > 5 minutes | Search Service | 2024-11-17 | In Progress | Backend Dev |

## Low (P3)
| ID | Title | Component | Reported | Status | Owner |
|----|-------|-----------|----------|--------|-------|
| BUG-478 | Search result sorting resets on pagination | Frontend | 2024-11-15 | Backlog | — |
| BUG-480 | Dark mode toggle icon doesn't update | Frontend | 2024-11-14 | Backlog | — |
| BUG-481 | Admin panel loads slowly with > 10K products | Admin | 2024-11-13 | Triaged | Backend Dev |
| BUG-485 | Password reset email sometimes goes to spam | Auth | 2024-11-12 | Won't Fix | — |

## Enhancement Requests
| ID | Title | Value/Effort | Status | Notes |
|----|-------|-------------|--------|-------|
| FR-101 | Bulk product import from CSV | High/Medium | Planned Sprint 14 | — |
| FR-102 | Abandoned cart email automation | High/High | Under Review | Requires Marketing service |
| FR-103 | Multi-language storefront | Medium/XL | Q1 2025 | Major initiative, not yet scoped |

## Technical Debt
| ID | Description | Effort | Interest | Priority |
|----|-------------|--------|----------|----------|
| TD-001 | Remove legacy monolith auth service | 3 sprints | Blocking microservice migration | HIGH |
| TD-002 | Standardize error handling across services | 1 sprint | Developer confusion | MEDIUM |
| TD-003 | Replace AutoMapper with manual mapping | 2 sprints | Performance + transparency | LOW |
