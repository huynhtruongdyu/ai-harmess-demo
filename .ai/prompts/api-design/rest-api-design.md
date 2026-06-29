# REST API Design Prompt

## Context
Design a RESTful API for {resource_name}.

## Requirements
- **Resource**: {resource_name}
- **Operations Needed**: {operations} (CRUD + any custom operations)
- **Clients**: {clients} (web, mobile, third-party)
- **Performance Requirements**: {perf_requirements}
- **Security Requirements**: {security_requirements}

## Design Guidelines

### URL Patterns
| Operation | HTTP Method | URL Pattern |
|-----------|-------------|-------------|
| List | GET | `/api/v1/{resources}` |
| Get | GET | `/api/v1/{resources}/{id}` |
| Create | POST | `/api/v1/{resources}` |
| Update | PUT | `/api/v1/{resources}/{id}` |
| Partial Update | PATCH | `/api/v1/{resources}/{id}` |
| Delete | DELETE | `/api/v1/{resources}/{id}` |

### Naming Conventions
- Use nouns (not verbs) for resources: `/orders`, not `/getOrders`
- Plural nouns for collections: `/products`, `/users`
- Nest related resources: `/orders/{id}/items`
- Use kebab-case for multi-word resources: `/order-templates`
- Version via URL prefix: `/api/v1/`, `/api/v2/`

### Response Format
```json
{
  "success": true,
  "data": { ... },
  "error": null,
  "meta": {
    "page": 1,
    "pageSize": 20,
    "totalCount": 150,
    "totalPages": 8
  }
}
```

### Error Responses
```json
{
  "success": false,
  "data": null,
  "error": {
    "code": "VALIDATION_ERROR",
    "message": "The request was invalid",
    "details": [
      { "field": "email", "message": "Email is required" }
    ]
  }
}
```

### Pagination
- Use page/pageSize parameters
- Include totalCount in response metadata
- Default pageSize: 20, maximum: 100
- Cursor-based pagination for high-volume resources

### Filtering, Sorting, Searching
- Filter: `GET /products?category=electronics&price_lte=100`
- Sort: `GET /products?sort=-createdAt` (prefix `-` for descending)
- Search: `GET /products?search=wireless+headphones`
