# Business Analyst Agent Examples

## Example 1: PRD Decomposition

**Input**: PRD for "Multi-Currency Checkout"

**BA Response**:
```
Decomposing PRD-MCC-2024-001 into stories:

Epic 1: Currency Display
  Story 1.1: Show localized prices on product page
    As a customer, I want to see product prices in my local currency
    so that I can understand the actual cost.
    AC: Gherkin scenario with DE, FR, IT locale detection

  Story 1.2: Show currency selector
    As a customer, I want to choose my display currency
    so that I can view prices in my preferred currency.

  Story 1.3: Show exchange rate info
    As a customer, I want to see the exchange rate applied
    so that I can trust the conversion is fair.

Epic 2: Checkout with Currency Conversion
  Story 2.1: Convert cart total at checkout
    ...

Epic 3: Settlement & Reporting
  ...

Total: 9 stories across 3 epics
Business rules identified: BR-007 through BR-015
See attached for full decomposition.
```

## Example 2: Gherkin Acceptance Criteria

**Output**:
```gherkin
Feature: Multi-Currency Product Display
  As a customer
  I want to see product prices in my local currency
  So that I can understand the actual cost before purchasing

  Background:
    Given the exchange rates are loaded
    And the customer locale is "de-DE"

  Scenario: Price displayed in local currency
    Given a product priced at 49.99 USD
    When the product page loads
    Then the price should display as "45,87 €"
    And the exchange rate "1 USD = 0.9175 EUR" should be visible
    And the original USD price should be shown in parentheses

  Scenario: Unsupported locale falls back to USD
    Given the customer locale is "xx-XX"
    When the product page loads
    Then the price should display in USD
    And a banner should say "Prices shown in USD"

  Scenario: Exchange rate API unavailable
    Given the exchange rate service returns 503
    When the product page loads
    Then the price should display in USD
    And a banner should say "Currency conversion temporarily unavailable"
    And the system should log alert EXRATE-001
```

## Example 3: Traceability Matrix

```
Requirement ID | Story ID | Acceptance Criteria | Test Case ID | Status
PRD-MCC-1.1   | ST-101   | AC-101.1 through AC-101.3 | TC-MCC-001 | DRAFT
PRD-MCC-1.2   | ST-102   | AC-102.1 through AC-102.2 | TC-MCC-002 | DRAFT
PRD-MCC-2.1   | ST-201   | AC-201.1 through AC-201.5 | TC-MCC-003 | REVIEW
```
