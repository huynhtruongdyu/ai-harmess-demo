# Compliance Check Prompt

## Context
Evaluate the {feature_name} or {system_component} for compliance with {compliance_standard}.

## Compliance Standard
**Standard**: {standard} (e.g., GDPR, SOC 2, PCI-DSS, HIPAA)
**Relevant Controls**: {controls}

## Assessment Criteria

### GDPR (if applicable)
- [ ] Data Processing Inventory maintained
- [ ] Lawful basis for processing documented
- [ ] Privacy Notice provided to data subjects
- [ ] Consent mechanism implemented (where required)
- [ ] Data Subject Access Request (DSAR) process in place
- [ ] Right to Erasure (Article 17) implemented
- [ ] Data Protection Impact Assessment (DPIA) completed
- [ ] Data Processing Agreement (DPA) with sub-processors
- [ ] Breach notification procedure documented
- [ ] Data minimization principle applied
- [ ] Storage limitation (retention periods defined)
- [ ] Cross-border data transfer mechanisms (SCCs, BCRs)

### SOC 2 (if applicable)
- [ ] Security: Access controls, encryption, monitoring
- [ ] Availability: Incident response, disaster recovery
- [ ] Processing Integrity: Data validation, error handling
- [ ] Confidentiality: Access controls, encryption, NDAs
- [ ] Privacy: PII handling, consent, notice

### PCI-DSS (if applicable)
- [ ] Cardholder data never stored unencrypted
- [ ] Network segmentation for CDE
- [ ] Access control for cardholder data
- [ ] Regular security scanning and penetration testing
- [ ] Security policies and procedures documented

## Findings Report
| Control ID | Requirement | Status | Evidence | Gap | Remediation |
|-----------|-------------|--------|----------|-----|-------------|
| {id} | {requirement} | ✅/⚠️/❌ | {evidence} | {gap} | {plan} |

## Overall Assessment
{compliant / partially_compliant / non_compliant}

## Required Actions Before Launch
1. {action_1}
2. {action_2}
