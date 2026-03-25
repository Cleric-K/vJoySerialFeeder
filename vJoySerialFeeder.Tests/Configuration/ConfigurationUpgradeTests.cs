using System;
using NUnit.Framework;
using vJoySerialFeeder;

namespace vJoySerialFeeder.Tests.Configuration
{
	[TestFixture]
	public class ConfigurationUpgradeTests
	{
		// ── Gate 1: fromVersion <= 1.2.0.0 ──────────────────────

		[Test]
		public void Upgrade_From1000_AppliesGate1Defaults()
		{
			var config = new vJoySerialFeeder.Configuration();
			config.Version = "1.0.0.0";
			config.WebSocketPort = 0;

			var profile = new vJoySerialFeeder.Configuration.Profile
			{
				FailsafeTime = 0,
				FailsafeUpdateRate = 0
			};
			profile.Mappings.Add(new AxisMapping
			{
				Channel = 1,
				Parameters = new AxisMapping.AxisParameters { Failsafe = 0 }
			});
			config.PutProfile("test", profile);

			config.Upgrade(new Version("2.0.0.0"));

			Assert.That(config.WebSocketPort,
				Is.EqualTo(vJoySerialFeeder.Configuration.DEFAULT_WEBSOCKET_PORT));
			Assert.That(profile.FailsafeTime,
				Is.EqualTo(vJoySerialFeeder.Configuration.Profile.DEFAULT_FAILSAFE_TIME));
			Assert.That(profile.FailsafeUpdateRate,
				Is.EqualTo(vJoySerialFeeder.Configuration.Profile.DEFAULT_FAILSAFE_UPDATE_RATE));
			Assert.That(((AxisMapping)profile.Mappings[0]).Parameters.Failsafe,
				Is.EqualTo(AxisMapping.AxisParameters.DEFAULT_FAILSAFE));
		}

		[Test]
		public void Upgrade_From1200_StillTriggersGate1()
		{
			var config = new vJoySerialFeeder.Configuration();
			config.Version = "1.2.0.0";
			config.WebSocketPort = 0;

			config.Upgrade(new Version("2.0.0.0"));

			Assert.That(config.WebSocketPort,
				Is.EqualTo(vJoySerialFeeder.Configuration.DEFAULT_WEBSOCKET_PORT));
		}

		[Test]
		public void Upgrade_From1300_DoesNotTriggerGate1()
		{
			var config = new vJoySerialFeeder.Configuration();
			config.Version = "1.3.0.0";
			config.WebSocketPort = 12345;

			config.Upgrade(new Version("2.0.0.0"));

			Assert.That(config.WebSocketPort, Is.EqualTo(12345));
		}

		[Test]
		public void Upgrade_Gate1_AxisMappingFailsafeSetToMinusOne()
		{
			var config = new vJoySerialFeeder.Configuration();
			config.Version = "1.0.0.0";

			var profile = new vJoySerialFeeder.Configuration.Profile();
			profile.Mappings.Add(new AxisMapping
			{
				Channel = 0,
				Parameters = new AxisMapping.AxisParameters { Failsafe = 999 }
			});
			profile.Mappings.Add(new AxisMapping
			{
				Channel = 1,
				Parameters = new AxisMapping.AxisParameters { Failsafe = 42 }
			});
			config.PutProfile("test", profile);

			config.Upgrade(new Version("2.0.0.0"));

			Assert.That(((AxisMapping)profile.Mappings[0]).Parameters.Failsafe, Is.EqualTo(-1));
			Assert.That(((AxisMapping)profile.Mappings[1]).Parameters.Failsafe, Is.EqualTo(-1));
		}

		[Test]
		public void Upgrade_Gate1_ButtonMappingsNotAffected()
		{
			var config = new vJoySerialFeeder.Configuration();
			config.Version = "1.0.0.0";

			var profile = new vJoySerialFeeder.Configuration.Profile();
			profile.Mappings.Add(new ButtonMapping { Channel = 5 });
			config.PutProfile("test", profile);

			config.Upgrade(new Version("2.0.0.0"));

			Assert.That(profile.Mappings[0], Is.InstanceOf<ButtonMapping>());
			Assert.That(profile.Mappings[0].Channel, Is.EqualTo(5));
		}

