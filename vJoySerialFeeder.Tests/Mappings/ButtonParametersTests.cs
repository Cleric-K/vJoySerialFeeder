using NUnit.Framework;
using vJoySerialFeeder;

namespace vJoySerialFeeder.Tests.Mappings
{
	[TestFixture]
	public class ButtonParametersTests
	{
		// ── Single threshold (notch=false) ──────────────────────

		[Test]
		public void Transform_SingleThreshold_AboveThreshold_ReturnsTrue()
		{
			var p = new ButtonMapping.ButtonParameters {
				thresh1 = 1500
			};
			Assert.That(p.Transform(1600), Is.True);
		}

		[Test]
		public void Transform_SingleThreshold_BelowThreshold_ReturnsFalse()
		{
			var p = new ButtonMapping.ButtonParameters {
				thresh1 = 1500
			};
			Assert.That(p.Transform(1400), Is.False);
		}

		[Test]
		public void Transform_SingleThreshold_ExactlyAtThreshold_ReturnsTrue()
		{
			var p = new ButtonMapping.ButtonParameters {
				thresh1 = 1500
			};
			Assert.That(p.Transform(1500), Is.True);
		}

		[Test]
		public void Transform_SingleThreshold_OneBelow_ReturnsFalse()
		{
			var p = new ButtonMapping.ButtonParameters {
				thresh1 = 1500
			};
			Assert.That(p.Transform(1499), Is.False);
		}

		// ── Invert (without Trigger) ────────────────────────────

		[Test]
		public void Transform_SingleThreshold_Invert_AboveThreshold_ReturnsFalse()
		{
			var p = new ButtonMapping.ButtonParameters {
				thresh1 = 1500, invert = true
			};
			Assert.That(p.Transform(1600), Is.False);
		}

		[Test]
		public void Transform_SingleThreshold_Invert_BelowThreshold_ReturnsTrue()
		{
			var p = new ButtonMapping.ButtonParameters {
				thresh1 = 1500, invert = true
			};
			Assert.That(p.Transform(1400), Is.True);
		}

		[Test]
		public void Transform_SingleThreshold_Invert_AtThreshold_ReturnsFalse()
		{
			var p = new ButtonMapping.ButtonParameters {
				thresh1 = 1500, invert = true
			};
			Assert.That(p.Transform(1500), Is.False);
		}

		// ── Dual threshold / notch mode ─────────────────────────

		[Test]
		public void Transform_Notch_InsideRange_ReturnsTrue()
		{
			var p = new ButtonMapping.ButtonParameters {
				notch = true, thresh1 = 1400, thresh2 = 1600
			};
			Assert.That(p.Transform(1500), Is.True);
		}

		[Test]
		public void Transform_Notch_AtLowerBound_ReturnsTrue()
		{
			var p = new ButtonMapping.ButtonParameters {
				notch = true, thresh1 = 1400, thresh2 = 1600
			};
			Assert.That(p.Transform(1400), Is.True);
		}

		[Test]
		public void Transform_Notch_AtUpperBound_ReturnsTrue()
		{
			var p = new ButtonMapping.ButtonParameters {
				notch = true, thresh1 = 1400, thresh2 = 1600
			};
			Assert.That(p.Transform(1600), Is.True);
		}

		[Test]
		public void Transform_Notch_BelowRange_ReturnsFalse()
		{
			var p = new ButtonMapping.ButtonParameters {
				notch = true, thresh1 = 1400, thresh2 = 1600
			};
			Assert.That(p.Transform(1399), Is.False);
		}

		[Test]
		public void Transform_Notch_AboveRange_ReturnsFalse()
		{
			var p = new ButtonMapping.ButtonParameters {
				notch = true, thresh1 = 1400, thresh2 = 1600
			};
			Assert.That(p.Transform(1601), Is.False);
		}

		// ── Notch + invert ──────────────────────────────────────

		[Test]
		public void Transform_Notch_Invert_InsideRange_ReturnsFalse()
		{
			var p = new ButtonMapping.ButtonParameters {
				notch = true, thresh1 = 1400, thresh2 = 1600, invert = true
			};
			Assert.That(p.Transform(1500), Is.False);
		}

		[Test]
		public void Transform_Notch_Invert_OutsideRange_ReturnsTrue()
		{
			var p = new ButtonMapping.ButtonParameters {
				notch = true, thresh1 = 1400, thresh2 = 1600, invert = true
			};
			Assert.That(p.Transform(1300), Is.True);
			Assert.That(p.Transform(1700), Is.True);
		}

