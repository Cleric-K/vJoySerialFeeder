using System;
using NUnit.Framework;
using vJoySerialFeeder;

namespace vJoySerialFeeder.Tests.Mappings
{
	/// <summary>
	/// Tests that pin down implicit/surprising behaviors in AxisParameters.Transform
	/// discovered by tracing the implementation line by line.
	/// </summary>
	[TestFixture]
	public class AxisParametersBehaviorTests
	{
		// ── Deadband is ignored without Symmetric ───────────────
		// Line 63: if(Symmetric && Deadband > 0) — both must be true

		[Test]
		public void Transform_Deadband_IgnoredWithoutSymmetric()
		{
			var withDb = new AxisMapping.AxisParameters {
				Min = 1000, Max = 2000, Deadband = 50
			};
			var withoutDb = new AxisMapping.AxisParameters {
				Min = 1000, Max = 2000, Deadband = 0
			};
			// Without Symmetric, deadband has no effect
			Assert.That(withDb.Transform(1250), Is.EqualTo(withoutDb.Transform(1250)));
			Assert.That(withDb.Transform(1500), Is.EqualTo(withoutDb.Transform(1500)));
		}

		[Test]
		public void Transform_NegativeDeadband_Ignored()
		{
			// Deadband > 0 check means negative values are no-ops
			var p = new AxisMapping.AxisParameters {
				Min = 1000, Max = 2000, Center = 1500,
				Symmetric = true, Deadband = -10
			};
			// At center, without deadband active, result should be exactly 0.5
			Assert.That(p.Transform(1500), Is.EqualTo(0.5f).Within(0.001f));
			// Values near center should NOT snap to 0.5 (no deadband active)
			Assert.That(p.Transform(1510), Is.Not.EqualTo(0.5f));
		}

		// ── Deadband=100 makes nearly everything return 0.5 ────
		// d = 100/100.01 ≈ 0.999, so v < 0.999 returns 0.5
		// Only the very extremes (Min, Max) escape the deadband

		[Test]
		public void Transform_Deadband100_AlmostEverythingIsCenter()
		{
			var p = new AxisMapping.AxisParameters {
				Min = 1000, Max = 2000, Center = 1500,
				Symmetric = true, Deadband = 100
			};
			// Most values return 0.5
			Assert.That(p.Transform(1500), Is.EqualTo(0.5f).Within(0.001f));
			Assert.That(p.Transform(1250), Is.EqualTo(0.5f).Within(0.001f));
			Assert.That(p.Transform(1750), Is.EqualTo(0.5f).Within(0.001f));
			// Extremes still reach 0 and 1
			Assert.That(p.Transform(1000), Is.EqualTo(0f).Within(0.01f));
			Assert.That(p.Transform(2000), Is.EqualTo(1f).Within(0.01f));
		}

		// ── Symmetric with asymmetric center ────────────────────
		// Center doesn't have to be the midpoint of Min/Max.
		// Each side scales independently.

		[Test]
		public void Transform_Symmetric_AsymmetricCenter_ScalesIndependently()
		{
			var p = new AxisMapping.AxisParameters {
				Min = 1000, Max = 2000, Center = 1200, Symmetric = true
			};
			// Center always maps to 0.5
			Assert.That(p.Transform(1200), Is.EqualTo(0.5f).Within(0.001f));
			// Min still maps to 0, Max still maps to 1
			Assert.That(p.Transform(1000), Is.EqualTo(0f).Within(0.001f));
			Assert.That(p.Transform(2000), Is.EqualTo(1f).Within(0.001f));
			// Halfway on left side (200 range): 1100 is 50% of [1000,1200]
			Assert.That(p.Transform(1100), Is.EqualTo(0.25f).Within(0.001f));
			// Halfway on right side (800 range): 1600 is 50% of [1200,2000]
			Assert.That(p.Transform(1600), Is.EqualTo(0.75f).Within(0.001f));
		}

		[Test]
		public void Transform_Symmetric_CenterAtMin_ReturnsZero()
		{
			// Center <= Min is invalid in symmetric mode
			var p = new AxisMapping.AxisParameters {
				Min = 1000, Max = 2000, Center = 1000, Symmetric = true
			};
			Assert.That(p.Transform(1500), Is.EqualTo(0f));
		}

		[Test]
		public void Transform_Symmetric_CenterAtMax_ReturnsZero()
		{
			var p = new AxisMapping.AxisParameters {
				Min = 1000, Max = 2000, Center = 2000, Symmetric = true
			};
			Assert.That(p.Transform(1500), Is.EqualTo(0f));
		}

		// ── Expo edge values ────────────────────────────────────
		// factor = Abs(Expo)/100.01, deliberately avoids exactly 1.0
		// to prevent divide-by-zero in (1-v*factor)

