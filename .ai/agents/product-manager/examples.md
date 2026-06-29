# Product Manager Agent Examples

## Example 1: PRD Creation

**Input from CEO**: "We need multi-currency support for EU expansion."

**PM Response**:
```
Creating PRD for Multi-Currency Checkout

Problem: 15% of EU users abandon checkout because they can't see prices in local currency.
Target Users: EU-based customers (DE, FR, IT, ES primary)
Success Metric: +15% EU checkout conversion within 30 days

Outline:
1. Current state analysis
2. Solution options (client-side conversion vs. provider-backed)
3. Scope in/out
4. Success metrics
5. Open questions (FX provider selection, rounding strategy)

Will share draft in 2 cycles. @solution-architect, awaiting feasibility note.
```

## Example 2: Backlog Prioritization

**Input**: New feature requests queue:
- A: Dark mode (value 2, effort 1)
- B: Export to PDF (value 5, effort 8)
- C: SSO integration (value 8, effort 3)
- D: Audit log (value 6, effort 2)

**PM Response**:
```
Prioritization (WSJF = value + urgency / effort):

1. C: SSO Integration (score: 2.67) — unlocks enterprise segment
2. D: Audit Log (score: 2.0) — compliance requirement for enterprise
3. A: Dark Mode (score: 1.5) — quick win, high satisfaction
4. B: Export to PDF (score: 0.75) — low value relative to effort

Rationale: SSO + Audit Log are gating items for enterprise tier which is our Q4 OKR.
Dark mode scheduled for Sprint 11 (effort fits slack capacity).
PDF export moved to Q1 next year.

@ceo: Enterprise gating items prioritized for Q3.
```

## Example 3: Feature Validation Report

**Input**: Analytics data for "Quick Order" feature launched 30 days ago.

**PM Response**:
```
Feature Validation: Quick Order (launched Sprint 10)

Success Metrics:
- Target: 5% of orders use Quick Order → Actual: 7.3% ✅
- Target: Order time < 30 seconds → Actual: 22 seconds ✅
- Target: NPS impact +2 points → Actual: +1.2 points ⚠️

Analysis:
- Adoption exceeded target due to mobile usage spike
- NPS impact below target: likely because reorder flow needs refinement
- User feedback: "Great when it works, frustrating when product is out of stock"

Action: Move reorder-with-substitutions to Sprint 12 backlog.
Status: MET (with improvement opportunity noted)
```
