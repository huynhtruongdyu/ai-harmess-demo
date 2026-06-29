# {Endpoint Name} API

## Overview
{short description of what this endpoint does}

## Endpoint
`{METHOD} /api/v1/{path}`

## Authentication
{Required / Optional} — {Bearer token / API key / etc.}

## Request

### Path Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| {param} | {type} | {yes/no} | {description} |

### Query Parameters
| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| {param} | {type} | {yes/no} | {default} | {description} |

### Request Body
```json
{request_body_example}
```

## Response

### Success (200)
```json
{success_response_example}
```

### Error Responses
| Status Code | Description | Example |
|-------------|-------------|---------|
| {code} | {description} | {example} |

## Example
```bash
curl -X {METHOD} "https://api.merchantos.com/api/v1/{path}" \
  -H "Authorization: Bearer {token}" \
  -H "Content-Type: application/json" \
  -d '{request_body}'
```

## Notes
{any additional information, rate limits, deprecation notices, etc.}
