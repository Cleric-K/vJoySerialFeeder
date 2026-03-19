using NUnit.Framework;
using vJoySerialFeeder;

namespace vJoySerialFeeder.Tests.Mappings
{
	[TestFixture]
	public class AxisParametersTests
	{
		[Test]
		public void Transform_LinearMapping_MapsMinToZero()
		{
			var p = new AxisMapping.AxisParameters {
				Min = 1000, Max = 2000
			};
			Assert.That(p.Transform(1000), Is.EqualTo(0f).Within(0.001f));
		}

		[Test]
		public void Transform_LinearMapping_MapsMaxToOne()
		{
			var p = new AxisMapping.AxisParameters {
				Min = 1000, Max = 2000
			};
			Assert.That(p.Transform(2000), Is.EqualTo(1f).Within(0.001f));
		}

		[Test]
		public void Transform_LinearMapping_MapsMidToHalf()
		{
			var p = new AxisMapping.AxisParameters {
				Min = 1000, Max = 2000
			};
			Assert.That(p.Transform(1500), Is.EqualTo(0.5f).Within(0.001f));
		}

		[Test]
		public void Transform_ClampsValueBelowMin()
		{
			var p = new AxisMapping.AxisParameters {
				Min = 1000, Max = 2000
			};
			Assert.That(p.Transform(500), Is.EqualTo(0f).Within(0.001f));
		}

		[Test]
		public void Transform_ClampsValueAboveMax()
		{
			var p = new AxisMapping.AxisParameters {
				Min = 1000, Max = 2000
			};
			Assert.That(p.Transform(2500), Is.EqualTo(1f).Within(0.001f));
		}

		[Test]
		public void Transform_Invert_ReversesOutput()
		{
			var p = new AxisMapping.AxisParameters {
				Min = 1000, Max = 2000, Invert = true
			};
			Assert.That(p.Transform(1000), Is.EqualTo(1f).Within(0.001f));
			Assert.That(p.Transform(2000), Is.EqualTo(0f).Within(0.001f));
			Assert.That(p.Transform(1500), Is.EqualTo(0.5f).Within(0.001f));
		}

		[Test]
		public void Transform_Symmetric_CenterMapsToHalf()
		{
			var p = new AxisMapping.AxisParameters {
				Min = 1000, Max = 2000, Center = 1500, Symmetric = true
			};
			Assert.That(p.Transform(1500), Is.EqualTo(0.5f).Within(0.001f));
		}

		[Test]
		public void Transform_Symmetric_MinMapsToZero()
		{
			var p = new AxisMapping.AxisParameters {
				Min = 1000, Max = 2000, Center = 1500, Symmetric = true
			};
			Assert.That(p.Transform(1000), Is.EqualTo(0f).Within(0.001f));
		}

		[Test]
		public void Transform_Symmetric_MaxMapsToOne()
		{
			var p = new AxisMapping.AxisParameters {
				Min = 1000, Max = 2000, Center = 1500, Symmetric = true
			};
			Assert.That(p.Transform(2000), Is.EqualTo(1f).Within(0.001f));
		}

		[Test]
		public void Transform_Symmetric_Deadband_CenterRegionReturnsHalf()
		{
			var p = new AxisMapping.AxisParameters {
				Min = 1000, Max = 2000, Center = 1500,
				Symmetric = true, Deadband = 10
			};
			// Values near center should return 0.5
			Assert.That(p.Transform(1500), Is.EqualTo(0.5f).Within(0.001f));
			Assert.That(p.Transform(1510), Is.EqualTo(0.5f).Within(0.001f));
			Assert.That(p.Transform(1490), Is.EqualTo(0.5f).Within(0.001f));
		}

		[Test]
		public void Transform_Symmetric_Deadband_OutsideDeadbandStillReachesExtremes()
		{
			var p = new AxisMapping.AxisParameters {
				Min = 1000, Max = 2000, Center = 1500,
				Symmetric = true, Deadband = 10
			};
			Assert.That(p.Transform(1000), Is.EqualTo(0f).Within(0.001f));
			Assert.That(p.Transform(2000), Is.EqualTo(1f).Within(0.001f));
		}

		[Test]
		public void Transform_PositiveExpo_ReducesSensitivityNearCenter()
		{
			var linear = new AxisMapping.AxisParameters {
				Min = 1000, Max = 2000
			};
			var expo = new AxisMapping.AxisParameters {
				Min = 1000, Max = 2000, Expo = 50
			};

			// At midpoint, expo should give a value closer to 0 (less sensitive near center)
			float linearVal = linear.Transform(1250);
			float expoVal = expo.Transform(1250);
			Assert.That(expoVal, Is.LessThan(linearVal));

			// At endpoints, both should reach the same values
			Assert.That(expo.Transform(1000), Is.EqualTo(0f).Within(0.001f));
			Assert.That(expo.Transform(2000), Is.EqualTo(1f).Within(0.001f));
		}

		[Test]
		public void Transform_NegativeExpo_IncreasesSensitivityNearCenter()
		{
			var linear = new AxisMapping.AxisParameters {
				Min = 1000, Max = 2000
			};
			var expo = new AxisMapping.AxisParameters {
				Min = 1000, Max = 2000, Expo = -50
			};

			float linearVal = linear.Transform(1250);
			float expoVal = expo.Transform(1250);
			Assert.That(expoVal, Is.GreaterThan(linearVal));
		}

		[Test]
		public void Transform_InvalidParameters_ReturnsZero()
		{
			// Max <= Min
			var p = new AxisMapping.AxisParameters {
				Min = 2000, Max = 1000
			};
			Assert.That(p.Transform(1500), Is.EqualTo(0f));

			// Symmetric with Center <= Min
			var p2 = new AxisMapping.AxisParameters {
				Min = 1000, Max = 2000, Center = 1000, Symmetric = true
			};
			Assert.That(p2.Transform(1500), Is.EqualTo(0f));

			// Symmetric with Center >= Max
			var p3 = new AxisMapping.AxisParameters {
				Min = 1000, Max = 2000, Center = 2000, Symmetric = true
			};
			Assert.That(p3.Transform(1500), Is.EqualTo(0f));
		}
	}
}
