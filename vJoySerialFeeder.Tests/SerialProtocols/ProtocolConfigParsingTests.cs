using NUnit.Framework;
using vJoySerialFeeder;

namespace vJoySerialFeeder.Tests.SerialProtocols
{
	[TestFixture]
	public class IbusConfigTests
	{
		[Test]
		public void ParseConfig_EmptyString_BuildConfigReturnsEmpty()
		{
			var reader = new IbusReader();
			reader.parseConfig("");
			Assert.That(reader.buildConfig(), Is.EqualTo(""));
		}

		[Test]
		public void ParseConfig_Null_BuildConfigReturnsEmpty()
		{
			var reader = new IbusReader();
			reader.parseConfig(null);
			Assert.That(reader.buildConfig(), Is.EqualTo(""));
		}

		[Test]
		public void ParseConfig_Ia6_BuildConfigContainsIa6()
		{
			var reader = new IbusReader();
			reader.parseConfig("ia6");
			Assert.That(reader.buildConfig(), Is.EqualTo("ia6"));
		}

		[Test]
		public void ParseConfig_16bit_BuildConfigContains16bit()
		{
			var reader = new IbusReader();
			reader.parseConfig("16bit");
			Assert.That(reader.buildConfig(), Is.EqualTo("16bit"));
		}

		[Test]
		public void ParseConfig_BothFlags_BuildConfigContainsBoth()
		{
			var reader = new IbusReader();
			reader.parseConfig("ia6,16bit");
			Assert.That(reader.buildConfig(), Is.EqualTo("ia6,16bit"));
		}

		[Test]
		public void ParseConfig_ReversedOrder_BuildConfigNormalizesOrder()
		{
			var reader = new IbusReader();
			reader.parseConfig("16bit,ia6");
			// buildConfig always outputs ia6 first, then 16bit
			Assert.That(reader.buildConfig(), Is.EqualTo("ia6,16bit"));
		}

		[Test]
		public void ParseConfig_UnknownTokensIgnored()
		{
			var reader = new IbusReader();
			reader.parseConfig("ia6,unknown,16bit");
			Assert.That(reader.buildConfig(), Is.EqualTo("ia6,16bit"));
		}

		[Test]
		public void ParseConfig_OnlyUnknownTokens_BuildConfigReturnsEmpty()
		{
			var reader = new IbusReader();
			reader.parseConfig("foo,bar");
			Assert.That(reader.buildConfig(), Is.EqualTo(""));
		}

		[Test]
		public void ParseConfig_RoundTrip()
		{
			var reader = new IbusReader();
			reader.parseConfig("ia6,16bit");
			var config = reader.buildConfig();

			var reader2 = new IbusReader();
			reader2.parseConfig(config);
			Assert.That(reader2.buildConfig(), Is.EqualTo(config));
		}
	}

	[TestFixture]
	public class SbusConfigTests
	{
		[Test]
		public void ParseConfig_EmptyString_BuildConfigReturnsEmpty()
		{
			var reader = new SbusReader();
			reader.parseConfig("");
			Assert.That(reader.buildConfig(), Is.EqualTo(""));
		}

		[Test]
		public void ParseConfig_Null_BuildConfigReturnsEmpty()
		{
			var reader = new SbusReader();
			reader.parseConfig(null);
			Assert.That(reader.buildConfig(), Is.EqualTo(""));
		}

		[Test]
		public void ParseConfig_Raw_BuildConfigContainsRaw()
		{
			var reader = new SbusReader();
			reader.parseConfig("raw");
			Assert.That(reader.buildConfig(), Is.EqualTo("raw"));
		}

		[Test]
		public void ParseConfig_Failsafe_BuildConfigContainsFailsafe()
		{
			var reader = new SbusReader();
			reader.parseConfig("failsafe");
			Assert.That(reader.buildConfig(), Is.EqualTo("failsafe"));
		}

		[Test]
		public void ParseConfig_BothFlags_BuildConfigContainsBoth()
		{
			var reader = new SbusReader();
			reader.parseConfig("raw,failsafe");
			Assert.That(reader.buildConfig(), Is.EqualTo("raw,failsafe"));
		}

