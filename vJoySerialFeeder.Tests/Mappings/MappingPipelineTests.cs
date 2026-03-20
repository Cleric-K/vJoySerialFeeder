using NUnit.Framework;
using vJoySerialFeeder;

namespace vJoySerialFeeder.Tests.Mappings
{
	/// <summary>
	/// Tests the full Input→Transform→Clamp→Output pipeline through the
	/// actual Mapping classes, not just the parameter structs.
	/// Also tests Copy() transient state behavior and bitmap Invert.
	/// </summary>
	[TestFixture]
	public class MappingPipelineTests
	{
		// ── AxisMapping: Input drives Output ────────────────────

		[Test]
		public void AxisMapping_InputSetter_DrivesOutput()
		{
			var m = new AxisMapping {
				Parameters = new AxisMapping.AxisParameters {
					Min = 1000, Max = 2000
				}
			};
			m.Input = 1500;
			Assert.That(m.Output, Is.EqualTo(0.5f).Within(0.001f));
			Assert.That(m.Input, Is.EqualTo(1500));
		}

		[Test]
		public void AxisMapping_InputBeyondMax_OutputClampedToOne()
		{
			var m = new AxisMapping {
				Parameters = new AxisMapping.AxisParameters {
					Min = 1000, Max = 2000
				}
			};
			m.Input = 3000;
			Assert.That(m.Output, Is.EqualTo(1f).Within(0.001f));
		}

		[Test]
		public void AxisMapping_InvalidParams_OutputIsZero()
		{
			var m = new AxisMapping {
				Parameters = new AxisMapping.AxisParameters {
					Min = 2000, Max = 1000 // invalid: Max < Min
				}
			};
			m.Input = 1500;
			Assert.That(m.Output, Is.EqualTo(0f));
		}

		// ── ButtonMapping: Input drives Output ──────────────────

		[Test]
		public void ButtonMapping_InputAboveThreshold_OutputIsOne()
		{
			var m = new ButtonMapping {
				Parameters = new ButtonMapping.ButtonParameters {
					thresh1 = 1500
				}
			};
			m.Input = 1600;
			Assert.That(m.Output, Is.EqualTo(1f));
		}

		[Test]
		public void ButtonMapping_InputBelowThreshold_OutputIsZero()
		{
			var m = new ButtonMapping {
				Parameters = new ButtonMapping.ButtonParameters {
					thresh1 = 1500
				}
			};
			m.Input = 1400;
			Assert.That(m.Output, Is.EqualTo(0f));
		}

		// ── ButtonBitmapMapping: Invert via Transform ───────────
		// Enabled bit with Invert=true flips the bit in Transform
		// (without needing Trigger — Trigger is separate)

		[Test]
		public void ButtonBitmapMapping_Invert_FlipsBit()
		{
			var m = new ButtonBitmapMapping();
			m.Parameters[0] = new ButtonBitmapMapping.BitButtonParameters {
				Enabled = true, Invert = true
			};
			// Input bit 0 = 1, Invert flips to 0
			m.Input = 0x0001;
			Assert.That((int)m.Output & 1, Is.EqualTo(0));
		}

		[Test]
		public void ButtonBitmapMapping_Invert_SetsUnsetBit()
		{
			var m = new ButtonBitmapMapping();
			m.Parameters[0] = new ButtonBitmapMapping.BitButtonParameters {
				Enabled = true, Invert = true
			};
			// Input bit 0 = 0, Invert flips to 1
			m.Input = 0x0000;
			Assert.That((int)m.Output & 1, Is.EqualTo(1));
		}

		[Test]
		public void ButtonBitmapMapping_AllBitsInverted()
		{
			var m = new ButtonBitmapMapping();
			for (int i = 0; i < 16; i++) {
				m.Parameters[i] = new ButtonBitmapMapping.BitButtonParameters {
					Enabled = true, Invert = true
				};
			}
			m.Input = 0xFFFF;
			Assert.That((int)m.Output, Is.EqualTo(0));

			m.Input = 0x0000;
			Assert.That((int)m.Output, Is.EqualTo(0xFFFF));
		}

		[Test]
		public void ButtonBitmapMapping_MixedInvert_SelectiveBitFlip()
		{
			var m = new ButtonBitmapMapping();
			// Bit 0: enabled, not inverted
			m.Parameters[0] = new ButtonBitmapMapping.BitButtonParameters {
				Enabled = true, Invert = false
			};
			// Bit 1: enabled, inverted
			m.Parameters[1] = new ButtonBitmapMapping.BitButtonParameters {
				Enabled = true, Invert = true
			};
			// Bit 2: disabled (passes through unchanged)

			m.Input = 0x0007; // bits 0,1,2 all set
			int result = (int)m.Output;
			Assert.That(result & 0x01, Is.EqualTo(1), "Bit 0: enabled, not inverted, stays 1");
			Assert.That(result & 0x02, Is.EqualTo(0), "Bit 1: enabled, inverted, flips to 0");
			Assert.That(result & 0x04, Is.EqualTo(4), "Bit 2: disabled, passes through as 1");
		}

		[Test]
		public void ButtonBitmapMapping_DisabledBits_PassThrough()
		{
			var m = new ButtonBitmapMapping();
			// All parameters default to Enabled=false
			m.Input = 0xAAAA;
			Assert.That((int)m.Output, Is.EqualTo(0xAAAA));
		}

		// ── Copy() does NOT copy transient state ────────────────

		[Test]
		public void AxisMapping_Copy_DoesNotCopyInputOutput()
		{
			var original = new AxisMapping {
				Parameters = new AxisMapping.AxisParameters {
					Min = 1000, Max = 2000
				}
			};
			original.Input = 1750;
			// Verify original has state
			Assert.That(original.Input, Is.EqualTo(1750));
			Assert.That(original.Output, Is.EqualTo(0.75f).Within(0.001f));

			var copy = (AxisMapping)original.Copy();
			// Copy should have default Input/Output (0/0)
			Assert.That(copy.Input, Is.EqualTo(0));
			Assert.That(copy.Output, Is.EqualTo(0f));
		}

		[Test]
		public void ButtonMapping_Copy_DoesNotCopyInputOutput()
		{
			var original = new ButtonMapping {
				Parameters = new ButtonMapping.ButtonParameters { thresh1 = 1500 }
			};
			original.Input = 1600;
			Assert.That(original.Output, Is.EqualTo(1f));

			var copy = (ButtonMapping)original.Copy();
			Assert.That(copy.Input, Is.EqualTo(0));
			Assert.That(copy.Output, Is.EqualTo(0f));
		}

		[Test]
		public void Mapping_Removed_DefaultsFalse()
		{
			Assert.That(new AxisMapping().Removed, Is.False);
			Assert.That(new ButtonMapping().Removed, Is.False);
			Assert.That(new ButtonBitmapMapping().Removed, Is.False);
		}

		[Test]
		public void Copy_DoesNotCopyRemoved()
		{
			// We can't set Removed=true without MainForm, but we can
			// verify a fresh copy also has Removed=false
			var copy = new AxisMapping().Copy();
			Assert.That(copy.Removed, Is.False);
		}
	}
}
