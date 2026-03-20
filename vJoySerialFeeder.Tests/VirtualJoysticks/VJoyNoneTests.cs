using NUnit.Framework;
using vJoySerialFeeder;

namespace vJoySerialFeeder.Tests.VirtualJoysticks
{
	[TestFixture]
	public class VJoyNoneTests
	{
		private VJoyNone vjoy;

		[SetUp]
		public void SetUp()
		{
			vjoy = new VJoyNone();
		}

		[Test]
		public void Acquire_DoesNotThrow()
		{
			Assert.DoesNotThrow(() => vjoy.Acquire("any"));
		}

		[Test]
		public void Release_DoesNotThrow()
		{
			Assert.DoesNotThrow(() => vjoy.Release());
		}

		[Test]
		public void SetAxis_DoesNotThrow()
		{
			Assert.DoesNotThrow(() => vjoy.SetAxis(0, 0.5));
		}

		[Test]
		public void SetButton_DoesNotThrow()
		{
			Assert.DoesNotThrow(() => vjoy.SetButton(0, true));
		}

		[Test]
		public void SetDiscPov_DoesNotThrow()
		{
			Assert.DoesNotThrow(() => vjoy.SetDiscPov(0, 1));
		}

		[Test]
		public void SetContPov_DoesNotThrow()
		{
			Assert.DoesNotThrow(() => vjoy.SetContPov(0, 0.5));
		}

		[Test]
		public void SetState_DoesNotThrow()
		{
			Assert.DoesNotThrow(() => vjoy.SetState());
		}

		[Test]
		public void GetJoysticks_ReturnsNull()
		{
			Assert.That(vjoy.GetJoysticks(), Is.Null);
		}
	}
}
