# Security

## Agent Security Model

### Least Privilege
Each agent operates with the minimum permissions needed to perform its role:

| Agent | Memory Read | Memory Write | Tools | Production Access |
|-------|-------------|--------------|-------|-------------------|
| CEO | Strategic | ADRs | Read, LLM | No |
| PM | Product | Sprint | Read, Write, LLM | No |
| BA | Requirements | Rules, Glossary | Read, Write | No |
| Architect | Architecture | ADRs, Standards | Read, Write, LLM | No |
| Tech Lead | All | Sprint, Issues | Read, Write | No |
| Backend Dev | Technical | Issues | All except DB prod | Dev only |
| Frontend Dev | Technical | — | All except DB prod | No |
| Mobile Dev | Technical | — | All except DB | No |
| QA | All | Issues | Test, Read | Read only |
| Security | All | ADRs, Standards | Scan, Read | Read only |
| DevOps | All | Infrastructure | All | Yes (with approval) |
| Reviewer | Code | Issues | Read, Scan | No |
| Docs Writer | Documentation | Context | Read, Write | No |

### Tool Restrictions
- **Database tool**: Production write requires multi-party approval
- **Deploy tool**: Requires QA sign-off + Tech Lead approval
- **Shell tool**: Restricted commands (no rm -rf, no package install without approval)
- **API tool**: Rate-limited, production write operations require confirmation

## Secret Management

1. **No secrets in code**: API keys, connection strings, passwords never committed
2. **Environment variables**: Secrets passed via `${VAR_NAME}` references in config
3. **Secret store**: Production secrets in Azure Key Vault / HashiCorp Vault
4. **Rotation**: Secrets rotated every 90 days, enforced by automated policy
5. **Audit**: All secret access logged and monitored

## Audit Logging

All agent actions are logged:

| Event | Logged Data | Retention |
|-------|-------------|-----------|
| Task assignment | Agent, task, timestamp | 90 days |
| File modification | Agent, file, diff summary | 90 days |
| Tool execution | Agent, tool, input hash | 90 days |
| LLM call | Agent, model, token count | 30 days |
| Approval/Rejection | Agent, target, decision | 90 days |
| Escalation | Agent, reason, resolution | 90 days |

## Secure Communication

1. All inter-agent communication via internal channels (no external network)
2. External API calls (GitHub, Jira) use HTTPS + tokens
3. LLM API calls use TLS 1.2+ with API key authentication
4. No PII or secrets included in LLM prompts

## Compliance Checks

Built-in compliance checks for:
- **GDPR**: Data minimization, right to erasure, DPIA
- **SOC 2**: Security, availability, processing integrity, confidentiality, privacy
- **PCI-DSS**: Cardholder data protection, access control, monitoring

See `.ai/prompts/security/compliance-check.md` for detailed compliance assessment prompts.
