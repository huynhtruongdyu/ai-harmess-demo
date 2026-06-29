# Frontend Developer Agent Rules

## Component Rules
1. Every component must handle: loading state, error state, empty state, success state
2. Prop interfaces must be explicit — no `any` types
3. Use semantic HTML elements (button, nav, main, section, aside)
4. All interactive elements must be keyboard accessible
5. Images must have alt text and explicit width/height
6. Forms must have proper label associations and error announcements
7. Use CSS Grid for layout, Flexbox for component-level alignment

## TypeScript Rules
1. Strict mode required — no exceptions
2. Define interfaces in separate `.types.ts` files when shared
3. Prefer `interface` over `type` for object shapes
4. Use discriminated unions for complex state representations
5. Generic components must have meaningful type parameter names

## Testing Rules
1. Component tests: render → interact → assert (Testing Library queries)
2. E2E tests: user flow scenarios (login → browse → purchase)
3. Mock API at the service layer, not component layer
4. Snapshot tests only for stable, design-system components
5. Accessibility tests: `axe-core` integration in E2E

## Performance Rules
1. Lazy load below-the-fold components with `next/dynamic`
2. Debounce search inputs (300ms minimum)
3. Memoize expensive computations (useMemo, useCallback)
4. Virtualize long lists (react-window or @tanstack/virtual)
5. Optimize images: WebP format, responsive sizes, lazy loading

## Accessibility Rules
1. Color contrast ratio must meet WCAG AA (4.5:1 normal, 3:1 large)
2. All interactions must work with keyboard only
3. ARIA labels on all icon-only buttons
4. Focus indicators visible (not removed)
5. Announce dynamic content changes to screen readers
6. Error messages must be programmatically associated with inputs

## Escalation Rules
1. API contract mismatch → Tag @backend-developer with specific endpoint and expected response
2. Design/UX ambiguity → Tag @tech-lead for clarification
3. Browser-specific bug → Document environment and repro steps, assign to @tech-lead
