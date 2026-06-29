# Project Context

## Project Name
MerchantOS — Multi-tenant e-commerce platform

## Overview
MerchantOS is a SaaS e-commerce platform serving 10,000+ daily active merchants. 
The platform enables merchants to manage products, process orders, handle payments,
and analyze sales data. We are currently migrating from a monolithic Ruby on Rails
application to a microservices architecture (.NET 8).

## Current Quarter OKRs
1. **Revenue Growth**: Launch multi-currency checkout to enable EU market expansion
2. **Platform Reliability**: Achieve 99.95% uptime (up from 99.7%)
3. **Developer Velocity**: Reduce feature lead time from 14 days to 7 days
4. **Security**: Complete SOC 2 Type II certification

## Stakeholders
| Role | Name | Contact |
|------|------|---------|
| CEO | Strategic AI | Via CEO agent |
| VP Product | Product AI | Via PM agent |
| Engineering Lead | Tech Lead AI | Via Tech Lead agent |

## Technology Stack
- **Backend**: .NET 8, ASP.NET Core, Entity Framework Core, Dapper
- **Frontend**: React 18, Next.js 14, Tailwind CSS, TypeScript
- **Mobile**: React Native / Expo (iOS + Android)
- **Database**: Azure SQL, Redis Cache
- **Messaging**: RabbitMQ
- **Infrastructure**: Azure, Kubernetes (AKS), Terraform
- **CI/CD**: GitHub Actions, Azure DevOps
- **Monitoring**: Azure Monitor, Datadog, Serilog

## Architecture Style
Clean Architecture with CQRS pattern, moving toward event-driven microservices.

## Key Dates
- Q3 2024: Multi-currency MVP (Sprint 12-14)
- Q4 2024: SOC 2 audit
- Q1 2025: Platform API for third-party integrations

## Constraints
- Must maintain backward compatibility during migration
- PCI-DSS compliance for payment processing
- GDPR compliance for EU user data
- 99.9% uptime SLA for enterprise tier merchants
