# Coding Standards

## General
- **Language**: C# (.NET 8), TypeScript (React/Next.js)
- **Indentation**: 4 spaces (no tabs)
- **Line length**: 120 characters max
- **Encoding**: UTF-8
- **File header**: File-scoped namespaces (C#), no BOM

## C# (.NET)
### Naming
| Element | Convention | Example |
|---------|------------|---------|
| Classes | PascalCase | `OrderService`, `ProductController` |
| Interfaces | IPascalCase | `IOrderRepository`, `IPaymentGateway` |
| Methods | PascalCase | `GetOrderById()`, `CalculateTotal()` |
| Properties | PascalCase | `OrderId`, `TotalAmount` |
| Fields (private) | _camelCase | `_orderRepository`, `_logger` |
| Local variables | camelCase | `orderTotal`, `customerId` |
| Constants | PascalCase | `MaxOrderItems`, `DefaultPageSize` |
| Enums | PascalCase | `OrderStatus { Pending, Shipped, Delivered }` |

### Code Style
- Use `var` when type is obvious (`var order = new Order()`)
- Use primary constructors for simple DI scenarios
- Use expression-bodied members for simple properties/methods
- Prefer records for DTOs: `public record OrderResponse(int Id, decimal Total);`
- Always use `async Task` over `async void` (except event handlers)
- Use `CancellationToken` in all async method signatures

### File Organization (per service)
```
src/{ServiceName}/
├── Domain/
│   ├── Entities/
│   ├── ValueObjects/
│   └── Interfaces/
├── Application/
│   ├── Commands/
│   ├── Queries/
│   ├── Services/
│   └── DTOs/
├── Infrastructure/
│   ├── Persistence/
│   ├── Services/
│   └── Configuration/
└── Api/
    ├── Controllers/
    ├── Middleware/
    └── Configuration/
```

## TypeScript / React
### Naming
| Element | Convention | Example |
|---------|------------|---------|
| Components | PascalCase | `ProductCard`, `OrderList` |
| Hooks | useCamelCase | `useProducts`, `useOrder` |
| Functions | camelCase | `formatCurrency`, `calculateTotal` |
| Interfaces | PascalCase | `ProductProps`, `OrderResponse` |
| Types | PascalCase | `CurrencyCode`, `OrderStatus` |
| Files | PascalCase | `ProductCard.tsx`, `useProducts.ts` |

### Code Style
- Strict TypeScript mode — no `any`
- Prefer interfaces over type aliases for object shapes
- Use functional components with hooks (no class components)
- Use named exports, not default exports
- CSS: Tailwind utility classes, extract to components for repetition
- State management: React Query for server state, Zustand for UI state

## Testing
- Minimum 80% line coverage for all code
- Tests follow naming: `{Method}_{Scenario}_{ExpectedResult}`
- Use realistic test data (not "test", "foo", "bar")
- Clean up test data (dispose, rollback transactions)
- Flaky tests must be fixed or quarantined within 24 hours
