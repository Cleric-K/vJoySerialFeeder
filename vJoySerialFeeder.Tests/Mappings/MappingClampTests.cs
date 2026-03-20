using NUnit.Framework;
using vJoySerialFeeder;

namespace vJoySerialFeeder.Tests.Mappings
{
	[TestFixture]
	public class MappingClampTests
	{
		// ── AxisMapping.Clamp: clamps to [0, 1] ────────────────

		[Test]
		public void AxisMapping_Output_ClampsAboveOne()
		{
			var m = new AxisMapping();
			m.Output = 1.5f;
			Assert.That(m.Output, Is.EqualTo(1.0f));
		}

		[Test]
		public void AxisMapping_Output_ClampsBelowZero()
		{
			var m = new AxisMapping();
			m.Output = -0.5f;
			Assert.That(m.Output, Is.EqualTo(0f));
		}

		[Test]
		public void AxisMapping_Output_PreservesValueInRange()
		{
			var m = new AxisMapping();
			m.Output = 0.5f;
			Assert.That(m.Output, Is.EqualTo(0.5f));
		}

		[Test]
		public void AxisMapping_Output_ExactlyZero()
		{
			var m = new AxisMapping();
			m.Output = 0f;
			Assert.That(m.Output, Is.EqualTo(0f));
		}

		[Test]
		public void AxisMapping_Output_ExactlyOne()
		{
			var m = new AxisMapping();
			m.Output = 1.0f;
			Assert.That(m.Output, Is.EqualTo(1.0f));
		}

		[Test]
		public void AxisMapping_Output_LargePositive()
		{
			var m = new AxisMapping();
			m.Output = 9999f;
			Assert.That(m.Output, Is.EqualTo(1.0f));
		}

		[Test]
		public void AxisMapping_Output_LargeNegative()
		{
			var m = new AxisMapping();
			m.Output = -9999f;
			Assert.That(m.Output, Is.EqualTo(0f));
		}

		// ── ButtonMapping.Clamp: >0 becomes 1, else 0 ──────────

		[Test]
		public void ButtonMapping_Output_PositiveBecomesOne()
		{
			var m = new ButtonMapping();
			m.Output = 5.0f;
			Assert.That(m.Output, Is.EqualTo(1f));
		}

		[Test]
		public void ButtonMapping_Output_FractionalPositiveBecomesOne()
		{
			var m = new ButtonMapping();
			m.Output = 0.001f;
			Assert.That(m.Output, Is.EqualTo(1f));
		}

		[Test]
		public void ButtonMapping_Output_ZeroRemainsZero()
		{
			var m = new ButtonMapping();
			m.Output = 0f;
			Assert.That(m.Output, Is.EqualTo(0f));
		}

		[Test]
		public void ButtonMapping_Output_NegativeBecomesZero()
		{
			var m = new ButtonMapping();
			m.Output = -1.0f;
			Assert.That(m.Output, Is.EqualTo(0f));
		}

		// ── ButtonBitmapMapping.Clamp: int cast, [0, 0xFFFF] ───

		[Test]
		public void ButtonBitmapMapping_Output_ClampsAbove65535()
		{
			var m = new ButtonBitmapMapping();
			m.Output = 70000f;
			Assert.That(m.Output, Is.EqualTo(65535f));
		}

		[Test]
		public void ButtonBitmapMapping_Output_ClampsBelowZero()
		{
			var m = new ButtonBitmapMapping();
			m.Output = -100f;
			Assert.That(m.Output, Is.EqualTo(0f));
		}

		[Test]
		public void ButtonBitmapMapping_Output_PreservesValidValue()
		{
			var m = new ButtonBitmapMapping();
			m.Output = 255f;
			Assert.That(m.Output, Is.EqualTo(255f));
		}

		[Test]
		public void ButtonBitmapMapping_Output_CastsToInt()
		{
			// Clamp casts to int first: (int)100.7f = 100
			var m = new ButtonBitmapMapping();
			m.Output = 100.7f;
			Assert.That(m.Output, Is.EqualTo(100f));
		}

		[Test]
		public void ButtonBitmapMapping_Output_ZeroIsValid()
		{
			var m = new ButtonBitmapMapping();
			m.Output = 0f;
			Assert.That(m.Output, Is.EqualTo(0f));
		}

		[Test]
		public void ButtonBitmapMapping_Output_MaxValid()
		{
			var m = new ButtonBitmapMapping();
			m.Output = 65535f;
			Assert.That(m.Output, Is.EqualTo(65535f));
		}
	}
}