		[Test]
		public void ParseConfig_ReversedOrder_BuildConfigNormalizesOrder()
		{
			var reader = new SbusReader();
			reader.parseConfig("failsafe,raw");
			// buildConfig always outputs raw first, then failsafe
			Assert.That(reader.buildConfig(), Is.EqualTo("raw,failsafe"));
		}

		[Test]
		public void ParseConfig_UnknownTokensIgnored()
		{
			var reader = new SbusReader();
			reader.parseConfig("raw,bogus,failsafe");
			Assert.That(reader.buildConfig(), Is.EqualTo("raw,failsafe"));
		}

		[Test]
		public void ParseConfig_OnlyUnknownTokens_BuildConfigReturnsEmpty()
		{
			var reader = new SbusReader();
			reader.parseConfig("foo,bar");
			Assert.That(reader.buildConfig(), Is.EqualTo(""));
		}

		[Test]
		public void ParseConfig_RoundTrip()
		{
			var reader = new SbusReader();
			reader.parseConfig("raw,failsafe");
			var config = reader.buildConfig();

			var reader2 = new SbusReader();
			reader2.parseConfig(config);
			Assert.That(reader2.buildConfig(), Is.EqualTo(config));
		}
	}

	[TestFixture]
	public class KissConfigTests
	{
		[Test]
		public void ParseConfig_EmptyString_DefaultsToDefaultUpdateRate()
		{
			var reader = new KissReader();
			reader.parseConfig("");
			// DEFAULT_UPDATE_RATE = 10
			Assert.That(reader.buildConfig(), Is.EqualTo("10"));
		}

		[Test]
		public void ParseConfig_Null_DefaultsToDefaultUpdateRate()
		{
			var reader = new KissReader();
			reader.parseConfig(null);
			Assert.That(reader.buildConfig(), Is.EqualTo("10"));
		}

		[Test]
		public void ParseConfig_ValidInteger_SetsUpdateRate()
		{
			var reader = new KissReader();
			reader.parseConfig("50");
			Assert.That(reader.buildConfig(), Is.EqualTo("50"));
		}

		[Test]
		public void ParseConfig_InvalidString_DefaultsToDefaultUpdateRate()
		{
			var reader = new KissReader();
			reader.parseConfig("abc");
			Assert.That(reader.buildConfig(), Is.EqualTo("10"));
		}

		[Test]
		public void ParseConfig_RoundTrip()
		{
			var reader = new KissReader();
			reader.parseConfig("25");
			var config = reader.buildConfig();

			var reader2 = new KissReader();
			reader2.parseConfig(config);
			Assert.That(reader2.buildConfig(), Is.EqualTo("25"));
		}
	}

	[TestFixture]
	public class MultiWiiConfigTests
	{
		[Test]
		public void ParseConfig_EmptyString_DefaultsToDefaultUpdateRate()
		{
			var reader = new MultiWiiReader();
			reader.parseConfig("");
			Assert.That(reader.buildConfig(), Is.EqualTo("10"));
		}

		[Test]
		public void ParseConfig_Null_DefaultsToDefaultUpdateRate()
		{
			var reader = new MultiWiiReader();
			reader.parseConfig(null);
			Assert.That(reader.buildConfig(), Is.EqualTo("10"));
		}

		[Test]
		public void ParseConfig_ValidInteger_SetsUpdateRate()
		{
			var reader = new MultiWiiReader();
			reader.parseConfig("100");
			Assert.That(reader.buildConfig(), Is.EqualTo("100"));
		}

		[Test]
		public void ParseConfig_InvalidString_DefaultsToDefaultUpdateRate()
		{
			var reader = new MultiWiiReader();
			reader.parseConfig("xyz");
			Assert.That(reader.buildConfig(), Is.EqualTo("10"));
		}

		[Test]
		public void ParseConfig_RoundTrip()
		{
			var reader = new MultiWiiReader();
			reader.parseConfig("75");
			var config = reader.buildConfig();

			var reader2 = new MultiWiiReader();
			reader2.parseConfig(config);
			Assert.That(reader2.buildConfig(), Is.EqualTo("75"));
		}
	}

