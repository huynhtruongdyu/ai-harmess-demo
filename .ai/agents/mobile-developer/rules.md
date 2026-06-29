# Mobile Developer Agent Rules

## Platform Rules
1. Test on both iOS and Android before marking complete
2. Handle platform-specific permissions (camera, location, notifications)
3. Support both light and dark mode themes
4. Handle dynamic type / font scaling (accessibility)
5. Support device orientation changes where appropriate
6. Handle notches, punch holes, and status bar properly

## Offline Rules
1. Cache product catalog for offline browsing
2. Queue user actions when offline and sync when online
3. Show clear "you are offline" banner
4. Handle conflict resolution for offline modifications
5. Provide visual feedback for sync status

## Performance Rules
1. Use FlatList (not ScrollView) for lists > 20 items
2. Memoize expensive components with React.memo
3. Lazy load images with progressive JPEG/WebP
4. Use Hermes engine for Android (enabled by default in Expo)
5. Code-split routes with lazy loading
6. Minimize bridge calls between JS and native

## Testing Rules
1. Test on real devices (not just simulators) before release
2. Test on low-end devices (3+ year old models)
3. Test on latest OS versions + previous 2 major versions
4. Test with poor network conditions (throttling)
5. Test accessibility with screen readers (VoiceOver, TalkBack)

## App Store Rules
1. No debug flags or test code in release builds
2. Privacy manifest required for iOS (privacy tracked APIs)
3. Data Safety section required for Google Play
4. App icons and screenshots for all required sizes
5. Version code/build number must increment for each release
6. In-app purchase products configured before submission

## Escalation Rules
1. Platform API not available on one OS → Tag @tech-lead with workaround options
2. App store rejection → Document reason, reassign for fix
3. Native module requirement → Tag @solution-architect for architecture decision
