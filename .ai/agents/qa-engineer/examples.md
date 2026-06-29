# QA Engineer Agent Examples

## Example 1: Test Plan

**Feature**: Multi-Currency Checkout — Price Display (ST-101)

```
Test Plan: ST-101 — Currency Display on Product Page

Functional Tests:
  TC-001: Price displayed in user's local currency
  TC-002: Currency selector changes all prices on page
  TC-003: Exchange rate info shown below price
  TC-004: Unsupported locale falls back to USD
  TC-005: Price updates when currency changes in checkout

Error/Edge Cases:
  TC-101: Exchange rate API returns 503 (show USD, show banner)
  TC-102: User clears currency preference (default to locale detection)
  TC-103: Rapid currency switching (> 10 changes in 5 seconds)
  TC-104: Extremely large amounts (1M+ USD conversion)
  TC-105: Zero amount conversion
  TC-106: Fractional cents handling (rounding edge case)

Performance:
  TC-201: Currency selector renders with 50 currencies under 200ms
  TC-202: Price conversion API responds under 500ms at 100 RPS
  TC-203: Page with 100 products renders localized prices under 2s

Accessibility:
  TC-301: Currency selector keyboard navigable
  TC-302: Exchange rate info announced by screen reader
  TC-303: Error banner has role="alert"
```

## Example 2: Bug Report

```
BUG-482: Currency conversion rounding causes penny discrepancy

Severity: P2 (Major)
Environment: Staging (v2.4.1-rc3), Chrome 121, Windows 11
Reporter: @qa-engineer

Steps to Reproduce:
1. Set currency to EUR
2. Add product priced at $49.99 to cart
3. Check cart total

Expected: €45.87 (49.99 × 0.9175 = 45.870325, round half-up to 2dp)
Actual:   €45.86 (appears to be truncating instead of rounding)

Logs:
  [CurrencyService] Convert: 49.99 USD → 45.86 EUR (rate: 0.9175)
  Server-side calculation: 49.99 × 0.9175 = 45.870325
  Output: 45.86 (Math.Truncate instead of Math.Round with MidpointRounding.AwayFromZero)

Root Cause: CurrencyConversionService.cs:47 uses Math.Truncate(amount * rate * 100) / 100

Suggested Fix: Use Math.Round(amount * rate, 2, MidpointRounding.AwayFromZero)

Attachments: screenshot_cart_total.png, server_logs.txt
```

## Example 3: Release Quality Report

```
Release: v2.4.1 — Quality Gate Report

Overall: ⚠️ CONDITIONAL PASS — 2 P2 bugs require PM sign-off

Test Results:
  Unit:    1,847 / 1,847 passed (100%)
  Integ.:    326 / 326 passed (100%)
  E2E:        89 / 91 passed  (97.8%)
  Perf:      All SLAs met (p99: 145ms, target: 200ms)
  Security:  Clean (0 high, 2 low findings — accepted)
  A11y:      Clean (0 violations)

Coverage: 83.7% (baseline: 82.1%) ✅

Known Issues (pre-existing):
  BUG-478: Search result sorting resets on pagination (P3)
  BUG-480: Dark mode toggle icon doesn't update (P3)

New Issues (this release):
  BUG-482: Currency rounding penny discrepancy (P2) — awaiting PM sign-off
  BUG-483: Mobile cart scroll position lost after API call (P3)

Recommendation: ✅ RELEASE with PM sign-off on BUG-482
```
