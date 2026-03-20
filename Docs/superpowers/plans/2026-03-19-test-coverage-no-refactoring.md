# Test Coverage Expansion Plan (No Application Code Changes)

> **For agentic workers:** REQUIRED: Use superpowers:subagent-driven-development (if subagents available) or superpowers:executing-plans to implement this plan. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Maximize unit test coverage across all pure-logic code paths without modifying any application source files.

**Architecture:** Each chunk adds a new test file (or extends an existing one) targeting a specific area of the codebase. Tests reference the main project via the existing ProjectReference. All tests must pass locally via `docker build -f Dockerfile.tests -t vjoy-tests .` and in CI on Windows.

**Tech Stack:** NUnit 4, .NET Framework 4.8, C#

**Constraints:**
- Zero changes to files under `vJoySerialFeeder/` — tests only
- No mocking frameworks (keep dependencies minimal)
- Follow existing test conventions (see `AxisParametersTests.cs` for style)
- Tab indentation, K&R braces, explicit access modifiers

---

## What's Already Tested

| File | Coverage |
|------|----------|
| `AxisParametersTests.cs` | AxisParameters.Transform — linear, symmetric, deadband, expo, invert, clamp, invalid params |
| `ButtonParametersTests.cs` | ButtonParameters.Transform — single/dual threshold, invert, trigger flag, edge cases |
| `Crc8Tests.cs` | Crc8 — DVB-S2 vectors, reset, determinism, all byte values |
| `ConfigurationTests.cs` | Serialization round-trips, profile CRUD, merge, ProfilesEqual |

## What's NOT Testable Without Code Changes

| Target | Blocker |
|--------|---------|
| `TriggerState.Trigger()` | Depends on `MainForm.Now` static property — no way to inject time |
| `Configuration.DoUpgrade()` paths | `Upgrade()` compares against `RuntimeVersion` which is `0.0.0.0` in test context — upgrade path never triggers |
| `ButtonBitmapMapping.Transform()` | Calls `TriggerState.Trigger()` internally |
| Protocol `parseConfig()`/`buildConfig()` | Private methods — not accessible from tests |
| `CrsfReader.map()` | Static private — not accessible from tests |
| Serial I/O, port handling | Requires hardware or serial port mocking |

---

## Chunk 1: Mapping Clamp Methods

**Branch:** `test/mapping-clamp`

**Why:** The `Clamp()` methods are `protected`, but they're called by the public `Output` setter on the `Mapping` base class. Setting `Output` directly exercises `Clamp()` and returns the clamped value. This covers the value-bounding logic for all three mapping types.

**File:** Create `vJoySerialFeeder.Tests/Mappings/MappingClampTests.cs`

### Task 1: AxisMapping.Clamp via Output setter

- [ ] **Step 1: Write tests**

```csharp
[TestFixture]
public class MappingClampTests
{
    // AxisMapping.Clamp: clamps to [0, 1]
    [Test]
    public void AxisMapping_Output_ClampsAboveOne() { ... }
    [Test]
    public void AxisMapping_Output_ClampsBelowZero() { ... }
    [Test]
    public void AxisMapping_Output_PreservesValidRange() { ... }
    [Test]
    public void AxisMapping_Output_BoundaryValues() { ... }  // exactly 0, exactly 1
}
```

**Test approach:** Create `new AxisMapping()`, set `Output = value`, assert `Output` equals clamped result.

- [ ] **Step 2: Run tests in Docker, verify pass**
- [ ] **Step 3: Commit**

### Task 2: ButtonMapping.Clamp via Output setter

- [ ] **Step 1: Write tests**

```csharp
// ButtonMapping.Clamp: returns 1 if val > 0, else 0
[Test]
public void ButtonMapping_Output_PositiveBecomesOne() { ... }
[Test]
public void ButtonMapping_Output_ZeroRemainsZero() { ... }
[Test]
public void ButtonMapping_Output_NegativeBecomesZero() { ... }
[Test]
public void ButtonMapping_Output_FractionalPositiveBecomesOne() { ... }
```

