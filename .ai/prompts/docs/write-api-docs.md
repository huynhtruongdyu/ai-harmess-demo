# Write API Documentation Prompt

## Context
Document the following API endpoint(s) for developers who will integrate with the system.

## Inputs
- **OpenAPI Spec**: {openapi_spec_path}
- **Service**: {service_name}
- **Endpoints**: {endpoint_list}

## Documentation Requirements
For each endpoint, document:

### 1. Overview
- Purpose of the endpoint
- When to use it (use cases)
- Link to related endpoints

### 2. Request
- HTTP method and URL
- Authentication requirements
- Headers (Content-Type, Accept, Authorization)
- Path parameters (name, type, required, description)
- Query parameters (name, type, required, description, default, validation)
- Request body schema (with example)

### 3. Response
- Success response schema (with example)
- Error response schemas (by HTTP status code)
- Pagination info (for list endpoints)
- Rate limit headers

### 4. Example
```bash
curl -X {method} "{url}" \
  -H "Authorization: Bearer {token}" \
  -H "Content-Type: application/json" \
  -d '{request_body}'
```

### 5. Error Handling
| Status Code | Meaning | When It Occurs |
|-------------|---------|----------------|
| {code}      | {meaning} | {condition} |

## Style Guide
- Use active voice
- Include real-world examples
- Show complete request/response cycles
- Document error scenarios as thoroughly as success
- Include code examples in multiple languages (curl, C#, JavaScript, Python)
