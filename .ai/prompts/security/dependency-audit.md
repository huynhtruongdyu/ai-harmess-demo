# Dependency Audit Prompt

## Context
Audit the dependencies of {project} for security vulnerabilities, license compliance, and maintenance health.

## Input
- **Dependency Manifest**: {manifest_path}
- **Lock File**: {lock_file_path}
- **Previous Audit**: {previous_audit_date}

## Audit Checklist

### Security Vulnerabilities
- [ ] Scan all direct and transitive dependencies for CVEs
- [ ] Identify dependencies with critical/high severity vulnerabilities
- [ ] Check for known exploits (CVE, CWE, GHSA references)
- [ ] Verify vulnerability reachability (is the vulnerable code path actually used?)

### License Compliance
- [ ] Verify all dependencies have compatible licenses
- [ ] Flag copyleft licenses (GPL, AGPL) that may require source disclosure
- [ ] Flag "unknown" or "proprietary" licenses for legal review
- [ ] Ensure license attribution requirements are met

### Maintenance Health
- [ ] When was the last release? (abandoned if > 2 years)
- [ ] How many maintainers? (bus factor risk if 1)
- [ ] Are security issues actively addressed?
- [ ] Is there a clear migration path if the project is abandoned?

### Upgrade Recommendations
| Dependency | Current Version | Latest Version | Risk | Recommendation |
|------------|----------------|----------------|------|----------------|
| {pkg} | {current} | {latest} | {sev} | {recommendation} |

## Remediation Priority
| Severity | Action | Timeline |
|----------|--------|----------|
| Critical | Immediate update or replace | 24 hours |
| High | Update in current sprint | 7 days |
| Medium | Schedule update | 30 days |
| Low | Track for next quarterly review | 90 days |
