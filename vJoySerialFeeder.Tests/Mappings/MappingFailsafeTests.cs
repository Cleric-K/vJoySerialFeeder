using NUnit.Framework;
using vJoySerialFeeder;

namespace vJoySerialFeeder.Tests.Mappings
{
	[TestFixture]
	public class MappingFailsafeTests
	{
		// ── AxisMapping.Failsafe ────────────────────────────────
		// if(Parameters.Failsafe >= 0)
		//     Output = Parameters.Failsafe/100.0f;

		[Test]
		public void AxisMapping_Failsafe_DefaultNegativeOne_OutputUnchanged()
		{
			var m = new AxisMapping {
				Parameters = new AxisMapping.AxisParameters { Failsafe = -1 }
			};
			m.Output = 0.75f;
			m.Failsafe();
			Assert.That(m.Output, Is.EqualTo(0.75f));
		}

		[Test]
		public void AxisMapping_Failsafe_Zero_OutputBecomesZero()
		{
			var m = new AxisMapping {
				Parameters = new AxisMapping.AxisParameters { Failsafe = 0 }
			};
			m.Output = 0.75f;
			m.Failsafe();
			Assert.That(m.Output, Is.EqualTo(0f));
		}

		[Test]
		public void AxisMapping_Failsafe_Fifty_OutputBecomesHalf()
		{
			var m = new AxisMapping {
				Parameters = new AxisMapping.AxisParameters { Failsafe = 50 }
			};
			m.Failsafe();
			Assert.That(m.Output, Is.EqualTo(0.5f));
		}

		[Test]
		public void AxisMapping_Failsafe_Hundred_OutputBecomesOne()
		{
			var m = new AxisMapping {
				Parameters = new AxisMapping.AxisParameters { Failsafe = 100 }
			};
			m.Failsafe();
			Assert.That(m.Output, Is.EqualTo(1.0f));
		}

		[Test]
		public void AxisMapping_Failsafe_Over100_ClampedToOne()
		{
			// Failsafe=200 -> 2.0f, but Clamp caps at 1.0
			var m = new AxisMapping {
				Parameters = new AxisMapping.AxisParameters { Failsafe = 200 }
			};
			m.Failsafe();
			Assert.That(m.Output, Is.EqualTo(1.0f));
		}

		// ── ButtonMapping.Failsafe ──────────────────────────────
		// if(Parameters.Failsafe > 0)
		//     Output = Parameters.Failsafe == 1 ? 0 : 1;

		[Test]
		public void ButtonMapping_Failsafe_Zero_OutputUnchanged()
		{
			var m = new ButtonMapping {
				Parameters = new ButtonMapping.ButtonParameters { Failsafe = 0 }
			};
			m.Output = 1f;
			m.Failsafe();
			Assert.That(m.Output, Is.EqualTo(1f));
		}

		[Test]
		public void ButtonMapping_Failsafe_One_OutputDepressed()
		{
			var m = new ButtonMapping {
				Parameters = new ButtonMapping.ButtonParameters { Failsafe = 1 }
			};
			m.Output = 1f;
			m.Failsafe();
			Assert.That(m.Output, Is.EqualTo(0f));
		}

		[Test]
		public void ButtonMapping_Failsafe_Two_OutputPressed()
		{
			var m = new ButtonMapping {
				Parameters = new ButtonMapping.ButtonParameters { Failsafe = 2 }
			};
			m.Output = 0f;
			m.Failsafe();
			Assert.That(m.Output, Is.EqualTo(1f));
		}

		[Test]
		public void ButtonMapping_Failsafe_HighValue_OutputPressed()
		{
			// Any Failsafe > 1 means pressed
			var m = new ButtonMapping {
				Parameters = new ButtonMapping.ButtonParameters { Failsafe = 99 }
			};
			m.Output = 0f;
			m.Failsafe();
			Assert.That(m.Output, Is.EqualTo(1f));
		}

		// ── ButtonBitmapMapping.Failsafe ────────────────────────
		// For each bit: if Enabled && Failsafe != 0:
		//   Failsafe == 1: clear bit; else: set bit

		[Test]
		public void ButtonBitmapMapping_Failsafe_AllDisabled_NoChange()
		{
			var m = new ButtonBitmapMapping();
			// All Parameters default to Enabled=false
			// Must set Input (not Output) because Failsafe() recalculates via Input = Input
			m.Input = 0x00FF;
			m.Failsafe();
			Assert.That((int)m.Output, Is.EqualTo(0x00FF));
		}

		[Test]
		public void ButtonBitmapMapping_Failsafe_ClearBit()
		{
			var m = new ButtonBitmapMapping();
			m.Parameters[0] = new ButtonBitmapMapping.BitButtonParameters {
				Enabled = true, Failsafe = 1 // clear
			};
			// Set bit 0 in output
			m.Output = 0x0001f;
			m.Failsafe();
			// Bit 0 should be cleared
			Assert.That((int)m.Output & 1, Is.EqualTo(0));
		}

		[Test]
		public void ButtonBitmapMapping_Failsafe_SetBit()
		{
			var m = new ButtonBitmapMapping();
			m.Parameters[0] = new ButtonBitmapMapping.BitButtonParameters {
				Enabled = true, Failsafe = 2 // set
			};
			m.Output = 0f;
			m.Failsafe();
			// Bit 0 should be set
			Assert.That((int)m.Output & 1, Is.EqualTo(1));
		}

		[Test]
		public void ButtonBitmapMapping_Failsafe_MixedBits()
		{
			var m = new ButtonBitmapMapping();
			// Bit 0: enabled, clear
			m.Parameters[0] = new ButtonBitmapMapping.BitButtonParameters {
				Enabled = true, Failsafe = 1
			};
			// Bit 1: enabled, set
			m.Parameters[1] = new ButtonBitmapMapping.BitButtonParameters {
				Enabled = true, Failsafe = 2
			};
			// Bit 2: enabled, no failsafe (Failsafe=0)
			m.Parameters[2] = new ButtonBitmapMapping.BitButtonParameters {
				Enabled = true, Failsafe = 0
			};
			// Bit 3: disabled with failsafe (should be ignored)
			m.Parameters[3] = new ButtonBitmapMapping.BitButtonParameters {
				Enabled = false, Failsafe = 2
			};

			// Start with bits 0 and 2 set: 0b0101 = 5
			// Must set Input because Failsafe() recalculates via Input = Input
			m.Input = 5;
			m.Failsafe();

			int result = (int)m.Output;
			Assert.That(result & 0x01, Is.EqualTo(0), "Bit 0 should be cleared");
			Assert.That(result & 0x02, Is.EqualTo(2), "Bit 1 should be set");
			Assert.That(result & 0x04, Is.EqualTo(4), "Bit 2 unchanged (no failsafe)");
			Assert.That(result & 0x08, Is.EqualTo(0), "Bit 3 unchanged (disabled)");
		}

		[Test]
		public void ButtonBitmapMapping_Failsafe_HighBit()
		{
			var m = new ButtonBitmapMapping();
			m.Parameters[15] = new ButtonBitmapMapping.BitButtonParameters {
				Enabled = true, Failsafe = 2 // set
			};
			m.Output = 0f;
			m.Failsafe();
			Assert.That((int)m.Output & (1 << 15), Is.EqualTo(1 << 15));
		}
	}
}
