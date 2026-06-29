# N+1 Query Analysis Prompt

## Context
Analyze and fix N+1 query problems in {component}.

## Suspect Pattern
```code
{suspected_code}
```

## What is N+1?
The N+1 problem occurs when code executes:
- 1 query to fetch parent entities
- N queries to fetch related data for each parent
- Total: 1 + N queries instead of 2

## Detection Methods
- **Log analysis**: Look for repeated identical SQL queries
- **ORM profiling**: EF Core Log, NHibernate Profiler, Sequel Pro
- **Database monitoring**: Slow query log, pg_stat_statements
- **APM tools**: Application Insights, Datadog show repeated queries

## Fix Strategies by ORM

### Entity Framework Core (.NET)
```csharp
// ❌ N+1
var orders = db.Orders.ToList();
foreach (var order in orders) {
    Console.WriteLine(order.Customer.Name); // N queries
}

// ✅ Eager loading
var orders = db.Orders.Include(o => o.Customer).ToList();

// ✅ Projection
var orderDtos = db.Orders.Select(o => new OrderDto {
    Id = o.Id,
    CustomerName = o.Customer.Name
}).ToList();
```

### React Query / Apollo (Frontend)
```graphql
# ❌ N+1
query {
  orders {
    id
    customer { name }  # N+1 on each order's customer resolver
  }
}

# ✅ Batch with DataLoader
query {
  orders {
    id
    customerId
  }
}
// Then batch load customers in a single query
```

### MongoDB / Mongoose (Node.js)
```javascript
// ❌ N+1
const orders = await Order.find();
for (const order of orders) {
  const customer = await Customer.findById(order.customerId);
}

// ✅ Batch
const orders = await Order.find();
const customerIds = orders.map(o => o.customerId);
const customers = await Customer.find({ _id: { $in: customerIds } });
```

## Fix Template
```markdown
### Issue
{description_of_nplus1}

### Impact
- {query_count_before}: {number} queries
- {query_count_after}: {number} queries
- {latency_impact}: {latency_difference}

### Fix
```diff
- {current_code}
+ {fixed_code}
```

### Verification
- [ ] Same results returned (data integrity preserved)
- [ ] Query count reduced from {before} to {after}
- [ ] Response time improved: {before}ms → {after}ms
```
