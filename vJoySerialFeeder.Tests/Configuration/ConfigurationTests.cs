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

		// ── ButtonBitmapMapping serialization ───────────────────

		[Test]
		public void ToJSONString_RoundTrips_ProfileWithButtonBitmapMapping()
		{
			var config = new vJoySerialFeeder.Configuration();
			var profile = new vJoySerialFeeder.Configuration.Profile {
				Protocol = "IbusReader"
			};
			var mapping = new ButtonBitmapMapping { Channel = 3 };
			mapping.Parameters[0] = new ButtonBitmapMapping.BitButtonParameters {
				Button = 5, Enabled = true, Invert = true,
				Trigger = true, Failsafe = 2, TriggerDuration = 300,
				TriggerEdge = TriggerState.Edge.Falling
			};
			profile.Mappings.Add(mapping);
			config.PutProfile("bitmap", profile);

			var json = config.ToJSONString();
			var loaded = vJoySerialFeeder.Configuration.LoadFromJSONString(json);

			var loadedProfile = loaded.GetProfile("bitmap");
			Assert.That(loadedProfile.Mappings[0], Is.InstanceOf<ButtonBitmapMapping>());
			var loadedMapping = (ButtonBitmapMapping)loadedProfile.Mappings[0];
			Assert.That(loadedMapping.Channel, Is.EqualTo(3));
			Assert.That(loadedMapping.Parameters[0].Button, Is.EqualTo(5));
			Assert.That(loadedMapping.Parameters[0].Enabled, Is.True);
			Assert.That(loadedMapping.Parameters[0].Invert, Is.True);
			Assert.That(loadedMapping.Parameters[0].Trigger, Is.True);
			Assert.That(loadedMapping.Parameters[0].Failsafe, Is.EqualTo(2));
			Assert.That(loadedMapping.Parameters[0].TriggerDuration, Is.EqualTo(300));
			Assert.That(loadedMapping.Parameters[0].TriggerEdge, Is.EqualTo(TriggerState.Edge.Falling));
		}

		// ── Mixed mapping types ─────────────────────────────────

		[Test]
		public void ToJSONString_RoundTrips_MixedMappingTypes()
		{
			var config = new vJoySerialFeeder.Configuration();
			var profile = new vJoySerialFeeder.Configuration.Profile();
			profile.Mappings.Add(new AxisMapping { Channel = 0 });
			profile.Mappings.Add(new ButtonMapping { Channel = 1 });
			profile.Mappings.Add(new ButtonBitmapMapping { Channel = 2 });
			config.PutProfile("mixed", profile);

			var json = config.ToJSONString();
			var loaded = vJoySerialFeeder.Configuration.LoadFromJSONString(json);

			var p = loaded.GetProfile("mixed");
			Assert.That(p.Mappings.Count, Is.EqualTo(3));
			Assert.That(p.Mappings[0], Is.InstanceOf<AxisMapping>());
			Assert.That(p.Mappings[1], Is.InstanceOf<ButtonMapping>());
			Assert.That(p.Mappings[2], Is.InstanceOf<ButtonBitmapMapping>());
			Assert.That(p.Mappings[0].Channel, Is.EqualTo(0));
			Assert.That(p.Mappings[1].Channel, Is.EqualTo(1));
			Assert.That(p.Mappings[2].Channel, Is.EqualTo(2));
		}

		// ── Edge cases ──────────────────────────────────────────

		[Test]
		public void ToJSONString_RoundTrips_EmptyStringFields()
		{
			var config = new vJoySerialFeeder.Configuration();
			var profile = new vJoySerialFeeder.Configuration.Profile {
				COMPort = "",
				Protocol = "",
				ProtocolConfiguration = "",
				VJoyInstance = ""
			};
			config.PutProfile("empty_strings", profile);

			var json = config.ToJSONString();
			var loaded = vJoySerialFeeder.Configuration.LoadFromJSONString(json);

			var p = loaded.GetProfile("empty_strings");
			Assert.That(p.COMPort, Is.EqualTo(""));
			Assert.That(p.Protocol, Is.EqualTo(""));
			Assert.That(p.ProtocolConfiguration, Is.EqualTo(""));
			Assert.That(p.VJoyInstance, Is.EqualTo(""));
		}

		[Test]
		public void ToJSONString_RoundTrips_NullLuaScript()
		{
			var config = new vJoySerialFeeder.Configuration();
			var profile = new vJoySerialFeeder.Configuration.Profile {
				LuaScript = null
			};
			config.PutProfile("null_lua", profile);

			var json = config.ToJSONString();
			var loaded = vJoySerialFeeder.Configuration.LoadFromJSONString(json);

			Assert.That(loaded.GetProfile("null_lua").LuaScript, Is.Null);
		}

		[Test]
		public void ToJSONString_RoundTrips_ProfileWithEmptyMappings()
		{
			var config = new vJoySerialFeeder.Configuration();
			var profile = new vJoySerialFeeder.Configuration.Profile();
			// Mappings list is empty by default
			config.PutProfile("no_mappings", profile);

			var json = config.ToJSONString();
			var loaded = vJoySerialFeeder.Configuration.LoadFromJSONString(json);

			Assert.That(loaded.GetProfile("no_mappings").Mappings.Count, Is.EqualTo(0));
		}

		[Test]
		public void ToJSONString_RoundTrips_MultipleProfiles()
		{
			var config = new vJoySerialFeeder.Configuration();
			config.PutProfile("a", new vJoySerialFeeder.Configuration.Profile { COMPort = "COM1" });
			config.PutProfile("b", new vJoySerialFeeder.Configuration.Profile { COMPort = "COM2" });
			config.PutProfile("c", new vJoySerialFeeder.Configuration.Profile { COMPort = "COM3" });

			var json = config.ToJSONString();
			var loaded = vJoySerialFeeder.Configuration.LoadFromJSONString(json);

			Assert.That(loaded.Profiles.Count, Is.EqualTo(3));
			Assert.That(loaded.GetProfile("a").COMPort, Is.EqualTo("COM1"));
			Assert.That(loaded.GetProfile("b").COMPort, Is.EqualTo("COM2"));
			Assert.That(loaded.GetProfile("c").COMPort, Is.EqualTo("COM3"));
		}

		[Test]
		public void Merge_EmptySource_NoChange()
		{
			var config = new vJoySerialFeeder.Configuration();
			config.PutProfile("existing", new vJoySerialFeeder.Configuration.Profile());

			var empty = new vJoySerialFeeder.Configuration();
			config.Merge(empty, importGlobalOptions: true, importProfiles: true);

			Assert.That(config.Profiles.Count, Is.EqualTo(1));
		}

		[Test]
		public void Merge_GlobalOptionsOnly_ProfilesUnchanged()
		{
			var config = new vJoySerialFeeder.Configuration {
				WebSocketEnabled = false, WebSocketPort = 40000
			};
			config.PutProfile("keep", new vJoySerialFeeder.Configuration.Profile());

			var source = new vJoySerialFeeder.Configuration {
				WebSocketEnabled = true, WebSocketPort = 55555
			};
			source.PutProfile("new_profile", new vJoySerialFeeder.Configuration.Profile());

			config.Merge(source, importGlobalOptions: true, importProfiles: false);

			Assert.That(config.WebSocketEnabled, Is.True);
			Assert.That(config.WebSocketPort, Is.EqualTo(55555));
			Assert.That(config.Profiles.Count, Is.EqualTo(1));
			Assert.That(config.GetProfile("new_profile"), Is.Null);
		}

		[Test]
		public void LoadFromJSONString_MalformedJSON_ThrowsSerializationException()
		{
			Assert.Throws<System.Runtime.Serialization.SerializationException>(
				() => vJoySerialFeeder.Configuration.LoadFromJSONString("not json at all"));
		}

		[Test]
		public void NewConfiguration_HasExpectedDefaults()
		{
			var config = new vJoySerialFeeder.Configuration();
			Assert.That(config.DefaultProfile, Is.EqualTo(""));
			Assert.That(config.Profiles.Count, Is.EqualTo(0));
			Assert.That(config.WebSocketEnabled, Is.False);
			Assert.That(config.WebSocketPort, Is.EqualTo(40000));
			Assert.That(config.Autoconnect, Is.False);
			Assert.That(config.MinimizeToTray, Is.False);
		}

		[Test]
		public void NewProfile_HasExpectedDefaults()
		{
			var profile = new vJoySerialFeeder.Configuration.Profile();
			Assert.That(profile.FailsafeTime,
				Is.EqualTo(vJoySerialFeeder.Configuration.Profile.DEFAULT_FAILSAFE_TIME));
			Assert.That(profile.FailsafeUpdateRate,
				Is.EqualTo(vJoySerialFeeder.Configuration.Profile.DEFAULT_FAILSAFE_UPDATE_RATE));
			Assert.That(profile.Mappings, Is.Not.Null);
			Assert.That(profile.Mappings.Count, Is.EqualTo(0));
			Assert.That(profile.ProtocolConfiguration, Is.EqualTo(""));
		}

		[Test]
		public void PutProfile_OverwritesExisting()
		{
			var config = new vJoySerialFeeder.Configuration();
			config.PutProfile("test", new vJoySerialFeeder.Configuration.Profile { COMPort = "COM1" });
			config.PutProfile("test", new vJoySerialFeeder.Configuration.Profile { COMPort = "COM2" });

			Assert.That(config.Profiles.Count, Is.EqualTo(1));
			Assert.That(config.GetProfile("test").COMPort, Is.EqualTo("COM2"));
		}

		[Test]
		public void Merge_MultipleConflicts_IncrementsSuffix()
		{
			var config = new vJoySerialFeeder.Configuration();
			config.PutProfile("p", new vJoySerialFeeder.Configuration.Profile { COMPort = "original" });
			config.PutProfile("p_1", new vJoySerialFeeder.Configuration.Profile { COMPort = "also_taken" });

			var source = new vJoySerialFeeder.Configuration();
			source.PutProfile("p", new vJoySerialFeeder.Configuration.Profile { COMPort = "imported" });

			config.Merge(source, importGlobalOptions: false, importProfiles: true);

			Assert.That(config.GetProfile("p").COMPort, Is.EqualTo("original"));
			Assert.That(config.GetProfile("p_1").COMPort, Is.EqualTo("also_taken"));
			Assert.That(config.GetProfile("p_2").COMPort, Is.EqualTo("imported"));
		}
	}
}