	[TestFixture]
	public class FportConfigTests
	{
		[Test]
		public void ParseConfig_EmptyString_BuildConfigReturnsEmpty()
		{
			var reader = new FportReader();
			reader.parseConfig("");
			Assert.That(reader.buildConfig(), Is.EqualTo(""));
		}

		[Test]
		public void ParseConfig_Null_BuildConfigReturnsEmpty()
		{
			var reader = new FportReader();
			reader.parseConfig(null);
			Assert.That(reader.buildConfig(), Is.EqualTo(""));
		}

		[Test]
		public void ParseConfig_Raw_BuildConfigContainsRaw()
		{
			var reader = new FportReader();
			reader.parseConfig("raw");
			Assert.That(reader.buildConfig(), Is.EqualTo("raw"));
		}

		[Test]
		public void ParseConfig_Failsafe_BuildConfigContainsFailsafe()
		{
			var reader = new FportReader();
			reader.parseConfig("failsafe");
			Assert.That(reader.buildConfig(), Is.EqualTo("failsafe"));
		}

		[Test]
		public void ParseConfig_BothFlags_BuildConfigContainsBoth()
		{
			var reader = new FportReader();
			reader.parseConfig("raw,failsafe");
			Assert.That(reader.buildConfig(), Is.EqualTo("raw,failsafe"));
		}

		[Test]
		public void ParseConfig_ReversedOrder_BuildConfigNormalizesOrder()
		{
			var reader = new FportReader();
			reader.parseConfig("failsafe,raw");
			Assert.That(reader.buildConfig(), Is.EqualTo("raw,failsafe"));
		}

		[Test]
		public void ParseConfig_UnknownTokensIgnored()
		{
			var reader = new FportReader();
			reader.parseConfig("raw,bogus,failsafe");
			Assert.That(reader.buildConfig(), Is.EqualTo("raw,failsafe"));
		}

		[Test]
		public void ParseConfig_OnlyUnknownTokens_BuildConfigReturnsEmpty()
		{
			var reader = new FportReader();
			reader.parseConfig("foo,bar");
			Assert.That(reader.buildConfig(), Is.EqualTo(""));
		}

		[Test]
		public void ParseConfig_RoundTrip()
		{
			var reader = new FportReader();
			reader.parseConfig("raw,failsafe");
			var config = reader.buildConfig();

			var reader2 = new FportReader();
			reader2.parseConfig(config);
			Assert.That(reader2.buildConfig(), Is.EqualTo(config));
		}
	}

	[TestFixture]
	public class DummyConfigTests
	{
		[Test]
		public void ParseConfig_EmptyString_DefaultsToDefaultUpdateRate()
		{
			var reader = new DummyReader();
			reader.parseConfig("");
			Assert.That(reader.buildConfig(), Is.EqualTo("10"));
		}

		[Test]
		public void ParseConfig_Null_DefaultsToDefaultUpdateRate()
		{
			var reader = new DummyReader();
			reader.parseConfig(null);
			Assert.That(reader.buildConfig(), Is.EqualTo("10"));
		}

		[Test]
		public void ParseConfig_ValidInteger_SetsUpdateRate()
		{
			var reader = new DummyReader();
			reader.parseConfig("50");
			Assert.That(reader.buildConfig(), Is.EqualTo("50"));
		}

		[Test]
		public void ParseConfig_InvalidString_DefaultsToDefaultUpdateRate()
		{
			var reader = new DummyReader();
			reader.parseConfig("notanumber");
			Assert.That(reader.buildConfig(), Is.EqualTo("10"));
		}

		[Test]
		public void ParseConfig_RoundTrip()
		{
			var reader = new DummyReader();
			reader.parseConfig("30");
			var config = reader.buildConfig();

			var reader2 = new DummyReader();
			reader2.parseConfig(config);
			Assert.That(reader2.buildConfig(), Is.EqualTo("30"));
		}
	}
}
