# Security Review Prompt

## Context
Perform a security review of the following code or architecture change.

## Input
- **Change Description**: {change_description}
- **Code/Architecture**: {change_reference}
- **Component**: {component_name}
- **Data Classification**: {data_classification}
- **Threat Model**: {threat_model_reference}

## Review Areas

### Authentication & Authorization
- [ ] Proper authentication mechanism in place?
- [ ] Authorization checked server-side (not just UI)?
- [ ] Least privilege principle applied?
- [ ] Session management secure (rotation, expiration)?
- [ ] JWT properly validated (sig, exp, iss, aud)?

### Input Validation
- [ ] All inputs validated server-side?
- [ ] SQL injection prevention (parameterized queries)?
- [ ] XSS prevention (output encoding)?
- [ ] File upload validation (type, size, content)?
- [ ] SSRF prevention (URL validation, allowlisting)?

### Data Protection
- [ ] Data encrypted at rest (AES-256)?
- [ ] Data encrypted in transit (TLS 1.2+)?
- [ ] PII properly handled and minimized?
- [ ] Secrets stored in secret manager (not code)?
- [ ] Audit logging for sensitive operations?

### Infrastructure
- [ ] Network segmentation (public/private/isolated)?
- [ ] Rate limiting configured?
- [ ] WAF rules applied?
- [ ] Security groups least-privilege?
- [ ] Container security (non-root user, minimal base image)?

## Risk Assessment
| Finding | Severity | CVSS | Recommendation |
|---------|----------|------|----------------|
| {finding} | {sev} | {cvss} | {recommendation} |

## Decision
{approve / changes_required / blocked}
