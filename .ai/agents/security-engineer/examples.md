# Security Engineer Agent Examples

## Example 1: Threat Model

```
Threat Model: Multi-Currency Checkout (Feature MCC-2024)

Component: Currency Conversion Service

Threat T-001: Exchange Rate API key leakage
  STRIDE: Information Disclosure
  Risk: High (CVSS 7.5) — API key allows unauthorized access
  Mitigation:
  - Store key in Azure Key Vault, not config file
  - Rotate key every 90 days
  - Monitor for unusual API usage patterns

Threat T-002: Man-in-the-middle rate manipulation
  STRIDE: Tampering
  Risk: Critical (CVSS 9.1) — attacker modifies exchange rate
  Mitigation:
  - TLS 1.3 between Currency Service and Exchange Rate Provider
  - Response signing with HMAC verification
  - Rate anomaly detection (alert if rate deviates >5% from market)

Threat T-003: Cache poisoning of exchange rates
  STRIDE: Tampering
  Risk: High (CVSS 8.0)
  Mitigation:
  - In-memory cache with TTL (not shared cache for rate data)
  - Validate rate reasonableness before caching (0.01 to 1000 range)
  - Audit trail for rate changes

Threat T-004: Customer currency preference manipulation
  STRIDE: Elevation of Privilege
  Risk: Medium (CVSS 5.5)
  Mitigation:
  - Validate currency codes against allowlist
  - Server-side validation (not just client-side)
  - Rate limit preference changes (max 10/hour)
```

## Example 2: Security Review Report

```
Security Review: PR #234 — Currency Conversion API

Finding S-001: SQL Injection in currency code parameter
  File: CurrencyController.cs:42
  Severity: CRITICAL
  Description: Currency code passed directly into SQL query string
  Code: $"SELECT rate FROM rates WHERE from_currency = '{currencyCode}'"
  Fix: Use parameterized query
  Recommendation: DbContext.Rates.Where(r => r.FromCurrency == currencyCode)

Finding S-002: Rate limiting missing
  File: Program.cs
  Severity: HIGH
  Description: No rate limiting on /api/v1/currency/convert endpoint
  Impact: Attacker can enumerate currency pairs, potential DoS
  Fix: Add `[EnableRateLimiting("PublicApi")]` attribute or middleware

Finding S-003: JWT not validated on internal service calls
  File: CurrencyService.cs
  Severity: HIGH
  Description: Internal HTTP calls between services skip auth check
  Fix: Implement mTLS for service-to-service communication

Overall: BLOCKED — 1 critical + 2 high findings must be resolved before merge.
```

## Example 3: Compliance Check

```
Compliance Check: GDPR — Multi-Currency Feature

Article 5(1)(c) — Data Minimization:
  Currency preference stored per customer. ✅ — Necessary for feature.
  IP-based locale detection does NOT store IP. ✅ — No retention.

Article 12 — Transparent Information:
  Exchange rate source disclosed in UI? ⚠️ — Missing in current design.
  Recommendation: Add "Exchange rates provided by [Provider]" text.

Article 17 — Right to Erasure:
  Deleting account removes currency preference. ✅ — Cascade delete configured.
  Rate cache entries with customer ID cleared. ⚠️ — Need TTL verification.

Article 32 — Security of Processing:
  TLS 1.3, AES-256, parameterized queries. ✅

OVERALL: CONDITIONALLY COMPLIANT — Fix Article 12 transparency gap before launch.
```