		[Test]
		public void Upgrade_Gate1_EmptyProfiles_DoesNotCrash()
		{
			var config = new vJoySerialFeeder.Configuration();
			config.Version = "1.0.0.0";

			Assert.DoesNotThrow(() => config.Upgrade(new Version("2.0.0.0")));
		}

		// ── Gate 2: fromVersion <= 1.6.0.0 ──────────────────────

		[Test]
		public void Upgrade_Gate2_Protocol0_BecomesIbusReader()
		{
			var config = new vJoySerialFeeder.Configuration();
			config.Version = "1.5.0.0";

			var profile = new vJoySerialFeeder.Configuration.Profile { Protocol = "0" };
			config.PutProfile("test", profile);

			config.Upgrade(new Version("2.0.0.0"));

			Assert.That(profile.Protocol, Is.EqualTo("IbusReader"));
		}

		[Test]
		public void Upgrade_Gate2_Protocol3_BecomesSbusReader()
		{
			var config = new vJoySerialFeeder.Configuration();
			config.Version = "1.5.0.0";

			var profile = new vJoySerialFeeder.Configuration.Profile { Protocol = "3" };
			config.PutProfile("test", profile);

			config.Upgrade(new Version("2.0.0.0"));

			Assert.That(profile.Protocol, Is.EqualTo("SbusReader"));
		}

		[Test]
		public void Upgrade_Gate2_Protocol7_BecomesDummyReader()
		{
			var config = new vJoySerialFeeder.Configuration();
			config.Version = "1.5.0.0";

			var profile = new vJoySerialFeeder.Configuration.Profile { Protocol = "7" };
			config.PutProfile("test", profile);

			config.Upgrade(new Version("2.0.0.0"));

			Assert.That(profile.Protocol, Is.EqualTo("DummyReader"));
		}

		[Test]
		public void Upgrade_Gate2_ProtocolOutOfRange_BecomesEmpty()
		{
			var config = new vJoySerialFeeder.Configuration();
			config.Version = "1.5.0.0";

			var profile = new vJoySerialFeeder.Configuration.Profile { Protocol = "99" };
			config.PutProfile("test", profile);

			config.Upgrade(new Version("2.0.0.0"));

			Assert.That(profile.Protocol, Is.EqualTo(""));
		}

		[Test]
		public void Upgrade_Gate2_ProtocolNonNumeric_BecomesEmpty()
		{
			var config = new vJoySerialFeeder.Configuration();
			config.Version = "1.5.0.0";

			var profile = new vJoySerialFeeder.Configuration.Profile { Protocol = "abc" };
			config.PutProfile("test", profile);

			config.Upgrade(new Version("2.0.0.0"));

			Assert.That(profile.Protocol, Is.EqualTo(""));
		}

		[Test]
		public void Upgrade_From1700_DoesNotTriggerGate2()
		{
			var config = new vJoySerialFeeder.Configuration();
			config.Version = "1.7.0.0";

			var profile = new vJoySerialFeeder.Configuration.Profile { Protocol = "SbusReader" };
			config.PutProfile("test", profile);

			config.Upgrade(new Version("2.0.0.0"));

			Assert.That(profile.Protocol, Is.EqualTo("SbusReader"));
		}

		// ── Combined / general behavior ─────────────────────────

