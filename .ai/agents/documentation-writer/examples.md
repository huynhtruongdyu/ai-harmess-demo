# Documentation Writer Agent Examples

## Example 1: API Documentation

**API Endpoint**: Convert Currency

```
## Convert Currency

Converts an amount from one currency to another using real-time exchange rates.

**Endpoint**: `GET /api/v1/currency/convert`

**Authentication**: Required (Bearer token)

**Query Parameters**:
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| amount    | decimal | yes | Amount to convert (positive, max 999999999.99) |
| from      | string  | yes | Source currency code (ISO 4217, e.g., USD) |
| to        | string  | yes | Target currency code (ISO 4217, e.g., EUR) |

**Example Request**:
```bash
curl -X GET "https://api.merchantos.com/api/v1/currency/convert?amount=49.99&from=USD&to=EUR" \
  -H "Authorization: Bearer {your_token}"
```

**Success Response** (200 OK):
```json
{
  "success": true,
  "data": {
    "amount": 45.87,
    "rate": 0.9175,
    "from": "USD",
    "to": "EUR",
    "timestamp": "2024-11-20T10:30:00Z"
  }
}
```

**Error Responses**:
| Code | Description | Example |
|------|-------------|---------|
| 400  | Invalid currency code | `{ "error": "Unsupported currency: INVALID" }` |
| 401  | Missing/invalid auth | `{ "error": "Authentication required" }` |
| 503  | Exchange rate unavailable | `{ "error": "Exchange rate service temporarily unavailable" }` |

**Rate Limiting**: 100 requests per minute per API key
```

## Example 2: Changelog Entry

```
# Changelog

## [2.4.1] - 2024-11-20

### Added
- Multi-currency checkout: customers can now view prices in their local currency
  and check out in EUR, GBP, JPY, and 10+ additional currencies (#234)
- Currency selector component on product detail page
- Automatic locale detection for currency display

### Changed
- Checkout flow now displays estimated and charged amounts in selected currency
- Exchange rate cache TTL increased from 5 min to 15 min for better performance

### Fixed
- Currency rounding now uses Math.Round(AwayFromZero) instead of Truncate,
  fixing penny discrepancies in cart totals (BUG-482, PR #237)
- Cart total recalculates correctly when currency is changed mid-session (#235)

### Security
- Rate limiting added to all currency conversion endpoints
- SQL injection vulnerability fixed in currency lookup query (thanks @security-engineer)

### Deprecated
- Legacy /api/v1/price endpoint (use /api/v1/currency/convert instead)

## [2.4.0] - 2024-11-01
...
```

## Example 3: User Guide (Excerpt)

```
# Multi-Currency Checkout User Guide

## Overview
Starting with v2.4.1, MerchantOS supports displaying prices and completing
checkout in multiple currencies. Your customers see prices in their local
currency, which increases trust and reduces checkout abandonment.

## How It Works

### Step 1: Enable Multi-Currency
1. Go to **Settings → Payments → Multi-Currency**
2. Toggle **Enable Multi-Currency** to ON
3. Select which currencies to support (minimum 2, maximum 15)

### Step 2: Configure Exchange Rates
Exchange rates are fetched automatically from OpenExchangeRates.
You can configure update frequency in **Settings → Payments → Rates**:

> **Note**: Exchange rates update every 15 minutes by default.
> For real-time rates, upgrade to the Enterprise plan.

### Supported Currencies
| Code | Currency | Supported Since |
|------|----------|-----------------|
| USD  | US Dollar | v2.4.0 |
| EUR  | Euro | v2.4.1 |
| GBP  | British Pound | v2.4.1 |
| JPY  | Japanese Yen | v2.4.1 |

### Troubleshooting

**Problem**: Customer sees USD instead of local currency
  **Solution**: Ensure the customer's browser locale is detected correctly.
  They can also manually select their currency from the dropdown.

**Problem**: "Exchange rate unavailable" error
  **Solution**: This means our provider is temporarily unavailable.
  Prices will display in USD as fallback. Rates resume automatically
  when the provider recovers.
```
