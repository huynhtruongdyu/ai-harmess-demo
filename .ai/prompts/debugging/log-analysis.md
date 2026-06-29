# Log Analysis Prompt

## Context
Analyze the following log data to identify patterns, anomalies, and root causes.

## Log Input
```
{log_data}
```

## Time Range
{start_time} to {end_time}

## Context
- **Service**: {service_name}
- **Environment**: {environment}
- **Log Level Filter**: {log_level} (e.g., "Error and Warning")
- **Known Issues**: {known_issues}

## Instructions
1. **Group and Categorize**: Group log entries by error type, component, or pattern
2. **Identify Frequency**: How often does each error pattern occur?
3. **Correlate**: Are these errors correlated with specific times, requests, or conditions?
4. **Identify Anomalies**: What's unusual or changed?
5. **Prioritize**: Which issues should be investigated first based on frequency and severity?

## Analysis Template

### Error Summary
| Error Type | Count | % of Total | First Seen | Last Seen |
|-----------|-------|-----------|-----------|-----------|
| {type}    | {n}   | {%}       | {time}    | {time}    |

### Patterns Found
1. **{pattern_1}**: Correlation between {factor_a} and {factor_b}
   - Evidence: {log_evidence}
   - Recommendation: {recommendation}

2. **{pattern_2}**: {description}
   - Evidence: {evidence}
   - Recommendation: {recommendation}

### Critical Findings
- {finding_1}
- {finding_2}

### Recommended Actions
1. **Immediate**: {immediate_action}
2. **Short-term**: {short_term_action}
3. **Long-term**: {long_term_action}
