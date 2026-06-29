# Generate Tests Prompt

## Context
Write comprehensive tests for the {component} component/feature.

## Implementation
```code
{code_to_test}
```

## Test Requirements
- **Testing Framework**: {test_framework} (e.g., xUnit, Vitest, Jest)
- **Mocking Framework**: {mocking_framework} (e.g., Moq, NSubstitute, MSW)
- **Coverage Target**: 80% minimum
- **Test Types Needed**: Unit + Integration

## Instructions
Write tests following these patterns:

### Unit Tests
- Test each public method in isolation
- Mock external dependencies
- Test: happy path, error path, edge cases
- Follow naming convention: `{MethodName}_{Scenario}_{ExpectedResult}`

### Integration Tests
- Test the endpoint end-to-end with real dependencies
- Use test containers or in-memory alternatives
- Verify: status codes, response bodies, side effects
- Clean up test data after each test

## Test Template (Unit)
```csharp
[Fact]
public void {Method}_{Scenario}_Returns{Expected}()
{
    // Arrange
    // Act
    // Assert
}
```

## Test Template (Integration)
```csharp
[Fact]
public async Task {Endpoint}_{Scenario}_Returns{StatusCode}()
{
    // Arrange
    // Act
    // Assert
}
```

## Edge Cases to Cover
- Null inputs
- Empty collections
- Boundary values
- Concurrent access
- Timeout scenarios
- Unauthorized access
- Invalid data formats
- Missing resources
- Duplicate submissions
- Large payloads
