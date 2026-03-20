using NUnit.Framework;
using vJoySerialFeeder;

namespace vJoySerialFeeder.Tests.Mappings
{
	[TestFixture]
	public class MappingCopyTests
	{
		// ── AxisMapping.Copy ────────────────────────────────────

		[Test]
		public void AxisMapping_Copy_ReturnsNewInstance()
		{
			var original = new AxisMapping();
			var copy = original.Copy();
			Assert.That(copy, Is.Not.SameAs(original));
			Assert.That(copy, Is.InstanceOf<AxisMapping>());
		}

		[Test]
		public void AxisMapping_Copy_CopiesAllFields()
		{
			var original = new AxisMapping {
				Channel = 3,
				Axis = 2,
				Parameters = new AxisMapping.AxisParameters {
					Min = 1000, Max = 2000, Center = 1500,
					Symmetric = true, Expo = 30, Deadband = 5,
					Failsafe = 50, Invert = true
				}
			};

			var copy = (AxisMapping)original.Copy();

			Assert.That(copy.Channel, Is.EqualTo(3));
			Assert.That(copy.Axis, Is.EqualTo(2));
			Assert.That(copy.Parameters.Min, Is.EqualTo(1000));
			Assert.That(copy.Parameters.Max, Is.EqualTo(2000));
			Assert.That(copy.Parameters.Center, Is.EqualTo(1500));
			Assert.That(copy.Parameters.Symmetric, Is.True);
			Assert.That(copy.Parameters.Expo, Is.EqualTo(30));
			Assert.That(copy.Parameters.Deadband, Is.EqualTo(5));
			Assert.That(copy.Parameters.Failsafe, Is.EqualTo(50));
			Assert.That(copy.Parameters.Invert, Is.True);
		}

		[Test]
		public void AxisMapping_Copy_IsIndependent()
		{
			var original = new AxisMapping {
				Channel = 1,
				Parameters = new AxisMapping.AxisParameters { Min = 1000 }
			};

			var copy = (AxisMapping)original.Copy();
			// AxisParameters is a struct, so modifying copy's Parameters
			// should not affect original
			copy.Channel = 99;
			var p = copy.Parameters;
			p.Min = 500;
			copy.Parameters = p;

			Assert.That(original.Channel, Is.EqualTo(1));
			Assert.That(original.Parameters.Min, Is.EqualTo(1000));
		}

		// ── ButtonMapping.Copy ──────────────────────────────────

		[Test]
		public void ButtonMapping_Copy_ReturnsNewInstance()
		{
			var original = new ButtonMapping();
			var copy = original.Copy();
			Assert.That(copy, Is.Not.SameAs(original));
			Assert.That(copy, Is.InstanceOf<ButtonMapping>());
		}

		[Test]
		public void ButtonMapping_Copy_CopiesAllFields()
		{
			var original = new ButtonMapping {
				Channel = 5,
				Button = 7,
				Parameters = new ButtonMapping.ButtonParameters {
					thresh1 = 1400, thresh2 = 1600,
					notch = true, invert = true,
					Trigger = true, Failsafe = 2,
					TriggerDuration = 500,
					TriggerEdge = TriggerState.Edge.Both
				}
			};

			var copy = (ButtonMapping)original.Copy();

			Assert.That(copy.Channel, Is.EqualTo(5));
			Assert.That(copy.Button, Is.EqualTo(7));
			Assert.That(copy.Parameters.thresh1, Is.EqualTo(1400));
			Assert.That(copy.Parameters.thresh2, Is.EqualTo(1600));
			Assert.That(copy.Parameters.notch, Is.True);
			Assert.That(copy.Parameters.invert, Is.True);
			Assert.That(copy.Parameters.Trigger, Is.True);
			Assert.That(copy.Parameters.Failsafe, Is.EqualTo(2));
			Assert.That(copy.Parameters.TriggerDuration, Is.EqualTo(500));
			Assert.That(copy.Parameters.TriggerEdge, Is.EqualTo(TriggerState.Edge.Both));
		}

		// ── ButtonBitmapMapping.Copy ────────────────────────────

		[Test]
		public void ButtonBitmapMapping_Copy_ReturnsNewInstance()
		{
			var original = new ButtonBitmapMapping();
			var copy = original.Copy();
			Assert.That(copy, Is.Not.SameAs(original));
			Assert.That(copy, Is.InstanceOf<ButtonBitmapMapping>());
		}

		[Test]
		public void ButtonBitmapMapping_Copy_CopiesChannel()
		{
			var original = new ButtonBitmapMapping { Channel = 10 };
			var copy = (ButtonBitmapMapping)original.Copy();
			Assert.That(copy.Channel, Is.EqualTo(10));
		}

		[Test]
		public void ButtonBitmapMapping_Copy_CopiesParametersArray()
		{
			var original = new ButtonBitmapMapping();
			original.Parameters[0] = new ButtonBitmapMapping.BitButtonParameters {
				Button = 5, Enabled = true, Invert = true,
				Trigger = true, Failsafe = 2, TriggerDuration = 300,
				TriggerEdge = TriggerState.Edge.Falling
			};
			original.Parameters[15] = new ButtonBitmapMapping.BitButtonParameters {
				Button = 12, Enabled = true
			};

			var copy = (ButtonBitmapMapping)original.Copy();

			Assert.That(copy.Parameters[0].Button, Is.EqualTo(5));
			Assert.That(copy.Parameters[0].Enabled, Is.True);
			Assert.That(copy.Parameters[0].Invert, Is.True);
			Assert.That(copy.Parameters[0].TriggerEdge, Is.EqualTo(TriggerState.Edge.Falling));
			Assert.That(copy.Parameters[15].Button, Is.EqualTo(12));
			Assert.That(copy.Parameters[15].Enabled, Is.True);
		}

		[Test]
		public void ButtonBitmapMapping_Copy_ArrayIsIndependent()
		{
			var original = new ButtonBitmapMapping();
			original.Parameters[0] = new ButtonBitmapMapping.BitButtonParameters {
				Button = 5, Enabled = true
			};

			var copy = (ButtonBitmapMapping)original.Copy();
			copy.Parameters[0] = new ButtonBitmapMapping.BitButtonParameters {
				Button = 99, Enabled = false
			};

			Assert.That(original.Parameters[0].Button, Is.EqualTo(5));
			Assert.That(original.Parameters[0].Enabled, Is.True);
		}
	}
}
