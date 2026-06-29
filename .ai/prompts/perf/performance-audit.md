# Performance Audit Prompt

## Context
Conduct a performance audit of {system_component}.

## Audit Scope
- **Component**: {component_name}
- **Endpoints/Operations**: {operations}
- **Environment**: {environment}
- **Baseline Date**: {baseline_date}
- **Previous Baseline**: {previous_baseline}

## Audit Areas

### API Performance
- Response times (p50, p95, p99)
- Throughput (requests/second)
- Error rates
- Payload sizes
- Connection pool utilization

### Database Performance
- Query execution times
- Index usage (scans vs seeks)
- Connection pool utilization
- Deadlocks / blocking
- Cache hit ratio

### Memory Usage
- Heap allocation rate
- Garbage collection pressure
- Memory leak detection
- Large object heap usage

### CPU Profiling
- Hot paths identification
- Thread pool utilization
- Async/await patterns (sync-over-async detection)
- Lock contention

### Frontend/Mobile Performance
- Core Web Vitals (LCP, FID, CLS)
- Bundle size analysis
- Time to Interactive (TTI)
- First Contentful Paint (FCP)

## Tools to Use
- Application Insights / Datadog APM
- Database: Query Store, Execution Plan analysis
- Profiling: dotTrace, PerfView, Chrome DevTools
- Load testing: k6, JMeter, NBomber

## Audit Report Template
```markdown
### Summary
- **Overall Health**: {good / fair / poor}
- **Regressions**: {list_of_regressions}
- **Improvements**: {list_of_improvements}

### Findings
1. {finding_1} ({severity})
   - Current: {current_value}
   - Target: {target_value}
   - Recommendation: {recommendation}

### Top Recommendations
1. **{recommendation_1}** — Expected impact: {impact}
2. **{recommendation_2}** — Expected impact: {impact}
3. **{recommendation_3}** — Expected impact: {impact}
```