		[Test]
		public void Transform_Expo100_StillReachesEndpoints()
		{
			var p = new AxisMapping.AxisParameters {
				Min = 1000, Max = 2000, Expo = 100
			};
			Assert.That(p.Transform(1000), Is.EqualTo(0f).Within(0.001f));
			Assert.That(p.Transform(2000), Is.EqualTo(1f).Within(0.001f));
		}

		[Test]
		public void Transform_ExpoNeg100_StillReachesEndpoints()
		{
			var p = new AxisMapping.AxisParameters {
				Min = 1000, Max = 2000, Expo = -100
			};
			Assert.That(p.Transform(1000), Is.EqualTo(0f).Within(0.001f));
			Assert.That(p.Transform(2000), Is.EqualTo(1f).Within(0.001f));
		}

		[Test]
		public void Transform_Expo_AppliedInNonSymmetricMode()
		{
			// Expo doesn't require Symmetric — it works in linear mode too
			var linear = new AxisMapping.AxisParameters {
				Min = 1000, Max = 2000
			};
			var expo = new AxisMapping.AxisParameters {
				Min = 1000, Max = 2000, Expo = 50
			};
			// Midpoint should differ
			Assert.That(expo.Transform(1500),
				Is.Not.EqualTo(linear.Transform(1500)).Within(0.001f));
		}

		// ── Expo combined with Symmetric ────────────────────────

		[Test]
		public void Transform_Expo_WithSymmetric_CenterStaysAtHalf()
		{
			var p = new AxisMapping.AxisParameters {
				Min = 1000, Max = 2000, Center = 1500,
				Symmetric = true, Expo = 50
			};
			// At center, v=0 before expo, so expo(0)=0, then 0/2+0.5=0.5
			Assert.That(p.Transform(1500), Is.EqualTo(0.5f).Within(0.001f));
		}

		[Test]
		public void Transform_Expo_WithSymmetric_ReducesSensitivityBothSides()
		{
			var linear = new AxisMapping.AxisParameters {
				Min = 1000, Max = 2000, Center = 1500, Symmetric = true
			};
			var expo = new AxisMapping.AxisParameters {
				Min = 1000, Max = 2000, Center = 1500,
				Symmetric = true, Expo = 50
			};
			// Right of center: expo pulls value toward center
			float linearRight = linear.Transform(1750);
			float expoRight = expo.Transform(1750);
			Assert.That(expoRight, Is.LessThan(linearRight));
			// Left of center: expo pulls value toward center
			float linearLeft = linear.Transform(1250);
			float expoLeft = expo.Transform(1250);
			Assert.That(expoLeft, Is.GreaterThan(linearLeft));
		}

		// ── Min == Max ──────────────────────────────────────────

		[Test]
		public void Transform_MinEqualsMax_ReturnsZero()
		{
			var p = new AxisMapping.AxisParameters {
				Min = 1500, Max = 1500
			};
			Assert.That(p.Transform(1500), Is.EqualTo(0f));
		}

		// ── Deadband + Expo interaction ─────────────────────────

		[Test]
		public void Transform_DeadbandAndExpo_ExpoAppliedAfterDeadband()
		{
			var dbOnly = new AxisMapping.AxisParameters {
				Min = 1000, Max = 2000, Center = 1500,
				Symmetric = true, Deadband = 10
			};
			var dbExpo = new AxisMapping.AxisParameters {
				Min = 1000, Max = 2000, Center = 1500,
				Symmetric = true, Deadband = 10, Expo = 50
			};
			// Inside deadband both return 0.5
			Assert.That(dbOnly.Transform(1500), Is.EqualTo(0.5f).Within(0.001f));
			Assert.That(dbExpo.Transform(1500), Is.EqualTo(0.5f).Within(0.001f));
			// Outside deadband, expo changes the curve
			Assert.That(dbExpo.Transform(1750),
				Is.Not.EqualTo(dbOnly.Transform(1750)).Within(0.001f));
			// Endpoints still the same
			Assert.That(dbExpo.Transform(2000), Is.EqualTo(1f).Within(0.001f));
		}

		// ── Invert combined with Symmetric ──────────────────────

		[Test]
		public void Transform_Invert_WithSymmetric_CenterStaysAtHalf()
		{
			// Invert: v = 1 - v. At center v=0.5, so 1-0.5=0.5 — unchanged!
			var p = new AxisMapping.AxisParameters {
				Min = 1000, Max = 2000, Center = 1500,
				Symmetric = true, Invert = true
			};
			Assert.That(p.Transform(1500), Is.EqualTo(0.5f).Within(0.001f));
		}

		[Test]
		public void Transform_Invert_WithSymmetric_SwapsMinMax()
		{
			var p = new AxisMapping.AxisParameters {
				Min = 1000, Max = 2000, Center = 1500,
				Symmetric = true, Invert = true
			};
			// Min now maps to 1 (was 0)
			Assert.That(p.Transform(1000), Is.EqualTo(1f).Within(0.001f));
			// Max now maps to 0 (was 1)
			Assert.That(p.Transform(2000), Is.EqualTo(0f).Within(0.001f));
		}
	}
}
