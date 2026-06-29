# Code Reviewer Agent Examples

## Example 1: Standard PR Review

**PR #234**: Currency Conversion Service — `currency-service/`

```
📋 Summary: Implements currency conversion endpoint with caching layer.
Files: 8 changed (+245 / -12)

Overall: ✅ APPROVED (with suggestions)

❌ BLOCKING:
1. CurrencyController.cs:42 — SQL injection risk
   Code: $"SELECT rate FROM rates WHERE code = '{currencyCode}'"
   Risk: currencyCode comes from user input, allows SQL injection
   Fix: Use parameterized query with Dapper: `WHERE code = @Code`

2. CurrencyService.cs:88 — No timeout on HTTP call
   Code: await httpClient.GetAsync(url)  // no cancellation
   Fix: Pass CancellationToken, add timeout policy via Polly

💡 SUGGESTIONS:
1. CurrencyService.cs:22 — Consider using IOptions<CurrencyOptions>
   instead of direct IConfiguration binding for testability

2. CurrencyConverterTests.cs:55 — Great use of edge case testing
   with zero amount and invalid currencies! Consider adding
   a test for extremely large values (decimal.MaxValue).

3. appsettings.json:15 — Consider making cache duration configurable
   per environment (shorter TTL for dev/staging)

✅ POSITIVE NOTES:
- Excellent separation of concerns (Controller → Service → Provider)
- Circuit breaker pattern implemented correctly
- Comprehensive error handling with ProblemDetails
- All 32 tests pass, 87% coverage (above 80% threshold)
```

## Example 2: Hotfix Review

**PR #237**: Fix currency rounding — `currency-service/`

```
📋 Summary: Hotfix for BUG-482 — Currency rounding penny discrepancy
Files: 2 changed (+1 / -1)

❌ BLOCKING: None critical for hotfix (edge case, P2)

💡 OBSERVATION:
CurrencyService.cs:47 — Changed Math.Truncate to Math.Round
  This fixes the reported bug. However, this changes rounding behavior
  for ALL conversions. Please verify:
  1. Accounting reports that depend on precise totals are unaffected
  2. Unit tests cover various rounding scenarios (0.005, 1.005, etc.)

  See comment in ST-202 tests for the added rounding test cases — good work.

[Approved as hotfix with note to run full regression in morning]
```

## Example 3: Rejected PR

**PR #238**: Refactor Auth Middleware

```
📋 Summary: Refactors authentication middleware to gRPC-based token validation
Files: 5 changed (+312 / -89)

⛔ REQUESTED CHANGES

❌ BLOCKING:
1. AuthMiddleware.cs:15 — Architecture violation
   PR introduces direct HTTP call to auth service from middleware.
   ADR-004 specifies all service-to-service calls must use message queue.
   Please either:
   (a) Use RabbitMQ for token validation event, or
   (b) Get ADR-004 exception approved by @solution-architect

2. AuthMiddlewareTests.cs:33 — No auth service mock
   Tests call real auth service endpoint, making them flaky
   and environment-dependent. Mock the IAuthServiceClient.

💡 SUGGESTIONS:
1. Middleware.cs:12 — Register as singleton, not transient
   (It's stateless and expensive to construct)

Re-review required after addressing blocking issues.
Tagging @solution-architect for architecture question.
```
