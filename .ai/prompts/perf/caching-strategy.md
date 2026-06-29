# Caching Strategy Prompt

## Context
Design a caching strategy for {component_or_data}.

## Requirements
- **Data Type**: {data_type} (reference data / user data / computed data / session data)
- **Read Frequency**: {read_frequency}
- **Write Frequency**: {write_frequency}
- **Data Freshness Requirements**: {freshness_requirements}
- **Data Size**: {data_size} per entry / total
- **Number of Entries**: {entry_count}

## Cache Architecture Options

### In-Memory Cache
- **Pros**: Fastest (< 1μs), no network call
- **Cons**: Limited to single node, memory pressure
- **Use When**: Small, read-heavy, frequently accessed data
- **Tool**: IMemoryCache (.NET), LRU cache

### Distributed Cache
- **Pros**: Shared across instances, supports larger data
- **Cons**: Network latency (~1-5ms), serialization overhead
- **Use When**: Multiple app instances, session data, API response cache
- **Tool**: Redis, Azure Cache for Redis

### CDN Cache
- **Pros**: Global distribution, offloads origin
- **Cons**: Cache invalidation complexity, cost
- **Use When**: Static assets, public API responses, images
- **Tool**: CloudFront, Azure CDN, Cloudflare

## Cache Patterns

### Cache-Aside (Lazy Loading)
```
1. Check cache for data
2. If found → return (cache hit)
3. If not found → load from DB
4. Store in cache
5. Return data
```

### Write-Through
```
1. Write data to DB
2. Write data to cache (or invalidate)
3. Return success
```

### Write-Behind
```
1. Write data to cache (immediate)
2. Queue DB write (async)
3. Return success immediately
4. Background worker persists to DB
```

## Cache Invalidation Strategy
| Approach | Mechanism | Complexity | Use Case |
|----------|-----------|------------|----------|
| TTL | `cache.Set(key, value, TimeSpan.FromMinutes(15))` | Low | Stale data acceptable |
| Event-driven | Invalidate on data change event | Medium | Freshness critical |
| Versioned keys | `cache.Get("product:" + product.Version)` | Medium | Versioned data |
| Cache tags | Group keys, invalidate by tag | High | Related data invalidation |

## Configuration Template
```json
{
  "Cache": {
    "Provider": "Redis",
    "ConnectionString": "${REDIS_CONNECTION_STRING}",
    "DefaultTTLMinutes": 15,
    "MaxMemoryPolicy": "allkeys-lru",
    "CircuitBreaker": {
      "FailureThreshold": 3,
      "DurationOfBreakSeconds": 30
    },
    "KeyPrefix": "{service_name}:",
    "Serialization": "MessagePack"
  }
}
```
