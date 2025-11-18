[[0101 Session-Cookie Authentication]]
# Mathematical Proof: timeRemaining < timeElapsed = 50% Point

## The Actual Source Code Condition

From ASP.NET Core's `CookieAuthenticationHandler.cs`:

```csharp
private async Task CheckForRefreshAsync(AuthenticationTicket ticket)
{
    var currentUtc = Clock.UtcNow;
    var issuedUtc = ticket.Properties.IssuedUtc;
    var expiresUtc = ticket.Properties.ExpiresUtc;

    if (issuedUtc != null && expiresUtc != null && Options.SlidingExpiration)
    {
        var timeElapsed = currentUtc.Subtract(issuedUtc.Value);
        var timeRemaining = expiresUtc.Value.Subtract(currentUtc);

        // THE KEY LINE:
        if (timeRemaining < timeElapsed)
        {
            await RequestRefreshAsync(ticket);
        }
    }
}
```

**There is NO explicit "50%" or "halfway" in the code.** But let's prove mathematically why this condition equals 50%.

---

## Mathematical Proof

### Variables:

- `IssuedUtc` = Time when session was created (T₀)
- `ExpiresUtc` = Time when session expires (T₁)
- `CurrentUtc` = Current time (T)
- `TotalDuration` = T₁ - T₀ (the full ExpireTimeSpan)

### Calculations:

```
timeElapsed = CurrentUtc - IssuedUtc
            = T - T₀

timeRemaining = ExpiresUtc - CurrentUtc
              = T₁ - T
```

### The Condition:

```
if (timeRemaining < timeElapsed)
```

Substituting:

```
T₁ - T < T - T₀
```

Add T to both sides:

```
T₁ < 2T - T₀
```

Add T₀ to both sides:

```
T₁ + T₀ < 2T
```

Divide both sides by 2:

```
(T₁ + T₀) / 2 < T
```

This means:

```
T > (T₁ + T₀) / 2
```

**The midpoint between IssuedUtc and ExpiresUtc is (T₁ + T₀) / 2**

**Therefore: The condition is TRUE when CurrentUtc is past the midpoint = 50% mark!**

---

## Visual Proof with Numbers

Session: 30 minutes (IssuedUtc = 10:00, ExpiresUtc = 10:30)

```
Timeline:
10:00          10:15          10:30
  ├──────────────┼──────────────┤
  T₀       MIDPOINT=50%        T₁
```

### At 10:14 (46.67% through):

```
timeElapsed   = 10:14 - 10:00 = 14 minutes
timeRemaining = 10:30 - 10:14 = 16 minutes

Is 16 < 14? NO → Don't renew
```

### At 10:15 (Exactly 50%):

```
timeElapsed   = 10:15 - 10:00 = 15 minutes
timeRemaining = 10:30 - 10:15 = 15 minutes

Is 15 < 15? NO → Don't renew (equality, not less than)
```

### At 10:16 (53.33% through):

```
timeElapsed   = 10:16 - 10:00 = 16 minutes
timeRemaining = 10:30 - 10:16 = 14 minutes

Is 14 < 16? YES → RENEW! ✓
```

---

## Percentage Formula

To find what percentage of time has passed:

```
Percentage = (timeElapsed / TotalDuration) × 100%
           = (timeElapsed / (timeElapsed + timeRemaining)) × 100%
```

### When timeRemaining < timeElapsed:

```
If timeRemaining < timeElapsed, then:
  timeRemaining < timeElapsed
  
Divide both sides by TotalDuration:
  timeRemaining/TotalDuration < timeElapsed/TotalDuration
  
Since TotalDuration = timeElapsed + timeRemaining:
  timeRemaining/(timeElapsed + timeRemaining) < timeElapsed/(timeElapsed + timeRemaining)
  
This means:
  Percentage remaining < 50%
  
Therefore:
  Percentage elapsed > 50%
```

---

## Alternative Proof (Algebraic)

Given:

```
timeRemaining < timeElapsed
```

We know:

```
timeElapsed + timeRemaining = TotalDuration
```

From the condition:

```
timeRemaining < timeElapsed

Add timeRemaining to both sides:
timeRemaining + timeRemaining < timeElapsed + timeRemaining

2 × timeRemaining < TotalDuration

timeRemaining < TotalDuration / 2

Therefore: timeRemaining < 50% of total
Which means: timeElapsed > 50% of total
```

---

## Test Cases

### 30 Minute Session:

|Time|Elapsed|Remaining|Remaining < Elapsed?|Renew?|% Through|
|---|---|---|---|---|---|
|10:05|5 min|25 min|25 < 5? NO|❌|16.7%|
|10:10|10 min|20 min|20 < 10? NO|❌|33.3%|
|10:15|15 min|15 min|15 < 15? NO|❌|50.0%|
|10:16|16 min|14 min|14 < 16? YES|✅|53.3%|
|10:20|20 min|10 min|10 < 20? YES|✅|66.7%|

### 60 Minute Session:

|Time|Elapsed|Remaining|Remaining < Elapsed?|Renew?|% Through|
|---|---|---|---|---|---|
|10:15|15 min|45 min|45 < 15? NO|❌|25.0%|
|10:30|30 min|30 min|30 < 30? NO|❌|50.0%|
|10:31|31 min|29 min|29 < 31? YES|✅|51.7%|
|10:45|45 min|15 min|15 < 45? YES|✅|75.0%|

---

## Conclusion

**The source code never explicitly says "50%" or "halfway"**, but the mathematical condition:

```csharp
if (timeRemaining < timeElapsed)
```

Is mathematically equivalent to:

```csharp
if (percentageElapsed > 50%)
```

This is why documentation and developers refer to it as "renewing after halfway through" or "renewing when more than 50% has passed" - it's the mathematical interpretation of the actual code condition.

---

## Why Use This Approach Instead of Explicit 50%?

ASP.NET uses `timeRemaining < timeElapsed` instead of calculating percentages because:

1. **Performance**: Avoids division operations
2. **Precision**: Works with TimeSpan directly, no floating point math
3. **Simplicity**: Two subtractions and one comparison
4. **No edge cases**: No need to handle division by zero

```csharp
// ❌ More expensive and complex:
var percentage = (timeElapsed.TotalSeconds / totalDuration.TotalSeconds) * 100;
if (percentage > 50) { }

// ✅ Faster and simpler:
if (timeRemaining < timeElapsed) { }
```

Both produce the exact same result, but the second is more efficient.