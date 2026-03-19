using System.IO.Ports;
using NUnit.Framework;
using vJoySerialFeeder;

namespace vJoySerialFeeder.Tests.Configuration
{
	[TestFixture]
	public class ConfigurationTests
	{
		[Test]
		public void ToJSONString_RoundTrips_EmptyConfiguration()
		{
			var config = new vJoySerialFeeder.Configuration();
			var json = config.ToJSONString();
			var loaded = vJoySerialFeeder.Configuration.LoadFromJSONString(json);

			Assert.That(loaded, Is.Not.Null);
			Assert.That(loaded.DefaultProfile, Is.EqualTo(""));
			Assert.That(loaded.Profiles.Count, Is.EqualTo(0));
			Assert.That(loaded.WebSocketPort, Is.EqualTo(vJoySerialFeeder.Configuration.DEFAULT_WEBSOCKET_PORT));
		}

		[Test]
		public void ToJSONString_RoundTrips_ProfileWithAxisMapping()
		{
			var config = new vJoySerialFeeder.Configuration();
			var profile = new vJoySerialFeeder.Configuration.Profile {
				COMPort = "COM3",
				VJoyInstance = "vJoy.1",
				Protocol = "IbusReader",
				ProtocolConfiguration = "ia6",
				LuaScript = "function update() end",
				FailsafeTime = 750,
				FailsafeUpdateRate = 200
			};
			profile.Mappings.Add(new AxisMapping {
				Channel = 1,
				Axis = 0,
				Parameters = new AxisMapping.AxisParameters {
					Min = 1000, Max = 2000, Center = 1500,
					Symmetric = true, Expo = 25, Deadband = 5,
					Failsafe = -1, Invert = false
				}
			});
			config.PutProfile("test", profile);
			config.DefaultProfile = "test";

			var json = config.ToJSONString();
			var loaded = vJoySerialFeeder.Configuration.LoadFromJSONString(json);

			Assert.That(loaded.DefaultProfile, Is.EqualTo("test"));
			var loadedProfile = loaded.GetProfile("test");
			Assert.That(loadedProfile, Is.Not.Null);
			Assert.That(loadedProfile.COMPort, Is.EqualTo("COM3"));
			Assert.That(loadedProfile.Protocol, Is.EqualTo("IbusReader"));
			Assert.That(loadedProfile.FailsafeTime, Is.EqualTo(750));
			Assert.That(loadedProfile.Mappings.Count, Is.EqualTo(1));

			var mapping = loadedProfile.Mappings[0] as AxisMapping;
			Assert.That(mapping, Is.Not.Null);
			Assert.That(mapping.Channel, Is.EqualTo(1));
			Assert.That(mapping.Parameters.Min, Is.EqualTo(1000));
			Assert.That(mapping.Parameters.Max, Is.EqualTo(2000));
			Assert.That(mapping.Parameters.Symmetric, Is.True);
			Assert.That(mapping.Parameters.Expo, Is.EqualTo(25));
		}

		[Test]
		public void ToJSONString_RoundTrips_ProfileWithButtonMapping()
		{
			var config = new vJoySerialFeeder.Configuration();
			var profile = new vJoySerialFeeder.Configuration.Profile {
				Protocol = "SbusReader"
			};
			profile.Mappings.Add(new ButtonMapping {
				Channel = 5
			});
			config.PutProfile("buttons", profile);

			var json = config.ToJSONString();
			var loaded = vJoySerialFeeder.Configuration.LoadFromJSONString(json);

			var loadedProfile = loaded.GetProfile("buttons");
			Assert.That(loadedProfile.Mappings[0], Is.InstanceOf<ButtonMapping>());
			Assert.That(loadedProfile.Mappings[0].Channel, Is.EqualTo(5));
		}

		[Test]
		public void ToJSONString_RoundTrips_ProfileWithSerialParameters()
		{
			var config = new vJoySerialFeeder.Configuration();
			var profile = new vJoySerialFeeder.Configuration.Profile {
				UseCustomSerialParameters = true,
				SerialParameters = new vJoySerialFeeder.Configuration.SerialParameters {
					BaudRate = 100000,
					DataBits = 8,
					Parity = Parity.Even,
					StopBits = StopBits.Two
				}
			};
			config.PutProfile("custom_serial", profile);

			var json = config.ToJSONString();
			var loaded = vJoySerialFeeder.Configuration.LoadFromJSONString(json);

			var loadedProfile = loaded.GetProfile("custom_serial");
			Assert.That(loadedProfile.UseCustomSerialParameters, Is.True);
			Assert.That(loadedProfile.SerialParameters.BaudRate, Is.EqualTo(100000));
			Assert.That(loadedProfile.SerialParameters.Parity, Is.EqualTo(Parity.Even));
			Assert.That(loadedProfile.SerialParameters.StopBits, Is.EqualTo(StopBits.Two));
		}

