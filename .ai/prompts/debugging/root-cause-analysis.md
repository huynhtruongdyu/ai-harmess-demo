# Root Cause Analysis Prompt

## Context
Perform a root cause analysis for the production incident described below.

## Incident Summary
- **Incident ID**: {incident_id}
- **Severity**: {severity}
- **Duration**: {duration}
- **Impact**: {impact_description}
- **Timeline**: {incident_timeline}

## Evidence
- **Logs**: {log_references}
- **Metrics**: {metric_references}
- **Traces**: {trace_references}
- **Deployments**: {deployment_history}
- **Configuration Changes**: {config_changes}

## Instructions
Use the **5 Whys** technique to identify the root cause:

1. **Why did the incident occur?** → Immediate cause
2. **Why did that happen?** → Contributing factor
3. **Why was that the case?** → Systemic issue
4. **Why wasn't this caught earlier?** → Detection gap
5. **Why wasn't this prevented?** → Prevention gap

## Output Template

### Root Cause Statement
{one_sentence_root_cause}

### 5 Whys Analysis
1. {answer_1}
2. {answer_2}
3. {answer_3}
4. {answer_4}
5. {answer_5}

### Contributing Factors
- {factor_1}
- {factor_2}

### Systemic Gaps
- **Detection**: {detection_gap}
- **Prevention**: {prevention_gap}
- **Process**: {process_gap}

### Action Items
| Action | Owner | Deadline | Type |
|--------|-------|----------|------|
| {action} | {owner} | {deadline} | {prevent/detect/mitigate} |

### Lessons Learned
{lessons_learned}
