# Performance Test Prompt

## Context
Design and execute a performance test for {system_under_test}.

## System Details
- **Service**: {service_name}
- **Endpoints to Test**: {endpoints}
- **Expected Load**: {expected_load}
- **Peak Load**: {peak_load}
- **SLA Targets**:
  - Latency p50: {p50_target}
  - Latency p95: {p95_target}
  - Latency p99: {p99_target}
  - Throughput: {throughput_target}
  - Error rate: {error_rate_target}

## Test Scenarios

### Baseline Test
- Load: {baseline_load} for {duration}
- Measure: latency, throughput, resource usage
- Goal: Establish baseline metrics

### Load Test
- Gradually increase load from {start_load} to {target_load}
- Measure: degradation curve, breaking point
- Goal: Verify system handles expected load

### Stress Test
- Load beyond expected peak: {stress_load}
- Measure: recovery behavior, error handling
- Goal: Understand system limits

### Endurance Test
- Sustained load: {sustained_load} for {sustained_duration}
- Measure: memory leak, degradation over time
- Goal: Verify long-running stability

### Spike Test
- Sudden load increase: {normal} → {spike} in {ramp_up_time}
- Measure: auto-scaling response time, recovery
- Goal: Verify burst handling

## Reporting Template
```markdown
## Performance Test Results

### Summary
| Scenario | Status | p50 | p95 | p99 | Throughput | Error Rate |
|----------|--------|-----|-----|-----|------------|------------|
| {scenario} | ✅/❌ | {ms} | {ms} | {ms} | {rps} | {%} |

### Observations
- {observation_1}
- {observation_2}

### Recommendations
1. {recommendation_1}
2. {recommendation_2}
```
