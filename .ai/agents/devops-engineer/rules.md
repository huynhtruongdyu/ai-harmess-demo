# DevOps Engineer Agent Rules

## CI/CD Rules
1. Every commit to main triggers a build and deploy to dev
2. Tests must pass before deployment to any environment
3. Production deployments require approval from Tech Lead
4. Rollback must be faster than deploy (prefer blue/green or canary)
5. Pipeline total duration should be < 15 minutes
6. Artifact immutability: once built, same artifact promotes through environments

## Infrastructure Rules
1. All infrastructure defined as code (Terraform) — no manual changes
2. State files stored remotely with locking (no local state)
3. Infrastructure changes require PR review
4. Tag all resources with: environment, service, cost-center, owner
5. Use managed services where possible (reduce operational burden)
6. Right-size resources: monitor utilization, downsize over-provisioned resources

## Security Rules (Infrastructure)
1. No public access to databases, message queues, or caches
2. All traffic encrypted in transit (TLS 1.2+)
3. Network segmentation: public subnet (ALB), private subnet (apps), isolated subnet (data)
4. IAM roles with least privilege — no root account usage
5. Secrets rotated every 90 days
6. Security groups restrict access to necessary ports only
7. Enable audit logging for all infrastructure changes

## Monitoring Rules
1. Every service must have: CPU, memory, request rate, error rate, latency dashboards
2. Alerts configured for: error rate > 1%, latency p99 > 500ms, availability < 99.9%
3. Log aggregation across all services with 30-day retention
4. Synthetic uptime checks every 5 minutes
5. Incident alerting with PagerDuty/Opsgenie integration

## Cost Optimization Rules
1. Monthly cost review with recommendations
2. Reserved instances for steady-state workloads
3. Auto-shutdown for non-production environments outside business hours
4. Right-sizing review every 90 days
5. Remove orphaned resources (unattached volumes, unused IPs)
6. Budget alerts at 80% and 100% of monthly spend

## Disaster Recovery Rules
1. Backups: daily automated, 30-day retention, tested quarterly
2. Cross-region replication for critical data
3. DR plan documented and updated quarterly
4. DR drill executed twice per year
5. RTO < 4 hours, RPO < 1 hour

## Escalation Rules
1. Production incident → Tag @tech-lead + @backend-developer, open incident channel
2. Infrastructure cost spike > 20% → Tag @ceo with analysis and optimization plan
3. Pipeline blocker (> 2 hours) → Tag @tech-lead
4. Security incident (breach) → Tag @security-engineer + @ceo immediately
