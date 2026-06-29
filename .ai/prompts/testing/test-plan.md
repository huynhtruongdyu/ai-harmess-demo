# Test Plan Prompt

## Context
Create a comprehensive test plan for {feature_name}.

## Inputs
- **PRD**: .ai/memory/current/prd.md
- **User Stories**: .ai/memory/current/stories/{story_id}.json
- **Architecture**: .ai/memory/current/architecture.md
- **API Spec**: .ai/memory/current/api/{spec_name}.yaml

## Test Plan Structure

### 1. Scope
- Features to test
- Features NOT to test (out of scope)
- Systems and integrations involved

### 2. Test Types

#### Functional Testing
| Test Case ID | Story ID | Scenario | Steps | Expected Result | Priority |
|-------------|----------|----------|-------|-----------------|----------|
| TC-{n}      | ST-{n}   | {desc}   | {steps} | {expected}   | {P0-P4}  |

#### Integration Testing
- API contract tests
- Service-to-service communication
- Database interaction

#### Performance Testing
- Expected load: {expected_rps} RPS
- Peak load: {peak_rps} RPS
- Response time SLA: {sla} p99

#### Security Testing
- Authentication bypass attempts
- Authorization boundary tests
- Input validation fuzzing
- Rate limit verification

#### Regression Testing
- {impacted_existing_features}

### 3. Test Data Requirements
- {test_data_description}

### 4. Environment Requirements
- {environment_requirements}

### 5. Entry / Exit Criteria
- **Entry**: {entry_criteria}
- **Exit**: {exit_criteria}

### 6. Risk Assessment
| Risk | Likelihood | Impact | Mitigation |
|------|-----------|--------|------------|
| {risk} | {L/M/H} | {L/M/H} | {mitigation} |