		[Test]
		public void Upgrade_From1000_BothGatesTrigger()
		{
			var config = new vJoySerialFeeder.Configuration();
			config.Version = "1.0.0.0";
			config.WebSocketPort = 0;

			var profile = new vJoySerialFeeder.Configuration.Profile
			{
				Protocol = "3",
				FailsafeTime = 0,
				FailsafeUpdateRate = 0
			};
			profile.Mappings.Add(new AxisMapping
			{
				Channel = 0,
				Parameters = new AxisMapping.AxisParameters { Failsafe = 0 }
			});
			config.PutProfile("test", profile);

			config.Upgrade(new Version("2.0.0.0"));

			// Gate 1
			Assert.That(config.WebSocketPort,
				Is.EqualTo(vJoySerialFeeder.Configuration.DEFAULT_WEBSOCKET_PORT));
			Assert.That(profile.FailsafeTime,
				Is.EqualTo(vJoySerialFeeder.Configuration.Profile.DEFAULT_FAILSAFE_TIME));
			Assert.That(((AxisMapping)profile.Mappings[0]).Parameters.Failsafe,
				Is.EqualTo(AxisMapping.AxisParameters.DEFAULT_FAILSAFE));

			// Gate 2
			Assert.That(profile.Protocol, Is.EqualTo("SbusReader"));
		}

		[Test]
		public void Upgrade_ReturnsTrue_WhenUpgradePerformed()
		{
			var config = new vJoySerialFeeder.Configuration();
			config.Version = "1.0.0.0";

			var result = config.Upgrade(new Version("2.0.0.0"));

			Assert.That(result, Is.True);
		}

		[Test]
		public void Upgrade_SetsVersionToTargetVersion()
		{
			var config = new vJoySerialFeeder.Configuration();
			config.Version = "1.0.0.0";

			config.Upgrade(new Version("2.0.0.0"));

			Assert.That(config.Version, Is.EqualTo("2.0.0.0"));
		}

		[Test]
		public void Upgrade_ReturnsFalse_WhenVersionsEqual()
		{
			var config = new vJoySerialFeeder.Configuration();
			config.Version = "2.0.0.0";

			var result = config.Upgrade(new Version("2.0.0.0"));

			Assert.That(result, Is.False);
		}

		[Test]
		public void Upgrade_ReturnsFalse_WhenCurrentVersionLower()
		{
			var config = new vJoySerialFeeder.Configuration();
			config.Version = "3.0.0.0";

			var result = config.Upgrade(new Version("2.0.0.0"));

			Assert.That(result, Is.False);
		}

		[Test]
		public void Upgrade_Parameterless_InTestContext_StillReturnsFalse()
		{
			// Parameterless Upgrade() uses RuntimeVersion which is 0.0.0.0 in tests
			var config = new vJoySerialFeeder.Configuration();
			config.Version = "1.0.0.0";

			Assert.That(config.Upgrade(), Is.False);
		}

		[Test]
		public void Upgrade_InvalidVersionString_TreatsAsZero()
		{
			var config = new vJoySerialFeeder.Configuration();
			config.Version = "not_a_version";

			var result = config.Upgrade(new Version("2.0.0.0"));

			// Invalid version falls back to 0.0.0.0, which < 2.0.0.0
			Assert.That(result, Is.True);
		}

		[Test]
		public void Upgrade_NullVersion_TreatsAsZero()
		{
			var config = new vJoySerialFeeder.Configuration();
			config.Version = null;

			var result = config.Upgrade(new Version("2.0.0.0"));

			Assert.That(result, Is.True);
		}

		[Test]
		public void Upgrade_Gate2_AllProtocolIndices()
		{
			string[] expected = {
				"IbusReader", "KissReader", "MultiWiiReader", "SbusReader",
				"FportReader", "DsmReader", "CrsfReader", "DummyReader"
			};

			for (int i = 0; i < expected.Length; i++)
			{
				var config = new vJoySerialFeeder.Configuration();
				config.Version = "1.5.0.0";
				var profile = new vJoySerialFeeder.Configuration.Profile { Protocol = i.ToString() };
				config.PutProfile("test", profile);

				config.Upgrade(new Version("2.0.0.0"));

				Assert.That(profile.Protocol, Is.EqualTo(expected[i]),
					$"Protocol index {i} should map to {expected[i]}");
			}
		}
	}
}