		[Test]
		public void ToJSONString_RoundTrips_GlobalOptions()
		{
			var config = new vJoySerialFeeder.Configuration {
				WebSocketEnabled = true,
				WebSocketPort = 12345,
				Autoconnect = true,
				MinimizeToTray = true
			};

			var json = config.ToJSONString();
			var loaded = vJoySerialFeeder.Configuration.LoadFromJSONString(json);

			Assert.That(loaded.WebSocketEnabled, Is.True);
			Assert.That(loaded.WebSocketPort, Is.EqualTo(12345));
			Assert.That(loaded.Autoconnect, Is.True);
			Assert.That(loaded.MinimizeToTray, Is.True);
		}

		[Test]
		public void PutProfile_GetProfile_DeleteProfile()
		{
			var config = new vJoySerialFeeder.Configuration();

			Assert.That(config.GetProfile("test"), Is.Null);

			var profile = new vJoySerialFeeder.Configuration.Profile { COMPort = "COM1" };
			config.PutProfile("test", profile);

			Assert.That(config.GetProfile("test"), Is.Not.Null);
			Assert.That(config.GetProfile("test").COMPort, Is.EqualTo("COM1"));

			config.DeleteProfile("test");
			Assert.That(config.GetProfile("test"), Is.Null);
		}

		[Test]
		public void GetProfileNames_ReturnsAllProfileNames()
		{
			var config = new vJoySerialFeeder.Configuration();
			config.PutProfile("alpha", new vJoySerialFeeder.Configuration.Profile());
			config.PutProfile("beta", new vJoySerialFeeder.Configuration.Profile());

			var names = config.GetProfileNames();
			Assert.That(names, Has.Length.EqualTo(2));
			Assert.That(names, Does.Contain("alpha"));
			Assert.That(names, Does.Contain("beta"));
		}

		[Test]
		public void Merge_ImportsProfiles_RenamesOnConflict()
		{
			var config1 = new vJoySerialFeeder.Configuration();
			config1.PutProfile("shared", new vJoySerialFeeder.Configuration.Profile { COMPort = "COM1" });

			var config2 = new vJoySerialFeeder.Configuration();
			config2.PutProfile("shared", new vJoySerialFeeder.Configuration.Profile { COMPort = "COM2" });
			config2.PutProfile("unique", new vJoySerialFeeder.Configuration.Profile { COMPort = "COM3" });

			config1.Merge(config2, importGlobalOptions: false, importProfiles: true);

			Assert.That(config1.GetProfile("shared").COMPort, Is.EqualTo("COM1")); // original preserved
			Assert.That(config1.GetProfile("shared_1").COMPort, Is.EqualTo("COM2")); // conflict renamed
			Assert.That(config1.GetProfile("unique").COMPort, Is.EqualTo("COM3"));
		}

		[Test]
		public void Merge_ImportsGlobalOptions()
		{
			var config1 = new vJoySerialFeeder.Configuration {
				WebSocketEnabled = false,
				WebSocketPort = 40000
			};

			var config2 = new vJoySerialFeeder.Configuration {
				WebSocketEnabled = true,
				WebSocketPort = 50000
			};

			config1.Merge(config2, importGlobalOptions: true, importProfiles: false);

			Assert.That(config1.WebSocketEnabled, Is.True);
			Assert.That(config1.WebSocketPort, Is.EqualTo(50000));
		}

		[Test]
		public void ProfilesEqual_IdenticalProfiles_ReturnsTrue()
		{
			var p1 = new vJoySerialFeeder.Configuration.Profile { COMPort = "COM1", Protocol = "IbusReader" };
			var p2 = new vJoySerialFeeder.Configuration.Profile { COMPort = "COM1", Protocol = "IbusReader" };
			Assert.That(vJoySerialFeeder.Configuration.ProfilesEqual(p1, p2), Is.True);
		}

		[Test]
		public void ProfilesEqual_DifferentProfiles_ReturnsFalse()
		{
			var p1 = new vJoySerialFeeder.Configuration.Profile { COMPort = "COM1" };
			var p2 = new vJoySerialFeeder.Configuration.Profile { COMPort = "COM2" };
			Assert.That(vJoySerialFeeder.Configuration.ProfilesEqual(p1, p2), Is.False);
		}

		[Test]
		public void ProfilesEqual_BothNull_ReturnsTrue()
		{
			Assert.That(vJoySerialFeeder.Configuration.ProfilesEqual(null, null), Is.True);
		}

		[Test]
		public void ProfilesEqual_OneNull_ReturnsFalse()
		{
			var p = new vJoySerialFeeder.Configuration.Profile();
			Assert.That(vJoySerialFeeder.Configuration.ProfilesEqual(p, null), Is.False);
			Assert.That(vJoySerialFeeder.Configuration.ProfilesEqual(null, p), Is.False);
		}

		[Test]
		public void DeleteProfile_NonExistent_DoesNotThrow()
		{
			var config = new vJoySerialFeeder.Configuration();
			Assert.DoesNotThrow(() => config.DeleteProfile("nonexistent"));
		}
	}
}
