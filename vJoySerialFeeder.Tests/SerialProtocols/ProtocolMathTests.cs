using System;
using NUnit.Framework;
using vJoySerialFeeder;

namespace vJoySerialFeeder.Tests.SerialProtocols
{
	[TestFixture]
	public class CrsfMapTests
	{
		[Test]
		public void Map_ValueBelowInMin_ReturnsOutMin()
		{
			Assert.That(CrsfReader.map(100, 191, 1792, 1000, 2000), Is.EqualTo(1000));
		}

		[Test]
		public void Map_ValueAboveInMax_ReturnsOutMax()
		{
			Assert.That(CrsfReader.map(2000, 191, 1792, 1000, 2000), Is.EqualTo(2000));
		}

		[Test]
		public void Map_ExactInMin_ReturnsOutMin()
		{
			Assert.That(CrsfReader.map(191, 191, 1792, 1000, 2000), Is.EqualTo(1000));
		}

		[Test]
		public void Map_ExactInMax_ReturnsOutMax()
		{
			Assert.That(CrsfReader.map(1792, 191, 1792, 1000, 2000), Is.EqualTo(2000));
		}

		[Test]
		public void Map_CrsfMidpoint_ReturnsApproximateMidpoint()
		{
			// Midpoint of 191..1792 = (191+1792)/2 = 991 (integer)
			// Expected: (991-191)*1000/1601 + 1000 = 800*1000/1601 + 1000 = 499 + 1000 = 1499
			uint mid = (191 + 1792) / 2; // 991
			uint result = CrsfReader.map(mid, 191, 1792, 1000, 2000);
			Assert.That(result, Is.EqualTo(1499));
		}

		[Test]
		public void Map_LinearInterpolation_QuarterPoint()
		{
			// Quarter point: 191 + (1792-191)/4 = 191 + 400 = 591
			// Expected: (591-191)*1000/1601 + 1000 = 400*1000/1601 + 1000 = 249 + 1000 = 1249
			uint result = CrsfReader.map(591, 191, 1792, 1000, 2000);
			Assert.That(result, Is.EqualTo(1249));
		}

		[Test]
		public void Map_ValueZero_ClampedToOutMin()
		{
			Assert.That(CrsfReader.map(0, 191, 1792, 1000, 2000), Is.EqualTo(1000));
		}

		[Test]
		public void Map_IdentityRange_ReturnsSameValue()
		{
			// When in range equals out range, returns the value itself
			Assert.That(CrsfReader.map(500, 0, 1000, 0, 1000), Is.EqualTo(500));
		}
	}

	[TestFixture]
	public class DjiRemapTests
	{
		[Test]
		public void Remap_RangeMin_Returns1000()
		{
			var reader = new DjiControllerReader();
			Assert.That(reader.remap(364), Is.EqualTo(1000));
		}

		[Test]
		public void Remap_RangeMax_Returns2000()
		{
			var reader = new DjiControllerReader();
			Assert.That(reader.remap(1684), Is.EqualTo(2000));
		}

		[Test]
		public void Remap_Midpoint1024_Returns1500()
		{
			var reader = new DjiControllerReader();
			// (1024-364) / (1684-364*1.0) * 1000 + 1000
			// = 660 / 1320.0 * 1000 + 1000 = 500 + 1000 = 1500
			Assert.That(reader.remap(1024), Is.EqualTo(1500));
		}

		[Test]
		public void Remap_BelowRangeMin_ExtrapolatesBelowThousand()
		{
			var reader = new DjiControllerReader();
			// (0-364) / 1320.0 * 1000 + 1000 = -275.757... + 1000 = 724.242... → 724
			int result = reader.remap(0);
			Assert.That(result, Is.EqualTo(724));
		}

		[Test]
		public void Remap_AboveRangeMax_ExtrapolatesAboveTwoThousand()
		{
			var reader = new DjiControllerReader();
			// (2000-364) / 1320.0 * 1000 + 1000 = 1239.39... + 1000 = 2239.39... → 2239
			int result = reader.remap(2000);
			Assert.That(result, Is.EqualTo(2239));
		}

		[Test]
		public void Remap_JustAboveMin_ReturnsSlightlyAbove1000()
		{
			var reader = new DjiControllerReader();
			// (365-364) / 1320.0 * 1000 + 1000 = 0.757... + 1000 = 1000.757... → 1001
			int result = reader.remap(365);
			Assert.That(result, Is.EqualTo(1001));
		}

		[Test]
		public void Remap_JustBelowMax_ReturnsSlightlyBelow2000()
		{
			var reader = new DjiControllerReader();
			// (1683-364) / 1320.0 * 1000 + 1000 = 999.242... + 1000 = 1999.242... → 1999
			int result = reader.remap(1683);
			Assert.That(result, Is.EqualTo(1999));
		}
	}
}
