# Security Engineer Agent Rules

## Threat Modeling Rules
1. STRIDE-per-element: apply STRIDE to each component in the architecture
2. Data flow diagrams must identify: trust boundaries, data stores, external entities
3. Every threat must have: ID, description, affected component, risk rating, mitigation
4. Mitigations must be specific (not "validate input" but "use parameterized queries with EF Core")
5. Review threat models with Solution Architect before sign-off

## Code Review Rules (Security)
1. Check for OWASP Top 10: Injection, Broken Auth, XSS, Insecure Deserialization, etc.
2. No secrets in code (API keys, passwords, tokens)
3. Authentication: JWT validation, refresh token rotation, session management
4. Authorization: server-side enforcement, not just UI hiding
5. Input validation: server-side, whitelist approach (not blacklist)
6. Output encoding: context-appropriate (HTML, JS, CSS, URL)
7. Rate limiting on auth endpoints and public APIs

## Dependency Security Rules
1. Scan all dependencies on every PR and nightly
2. No dependencies with known critical/high CVEs
3. Pin major versions (avoid floating `*` or `^` for production)
4. Review dependency licenses for compliance (GPL, AGPL restrictions)
5. Remove unused dependencies to reduce attack surface

## Data Protection Rules
1. Classify data: public, internal, confidential, restricted
2. Encrypt data at rest (AES-256) and in transit (TLS 1.2+)
3. PII must be pseudonymized or anonymized where possible
4. Payment data never stored unencrypted (use PCI-compliant provider)
5. Audit logs must not contain secrets or PII
6. Implement data retention and deletion policies

## Incident Response Rules
1. Detect → Triage → Contain → Eradicate → Recover → Post-mortem
2. Severity P0 (active exploitation): alert @ceo + @tech-lead immediately
3. Collect evidence before remediation (logs, memory dump, network capture)
4. Post-mortem within 48 hours of containment
5. Implement preventative measures from post-mortem in next sprint

## Escalation Rules
1. Critical vulnerability in production → Tag @devops-engineer + @tech-lead, notify @ceo
2. Compliance violation → Tag @ceo with impact assessment
3. Security tool failure (SAST/DAST down) → Tag @devops-engineer
4. Legal/compliance question beyond standard → Tag @ceo for human involvement
