# DevOps Engineer Agent Examples

## Example 1: CI/CD Pipeline Configuration

```yaml
# .github/workflows/deploy.yaml
name: Deploy Currency Service

on:
  push:
    branches: [main]
    paths: ["src/services/currency/**"]

env:
  SERVICE_NAME: currency-service
  ACR_NAME: merchantosacr
  AKS_CLUSTER: merchantos-aks

jobs:
  build:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: docker/setup-buildx-action@v3
      - name: Build and push
        uses: docker/build-push-action@v5
        with:
          context: src/services/currency
          push: true
          tags: ${{ env.ACR_NAME }}.azurecr.io/${{ env.SERVICE_NAME }}:${{ github.sha }}

  deploy-dev:
    needs: build
    environment: dev
    steps:
      - uses: azure/setup-kubectl@v4
      - name: Deploy to dev
        run: |
          kubectl set image deployment/${{ env.SERVICE_NAME }} \
            ${{ env.SERVICE_NAME }}=${{ env.ACR_NAME }}.azurecr.io/${{ env.SERVICE_NAME }}:${{ github.sha }} \
            -n dev

  deploy-staging:
    needs: deploy-dev
    environment: staging
    steps:
      - name: Run integration tests
        run: dotnet test tests/Currency.IntegrationTests
      - name: Deploy to staging
        run: |
          helm upgrade ${{ env.SERVICE_NAME }} ./helm/currency-service \
            --set image.tag=${{ github.sha }} \
            -n staging

  deploy-production:
    needs: deploy-staging
    environment: production
    steps:
      - name: Deploy to production (canary 10%)
        run: |
          helm upgrade ${{ env.SERVICE_NAME }} ./helm/currency-service \
            --set image.tag=${{ github.sha }} \
            --set canary.enabled=true \
            --set canary.weight=10 \
            -n production
```

## Example 2: Infrastructure as Code (Terraform)

```hcl
# terraform/modules/currency-service/main.tf
resource "azurerm_container_app" "currency_service" {
  name                         = "currency-service-${var.environment}"
  container_app_environment_id = var.container_env_id
  resource_group_name          = var.resource_group_name
  revision_mode                = "Single"

  template {
    container {
      name   = "currency-service"
      image  = "${var.acr_name}.azurecr.io/currency-service:${var.image_tag}"
      cpu    = 0.5
      memory = "1Gi"

      env {
        name        = "ASPNETCORE_ENVIRONMENT"
        value       = var.environment
      }
      env {
        name        = "ExchangeRate__ApiKey"
        secret_name = "exrate-api-key"
      }
    }

    scale {
      min_replicas = 1
      max_replicas = 10
      rule {
        name = "http-scaling"
        custom {
          type = "http"
          metadata = {
            concurrentRequests = "100"
          }
        }
      }
    }
  }
}

resource "azurerm_redis_cache" "currency_cache" {
  name                = "currency-cache-${var.environment}"
  resource_group_name = var.resource_group_name
  location            = var.location
  capacity            = var.environment == "production" ? 2 : 1
  family              = "C"
  sku_name            = "Standard"
  enable_non_ssl_port = false
  minimum_tls_version = "1.2"

  redis_configuration {
    maxmemory_policy = "allkeys-lru"
  }
}
```

## Example 3: Incident Post-Mortem

```
Post-Mortem: INC-2024-11-20 — Currency Service Latency Spike

Date: 2024-11-20 14:32 UTC
Severity: P1 (High)
Duration: 47 minutes
Detected: Datadog alert "currency-service-p99-latency > 500ms"

Timeline:
  14:32 — Alert triggered (p99 latency: 1.2s, normal: 120ms)
  14:35 — On-call engineer acknowledged
  14:38 — Identified: exchange rate provider API latency
  14:42 — Circuit breaker opened (after 3 failures)
  14:45 — Served cached rates, latency returned to 45ms
  14:50 — Switched to fallback provider (ExchangeRateHost)
  15:02 — Primary provider recovered, circuit breaker half-open
  15:19 — Full recovery, all traffic on primary provider

Root Cause:
  OpenExchangeRates API degraded due to upstream DNS failure.
  Their p99 latency went from 50ms to 3.2s.

What Went Well:
  - Circuit breaker prevented cascading failures
  - Cached rates served during outage (no user-facing errors)
  - Fallback provider configured and working

Action Items:
  1. [P1] Reduce circuit breaker timeout from 60s to 30s — DONE
  2. [P2] Add health check endpoint for exchange rate provider
  3. [P2] Configure multi-region fallback for provider API
  4. [P3] Document incident in runbook

Lessons Learned:
  - Stale cache is better than no cache — keep TTL but serve stale on error
  - Need automated failover testing in staging
```