- [ ] **Step 2: Run tests in Docker, verify pass**
- [ ] **Step 3: Commit**

### Task 3: ButtonBitmapMapping.Clamp via Output setter

- [ ] **Step 1: Write tests**

```csharp
// ButtonBitmapMapping.Clamp: int cast, clamp to [0, 0xFFFF]
[Test]
public void ButtonBitmapMapping_Output_ClampsAbove65535() { ... }
[Test]
public void ButtonBitmapMapping_Output_ClampsBelowZero() { ... }
[Test]
public void ButtonBitmapMapping_Output_PreservesValidRange() { ... }
[Test]
public void ButtonBitmapMapping_Output_CastsToInt() { ... }  // 100.7f -> 100
```

- [ ] **Step 2: Run tests in Docker, verify pass**
- [ ] **Step 3: Commit**

---

## Chunk 2: Mapping Copy Methods

**Branch:** `test/mapping-copy`

**Why:** `Copy()` is public on all three mapping types. It creates a new instance with the same data fields. Tests verify field-by-field equality and independence (modifying the copy doesn't affect the original).

**File:** Create `vJoySerialFeeder.Tests/Mappings/MappingCopyTests.cs`

### Task 1: AxisMapping.Copy

- [ ] **Step 1: Write tests**

Tests should verify:
- Returns a new `AxisMapping` instance (not same reference)
- `Channel`, `Axis`, and all `Parameters` fields are copied
- Modifying the copy's Parameters doesn't affect the original (struct semantics — this should be free, but worth asserting)

- [ ] **Step 2: Run tests in Docker, verify pass**
- [ ] **Step 3: Commit**

### Task 2: ButtonMapping.Copy

- [ ] **Step 1: Write tests**

Tests should verify:
- Returns a new `ButtonMapping` instance
- `Channel`, `Button`, and `Parameters` struct are copied

- [ ] **Step 2: Run tests in Docker, verify pass**
- [ ] **Step 3: Commit**

### Task 3: ButtonBitmapMapping.Copy

- [ ] **Step 1: Write tests**

Tests should verify:
- Returns a new `ButtonBitmapMapping` instance
- `Channel` is copied
- `Parameters` array is copied (all 16 entries)
- Modifying copy's Parameters array doesn't affect original (Array.Copy gives a shallow copy of value-type structs, so this is independent)

- [ ] **Step 2: Run tests in Docker, verify pass**
- [ ] **Step 3: Commit**

---

## Chunk 3: Mapping Failsafe Methods

**Branch:** `test/mapping-failsafe`

**Why:** `Failsafe()` is public on all mapping types. It sets `Output` based on the failsafe configuration. These are simple conditionals with no external dependencies.

**File:** Create `vJoySerialFeeder.Tests/Mappings/MappingFailsafeTests.cs`

### Task 1: AxisMapping.Failsafe

- [ ] **Step 1: Write tests**

```
AxisMapping.Failsafe() at vJoySerialFeeder/Mappings/AxisMapping.cs:162-166
  if(Parameters.Failsafe >= 0)
      Output = Parameters.Failsafe/100.0f;
```

Tests:
- Failsafe = -1 (default): Output unchanged
- Failsafe = 0: Output = 0.0
- Failsafe = 50: Output = 0.5
- Failsafe = 100: Output = 1.0
- Failsafe = 200: Output = 2.0 (but Clamp caps at 1.0)

- [ ] **Step 2: Run tests in Docker, verify pass**
- [ ] **Step 3: Commit**

### Task 2: ButtonMapping.Failsafe

- [ ] **Step 1: Write tests**

```
ButtonMapping.Failsafe() at vJoySerialFeeder/Mappings/ButtonMapping.cs:122-126
  if(Parameters.Failsafe > 0)
      Output = Parameters.Failsafe == 1 ? 0 : 1;
```

Tests:
- Failsafe = 0 (default): Output unchanged
- Failsafe = 1: Output = 0 (depressed)
- Failsafe = 2: Output = 1 (pressed)
- Failsafe = 99: Output = 1 (any value > 1 means pressed)

- [ ] **Step 2: Run tests in Docker, verify pass**
- [ ] **Step 3: Commit**

### Task 3: ButtonBitmapMapping.Failsafe

- [ ] **Step 1: Write tests**

```
ButtonBitmapMapping.Failsafe() at vJoySerialFeeder/Mappings/ButtonBitmapMapping.cs:138-155
  For each of 16 bits: if Enabled && Failsafe != 0:
    Failsafe == 1: clear bit
    Failsafe == 2: set bit
```

Note: This method calls `Input = Input` to recalculate Output, which internally calls `Transform()`. Since `Transform()` uses `TriggerState`, set all `Trigger = false` in test parameters to avoid that dependency.

Tests:
- All bits disabled, Failsafe has no effect
- Single bit enabled with Failsafe=1: bit cleared
- Single bit enabled with Failsafe=2: bit set
- Mix of Failsafe=0, 1, 2 across multiple bits
- Disabled bits are not modified regardless of Failsafe value

- [ ] **Step 2: Run tests in Docker, verify pass**
- [ ] **Step 3: Commit**

---

## Chunk 4: Protocol Reader Properties

**Branch:** `test/protocol-reader-properties`

**Why:** Every protocol reader exposes `GetDefaultSerialParameters()`, `ProtocolName`, and `Configurable` as public members. These return fixed values with no side effects. Testing them locks in the protocol contracts and catches any accidental changes.

**File:** Create `vJoySerialFeeder.Tests/SerialProtocols/ProtocolReaderPropertiesTests.cs`

### Task 1: Write parameterized tests for all 9 readers

All readers can be instantiated with `new XxxReader()` — the constructors don't touch serial ports or MainForm. The `Init()` method (which sets up serial buffers) does NOT need to be called to access properties.

- [ ] **Step 1: Write tests**

Use `[TestCase]` attributes for compact coverage:

```csharp
// Readers to test: IbusReader, SbusReader, DsmReader, KissReader,
// MultiWiiReader, CrsfReader, FportReader, DummyReader, DjiControllerReader

[TestCase(typeof(CrsfReader), 420000, 8, Parity.None, StopBits.One)]
[TestCase(typeof(SbusReader), 100000, 8, Parity.Even, StopBits.Two)]
[TestCase(typeof(IbusReader), 115200, 8, Parity.None, StopBits.One)]
// ... etc for all 9
public void GetDefaultSerialParameters_ReturnsExpectedValues(
    Type readerType, int baudRate, int dataBits, Parity parity, StopBits stopBits) { ... }

[TestCase(typeof(CrsfReader), "CRSF")]
[TestCase(typeof(SbusReader), "SBUS")]
// ... etc for all 9
public void ProtocolName_ReturnsExpectedValue(Type readerType, string expected) { ... }

[TestCase(typeof(IbusReader), true)]
[TestCase(typeof(CrsfReader), false)]
// ... etc for all 9
public void Configurable_ReturnsExpectedValue(Type readerType, bool expected) { ... }
```

Use `Activator.CreateInstance(readerType)` to instantiate each reader.

- [ ] **Step 2: Run tests in Docker, verify pass**
- [ ] **Step 3: Commit**

---

## Chunk 5: VJoyNone (Null Object)

**Branch:** `test/vjoy-none`

**Why:** `VJoyNone` is the null-object implementation of `VJoyBase`. All methods should be no-ops and not throw. `GetJoysticks()` returns null. This is a quick safety net.

**File:** Create `vJoySerialFeeder.Tests/VirtualJoysticks/VJoyNoneTests.cs`

### Task 1: Write tests for all VJoyNone methods

- [ ] **Step 1: Write tests**

```csharp
[TestFixture]
public class VJoyNoneTests
{
    [Test] public void Acquire_DoesNotThrow() { ... }
    [Test] public void Release_DoesNotThrow() { ... }
    [Test] public void SetAxis_DoesNotThrow() { ... }
    [Test] public void SetButton_DoesNotThrow() { ... }
    [Test] public void SetDiscPov_DoesNotThrow() { ... }
    [Test] public void SetContPov_DoesNotThrow() { ... }
    [Test] public void SetState_DoesNotThrow() { ... }
    [Test] public void GetJoysticks_ReturnsNull() { ... }
}
```

- [ ] **Step 2: Run tests in Docker, verify pass**
- [ ] **Step 3: Commit**

---

## Chunk 6: Configuration Serialization Edge Cases

**Branch:** `test/config-serialization-edge-cases`

**Why:** The existing Configuration tests cover the happy path. These tests add coverage for edge cases: ButtonBitmapMapping serialization, mixed mapping types in a single profile, empty/null field handling, multiple profile operations, and malformed JSON.

**File:** Extend `vJoySerialFeeder.Tests/Configuration/ConfigurationTests.cs`

### Task 1: ButtonBitmapMapping round-trip

- [ ] **Step 1: Write tests**

Tests:
- Profile with ButtonBitmapMapping serializes and deserializes
- BitButtonParameters fields survive round-trip (Enabled, Invert, Button, Failsafe, TriggerEdge, TriggerDuration)

- [ ] **Step 2: Run tests in Docker, verify pass**
- [ ] **Step 3: Commit**

### Task 2: Mixed mapping types

- [ ] **Step 1: Write tests**

Tests:
- Profile with one of each mapping type (Axis + Button + ButtonBitmap) round-trips correctly
- Mapping order is preserved
- Each mapping retains its concrete type after deserialization

- [ ] **Step 2: Run tests in Docker, verify pass**
- [ ] **Step 3: Commit**

### Task 3: Edge cases

- [ ] **Step 1: Write tests**

Tests:
- Empty string fields (COMPort="", Protocol="") survive round-trip
- Null LuaScript survives round-trip
- Profile with empty Mappings list
- Multiple profiles with unique names
- Merge with no profiles (importProfiles=true but source has none)
- Merge with importGlobalOptions=false preserves original values
- LoadFromJSONString with malformed JSON throws SerializationException
- Default field values on fresh Configuration (DefaultProfile="", Profiles empty, WebSocketPort=40000, etc.)

- [ ] **Step 2: Run tests in Docker, verify pass**
- [ ] **Step 3: Commit**

---

## Summary

| Chunk | New Tests (approx) | Branch |
|-------|-------------------|--------|
| 1. Mapping Clamp | ~12 | `test/mapping-clamp` |
| 2. Mapping Copy | ~10 | `test/mapping-copy` |
| 3. Mapping Failsafe | ~15 | `test/mapping-failsafe` |
| 4. Protocol Reader Properties | ~27 (9 readers x 3 properties) | `test/protocol-reader-properties` |
| 5. VJoyNone | ~8 | `test/vjoy-none` |
| 6. Configuration Edge Cases | ~15 | `test/config-serialization-edge-cases` |
| **Total** | **~87** | |

Combined with the existing 69 tests, this brings the total to ~156 tests — covering essentially all pure logic that's reachable without application code changes.

## Execution Order

Chunks are independent and can be done in any order. The recommended order above goes from simplest (Clamp) to most involved (Configuration edge cases), making each chunk a quick win.

After all chunks are complete, the remaining untested logic (TriggerState, DoUpgrade, protocol parsing, private math methods) will require application code changes to make testable — that work aligns with the modernization plan's "separate logic from UI" phase.
