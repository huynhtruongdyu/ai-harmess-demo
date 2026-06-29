# Threat Modeling Prompt

## Context
Perform threat modeling for {system_component} using the STRIDE methodology.

## System Description
- **Component**: {component_name}
- **Architecture**: {architecture_diagram_or_reference}
- **Data Flow**: {data_flow_description}
- **Trust Boundaries**: {trust_boundaries}
- **Technologies**: {technologies_used}

## STRIDE Analysis

### Spoofing
Can an attacker pretend to be someone or something else?
- Authentication mechanisms
- API key/token security
- mTLS for service-to-service communication

### Tampering
Can an attacker modify data in transit or at rest?
- Data integrity checks
- Signing/checksums
- Audit logging

### Repudiation
Can an attacker deny performing an action?
- Audit trails
- Non-repudiation mechanisms
- Immutable logs

### Information Disclosure
Can sensitive data be exposed to unauthorized parties?
- Encryption at rest and in transit
- Access controls
- Data classification handling

### Denial of Service
Can an attacker disrupt service availability?
- Rate limiting
- Resource quotas
- Auto-scaling
- DDoS protection

### Elevation of Privilege
Can an attacker gain higher permissions?
- Principle of least privilege
- Role-based access control
- Authorization checks on every request

## Threat Template
```
### Threat T-{number}: {threat_name}
- **STRIDE Category**: {category}
- **Risk**: {CVSS_score} ({severity})
- **Description**: {description}
- **Affected Component**: {component}
- **Attack Vector**: {attack_vector}
- **Impact**: {business_impact}
- **Mitigation**: {mitigation_strategy}
- **Detection**: {detection_method}
- **Status**: {mitigated / accepted / in_progress / not_started}
```

## Action Items
| Threat ID | Action | Owner | Due Date |
|-----------|--------|-------|----------|
| T-{n} | {action} | {owner} | {date} |
