# Generate API Endpoint Prompt

## Context
You are implementing a new API endpoint for {feature_name} in the {service_name} service.

## Requirements
- **Story**: {story_reference}
- **Acceptance Criteria**: {acceptance_criteria}
- **API Design**: {api_design_reference}
- **OpenAPI Spec**: {openapi_spec_path}

## Technical Context
- **Language/Framework**: {language} / {framework}
- **Architecture Pattern**: Clean Architecture
- **Database**: {database_type}
- **Authentication**: {auth_mechanism}
- **Caching**: {cache_strategy}
- **Error Handling Pattern**: FluentResults + ProblemDetails

## Implementation Checklist
1. Create/Update DTOs (Request/Response)
2. Create API endpoint in controller
3. Implement service layer business logic
4. Implement repository/data access layer
5. Add validation (FluentValidation)
6. Add mapping (AutoMapper or manual)
7. Write unit tests for service logic
8. Write integration tests for endpoint
9. Update OpenAPI spec
10. Run all tests and verify coverage

## Constraints
- Follow existing patterns in `src/services/{service_name}/`
- Use async/await throughout
- Input validation on all endpoints
- Standardized response format: `{ success, data, error, meta }`
- Log all errors with structured logging
- No secrets or hardcoded values