		// ── Edge cases ──────────────────────────────────────────

		[Test]
		public void Transform_ZeroThreshold_ZeroValue_ReturnsTrue()
		{
			var p = new ButtonMapping.ButtonParameters {
				thresh1 = 0
			};
			Assert.That(p.Transform(0), Is.True);
		}

		[Test]
		public void Transform_ZeroThreshold_NegativeValue_ReturnsFalse()
		{
			var p = new ButtonMapping.ButtonParameters {
				thresh1 = 0
			};
			Assert.That(p.Transform(-1), Is.False);
		}

		[Test]
		public void Transform_NegativeThreshold_Works()
		{
			var p = new ButtonMapping.ButtonParameters {
				thresh1 = -100
			};
			Assert.That(p.Transform(-100), Is.True);
			Assert.That(p.Transform(-101), Is.False);
			Assert.That(p.Transform(0), Is.True);
		}

		[Test]
		public void Transform_MaxIntValue_DoesNotOverflow()
		{
			var p = new ButtonMapping.ButtonParameters {
				thresh1 = int.MaxValue
			};
			Assert.That(p.Transform(int.MaxValue), Is.True);
			Assert.That(p.Transform(int.MaxValue - 1), Is.False);
		}

		[Test]
		public void Transform_MinIntValue_DoesNotOverflow()
		{
			var p = new ButtonMapping.ButtonParameters {
				thresh1 = int.MinValue
			};
			Assert.That(p.Transform(int.MinValue), Is.True);
			Assert.That(p.Transform(int.MinValue + 1), Is.True);
		}

		[Test]
		public void Transform_Notch_EqualThresholds_OnlyExactValueMatches()
		{
			var p = new ButtonMapping.ButtonParameters {
				notch = true, thresh1 = 1500, thresh2 = 1500
			};
			Assert.That(p.Transform(1500), Is.True);
			Assert.That(p.Transform(1499), Is.False);
			Assert.That(p.Transform(1501), Is.False);
		}

		[Test]
		public void Transform_Notch_InvertedThresholds_NeverTrue()
		{
			// thresh1 > thresh2 means the range is impossible
			var p = new ButtonMapping.ButtonParameters {
				notch = true, thresh1 = 1600, thresh2 = 1400
			};
			Assert.That(p.Transform(1500), Is.False);
			Assert.That(p.Transform(1600), Is.False);
			Assert.That(p.Transform(1400), Is.False);
		}

		// ── Trigger flag interaction with Transform ─────────────
		// When Trigger=true, the invert XOR path changes because
		// (!Trigger & invert) becomes false regardless of invert.

		[Test]
		public void Transform_TriggerFlag_InvertIgnored_AboveThreshold_ReturnsTrue()
		{
			var p = new ButtonMapping.ButtonParameters {
				thresh1 = 1500, Trigger = true, invert = true
			};
			// With Trigger=true: (!true & true) ^ state = false ^ state = state
			Assert.That(p.Transform(1600), Is.True);
		}

		[Test]
		public void Transform_TriggerFlag_InvertIgnored_BelowThreshold_ReturnsFalse()
		{
			var p = new ButtonMapping.ButtonParameters {
				thresh1 = 1500, Trigger = true, invert = true
			};
			Assert.That(p.Transform(1400), Is.False);
		}

		[Test]
		public void Transform_TriggerFlag_NoInvert_SameAsNormal()
		{
			var p = new ButtonMapping.ButtonParameters {
				thresh1 = 1500, Trigger = true, invert = false
			};
			// (!true & false) ^ state = false ^ state = state
			Assert.That(p.Transform(1600), Is.True);
			Assert.That(p.Transform(1400), Is.False);
		}

		// ── Default struct values ───────────────────────────────

		[Test]
		public void Transform_DefaultParameters_ZeroValue_ReturnsTrue()
		{
			// Default struct: all fields are 0/false
			// thresh1=0, notch=false, invert=false, Trigger=false
			// val >= 0 is true => returns true
			var p = new ButtonMapping.ButtonParameters();
			Assert.That(p.Transform(0), Is.True);
		}

		[Test]
		public void Transform_DefaultParameters_NegativeValue_ReturnsFalse()
		{
			var p = new ButtonMapping.ButtonParameters();
			Assert.That(p.Transform(-1), Is.False);
		}
	}
}
