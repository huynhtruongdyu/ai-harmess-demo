# Mobile Developer Agent Examples

## Example 1: Platform-Specific Component

```tsx
import { Platform } from 'react-native';

export function CurrencySelector({ selected, currencies, onChange }) {
  const pickerProps = Platform.select({
    ios: {
      // iOS native picker with wheel style
      style: { height: 200 },
    },
    android: {
      // Android dropdown with Material Design
      mode: 'dropdown',
      dropdownIconColor: '#666',
    },
    default: {},
  });

  return (
    <View style={styles.container}>
      <Picker
        selectedValue={selected}
        onValueChange={onChange}
        {...pickerProps}
      >
        {currencies.map((c) => (
          <Picker.Item key={c.code} label={`${c.symbol} ${c.code}`} value={c.code} />
        ))}
      </Picker>
    </View>
  );
}
```

## Example 2: Offline-First Hook

```typescript
// hooks/useOfflineCurrency.ts
import { useNetInfo } from '@react-native-community/netinfo';
import { useMMKVStorage } from 'react-native-mmkv';

const CACHE_KEY = 'cached_rates';

export function useOfflineCurrency() {
  const { isConnected } = useNetInfo();
  const [cachedRates, setCachedRates] = useMMKVStorage(CACHE_KEY, storage);

  const getRate = async (from: string, to: string) => {
    if (isConnected) {
      try {
        const rate = await fetchRate(from, to);
        // Update cache
        setCachedRates({ ...cachedRates, [`${from}_${to}`]: { rate, timestamp: Date.now() } });
        return rate;
      } catch {
        // Fall through to cache
      }
    }
    // Return cached rate (or stale)
    const cached = cachedRates?.[`${from}_${to}`];
    if (cached) {
      const isStale = Date.now() - cached.timestamp > 15 * 60 * 1000; // 15 min
      return { rate: cached.rate, isStale };
    }
    throw new OfflineRateError('No exchange rate available offline');
  };

  return { getRate, isOnline: isConnected };
}
```

## Example 3: Deep Linking Configuration

```json
{
  "expo": {
    "plugins": [
      [
        "expo-linking",
        {
          "schemes": ["merchantos"]
        }
      ]
    ],
    "ios": {
      "associatedDomains": ["applinks:merchantos.com"]
    },
    "android": {
      "intentFilters": [
        {
          "action": "VIEW",
          "data": [
            { "scheme": "https", "host": "merchantos.com", "pathPrefix": "/product/" },
            { "scheme": "merchantos", "host": "product" }
          ],
          "category": ["BROWSABLE", "DEFAULT"]
        }
      ]
    }
  }
}
```
