# Business Rules

## Order Management
| ID | Rule | Category | Source |
|----|------|----------|--------|
| BR-001 | Orders with total value > $1,000 require manager approval | Approval | Legal |
| BR-002 | Free shipping on orders over $50 (domestic) | Shipping | Marketing |
| BR-003 | International shipping costs calculated at checkout | Shipping | Operations |
| BR-004 | Orders can be cancelled within 1 hour of placement | Fulfillment | Policy |
| BR-005 | Maximum 100 items per order | Validation | Operations |
| BR-006 | Gift card purchases are final (no returns) | Payments | Legal |

## Multi-Currency (New)
| ID | Rule | Category | Source |
|----|------|----------|--------|
| BR-007 | Display prices in customer's locale currency by default | Display | Product |
| BR-008 | Customers can manually override display currency | Display | Product |
| BR-009 | Checkout always processes in merchant's base currency | Payments | Operations |
| BR-010 | Exchange rates update every 15 minutes minimum | Data | Architecture |
| BR-011 | Fallback to USD when exchange rate unavailable | Resilience | Engineering |
| BR-012 | Round to 2 decimal places using MidpointRounding.AwayFromZero | Calculations | Finance |
| BR-013 | Supported currencies: USD, EUR, GBP, JPY, CAD, AUD | Scope | Product |
| BR-014 | Currency conversion history logged for audit trail | Compliance | Finance |

## Security
| ID | Rule | Category | Source |
|----|------|----------|--------|
| BR-020 | Password minimum 12 characters with complexity | Auth | Security |
| BR-021 | MFA required for merchant admin accounts | Auth | Security |
| BR-022 | Session timeout after 30 minutes of inactivity | Auth | Security |
| BR-023 | API rate limit: 1000 req/min per API key | API | Engineering |
| BR-024 | PCI data never stored in application database | Compliance | Security |

## Merchant Tiers
| ID | Rule | Category | Source |
|----|------|----------|--------|
| BR-030 | Free tier: up to 100 products, 500 orders/month | Product | Marketing |
| BR-031 | Pro tier: unlimited products, priority support | Product | Marketing |
| BR-032 | Enterprise tier: custom integrations, SLA guarantee | Product | Sales |
