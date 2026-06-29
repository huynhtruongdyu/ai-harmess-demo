# Webhook Design Prompt

## Context
Design a webhook system for {event_source}.

## Requirements
- **Events to Publish**: {events}
- **Expected Volume**: {volume} events/day
- **Delivery Guarantee**: {guarantee} (at-least-once / exactly-once)
- **Max Subscribers**: {max_subscribers}
- **Retry Policy**: {retry_policy}

## Webhook Payload Format
```json
{
  "eventId": "evt_abc123",
  "eventType": "order.created",
  "eventVersion": "1.0",
  "occurredAt": "2024-11-20T10:30:00Z",
  "data": {
    "orderId": "ord_456",
    "customerId": "cus_789",
    "total": 49.99,
    "currency": "USD"
  }
}
```

## Design Specifications

### Event Types
- Naming: `{resource}.{action}` (e.g., `order.created`, `payment.refunded`, `customer.updated`)
- Version: Semantic versioning for event schemas
- Schema: Each event type should have a documented JSON Schema

### Delivery Mechanism
- Protocol: HTTPS POST
- Content-Type: `application/json`
- Signature: HMAC-SHA256 signature in header `X-Webhook-Signature`
- Idempotency: Include `eventId` for deduplication on consumer side

### Retry Policy
| Attempt | Delay |
|---------|-------|
| 1st | Immediate |
| 2nd | 10 seconds |
| 3rd | 1 minute |
| 4th | 10 minutes |
| 5th | 1 hour |
| 6th | 6 hours |
| Final | 24 hours → dead letter queue |

### Security
- Signature verification (HMAC-SHA256 with shared secret)
- IP allowlisting (publish list of webhook IPs)
- HTTPS only
- Time-window validation (reject events older than 5 minutes)
- Rate limit per subscriber: {rate_limit} requests/second

### Monitoring
- Delivery success/failure rate
- Average delivery latency
- Dead letter queue depth
- Subscriber health scores (consecutive failures)
