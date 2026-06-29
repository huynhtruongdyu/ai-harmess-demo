# GraphQL Schema Design Prompt

## Context
Design a GraphQL schema for {domain_name}.

## Requirements
- **Domain Entities**: {entities}
- **Queries Needed**: {query_requirements}
- **Mutations Needed**: {mutation_requirements}
- **Subscriptions**: {subscription_requirements}
- **Clients**: {clients}

## Design Guidelines

### Schema Structure
```graphql
# Types should represent domain entities
type Product {
  id: ID!
  name: String!
  description: String
  price: Money!
  currency: Currency!
  category: Category!
  variants: [ProductVariant!]!
  createdAt: DateTime!
  updatedAt: DateTime!
}

# Input types for mutations
input CreateProductInput {
  name: String!
  description: String
  price: Decimal!
  currency: CurrencyCode!
  categoryId: ID!
}

# Response types with errors
type ProductPayload {
  product: Product
  errors: [UserError!]
}
```

### Naming Conventions
- Types: PascalCase, singular nouns: `Product`, `User`, `Order`
- Fields: camelCase: `firstName`, `createdAt`
- Enums: PascalCase, UPPER_CASE values: `CurrencyCode { USD, EUR, GBP }`
- Inputs: PascalCase with `Input` suffix: `CreateProductInput`
- Payloads: PascalCase with `Payload` suffix: `ProductPayload`
- Queries: camelCase, descriptive: `product(id: ID!)`, `products(filter: ProductFilter)`
- Mutations: verbNoun in camelCase: `createProduct`, `updateProduct`, `deleteProduct`

### Best Practices
- Always return `errors: [UserError!]` in mutation payloads
- Use connection/pagination pattern for list queries (Relay spec or simple)
- Limit query depth (max 5 levels)
- Implement query cost analysis to prevent expensive queries
- Use DataLoader for N+1 prevention
- Deprecate fields with `@deprecated` directive instead of removing
- Secure with authentication and authorization resolvers

### Pagination (Connection Pattern)
```graphql
type ProductConnection {
  edges: [ProductEdge!]!
  pageInfo: PageInfo!
  totalCount: Int!
}

type ProductEdge {
  node: Product!
  cursor: String!
}

type PageInfo {
  hasNextPage: Boolean!
  hasPreviousPage: Boolean!
  startCursor: String
  endCursor: String
}
```
