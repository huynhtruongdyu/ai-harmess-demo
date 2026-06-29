# Architecture Decision Records

## ADR-001: Adopt .NET 8 for Microservices
- **Status**: ACCEPTED
- **Date**: 2024-01-15
- **Context**: Monolithic Rails app reaching scaling limits, need to modernize
- **Decision**: Migrate to .NET 8 with ASP.NET Core for new microservices
- **Consequences**: +Team training needed, +Better performance, +Strong typing

## ADR-002: PostgreSQL for Service Data Stores
- **Status**: SUPERSEDED by ADR-006
- **Date**: 2024-02-01
- **Decision**: Use PostgreSQL for all new microservice data stores
- **Superseded by**: ADR-006 (Azure SQL for consistency with legacy monolith)

## ADR-003: RabbitMQ for Event Bus
- **Status**: ACCEPTED
- **Date**: 2024-02-10
- **Context**: Need async communication between services during migration
- **Decision**: Adopt RabbitMQ with MassTransit for message handling
- **Consequences**: +Proven technology, +Team familiarity, -Requires self-hosting

## ADR-004: CQRS with MediatR
- **Status**: ACCEPTED
- **Date**: 2024-03-01
- **Context**: Need to separate read and write models for performance
- **Decision**: Use CQRS pattern with MediatR library for command/query dispatch
- **Consequences**: +Clear separation, +Testability, -More boilerplate

## ADR-005: Clean Architecture for All Services
- **Status**: ACCEPTED
- **Date**: 2024-03-15
- **Context**: Need consistent architecture across services
- **Decision**: Standardize on Clean Architecture (Domain, Application, Infrastructure, API)
- **Consequences**: +Consistency, +Testability, +Learning curve for new devs

## ADR-006: Azure SQL for Data Store
- **Status**: ACCEPTED
- **Date**: 2024-04-01
- **Context**: Supersedes ADR-002. Need consistency with legacy monolith's SQL Server
- **Decision**: Use Azure SQL for all service data stores
- **Consequences**: +Consistency, +Managed service, -Vendor lock-in risk

## ADR-007: Payment Provider Abstraction
- **Status**: ACCEPTED
- **Date**: 2024-11-15
- **Context**: Multi-currency checkout needs multiple payment providers
- **Decision**: Introduce IPaymentGateway abstraction with Strategy pattern
- **Consequences**: +Provider flexibility, +Additional complexity

## ADR-008: API Versioning via URL Prefix
- **Status**: ACCEPTED
- **Date**: 2024-11-16
- **Context**: Need ability to evolve APIs without breaking existing clients
- **Decision**: Version APIs via URL prefix: `/api/v1/`, `/api/v2/`
- **Consequences**: +Clear versioning, +Easy to understand, -URL pollution
