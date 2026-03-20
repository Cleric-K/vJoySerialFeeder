using System;
using System.Runtime.Serialization;
using NUnit.Framework;
using vJoySerialFeeder;

namespace vJoySerialFeeder.Tests.Configuration
{
	/// <summary>
	/// Tests that pin down edge-case and error behaviors in Configuration
	/// discovered by tracing the implementation.
	/// </summary>
	[TestFixture]
	public class ConfigurationBehaviorTests
	{
		// ── LoadFromJSONString error handling ────────────────────

		[Test]
		public void LoadFromJSONString_Null_ThrowsArgumentNullException()
		{
			// Encoding.UTF8.GetBytes(null) throws ArgumentNullException
			Assert.Throws<ArgumentNullException>(
				() => vJoySerialFeeder.Configuration.LoadFromJSONString(null));
		}

		[Test]
		public void LoadFromJSONString_EmptyString_ThrowsSerializationException()
		{
			// Empty MemoryStream causes ReadObject to fail
			Assert.Throws<SerializationException>(
				() => vJoySerialFeeder.Configuration.LoadFromJSONString(""));
		}

		[Test]
		public void LoadFromJSONString_ValidJsonWrongShape_ProfilesDefaultsToEmptyDictionary()
		{
			// DataContractJsonSerializer silently accepts a JSON array and
			// returns a Configuration. Field initializers are NOT run during
			// deserialization, but the [OnDeserialized] callback ensures
			// Profiles is never null.
			var config = vJoySerialFeeder.Configuration.LoadFromJSONString("[]");
			Assert.That(config, Is.Not.Null);
			Assert.That(config.Profiles, Is.Not.Null);
			Assert.That(config.Profiles.Count, Is.EqualTo(0));
		}

		[Test]
		public void LoadFromJSONString_EmptyObject_ProfilesDefaultsToEmptyDictionary()
		{
			// A valid JSON object with no Profiles key should still get
			// an empty dictionary via the [OnDeserialized] callback.
			var config = vJoySerialFeeder.Configuration.LoadFromJSONString("{}");
			Assert.That(config, Is.Not.Null);
			Assert.That(config.Profiles, Is.Not.Null);
			Assert.That(config.Profiles.Count, Is.EqualTo(0));
		}

		[Test]
		public void LoadFromJSONString_WithProfiles_ProfilesPreserved()
		{
			// When Profiles is present in JSON, deserialization should
			// preserve the actual data (OnDeserialized should not overwrite it).
			var config = new vJoySerialFeeder.Configuration();
			config.PutProfile("test", new vJoySerialFeeder.Configuration.Profile {
				COMPort = "COM3"
			});
			var json = config.ToJSONString();

			var loaded = vJoySerialFeeder.Configuration.LoadFromJSONString(json);
			Assert.That(loaded.Profiles, Is.Not.Null);
			Assert.That(loaded.Profiles.Count, Is.EqualTo(1));
			Assert.That(loaded.GetProfile("test").COMPort, Is.EqualTo("COM3"));
		}

		[Test]
		public void LoadFromJSONString_UnknownFields_IgnoredGracefully()
		{
			// DataContract serializer ignores unknown fields (forward compatibility)
			// Start with valid config JSON and add an unknown field
			var config = new vJoySerialFeeder.Configuration();
			config.PutProfile("test", new vJoySerialFeeder.Configuration.Profile());
			var json = config.ToJSONString();

			// Inject unknown field into JSON
			var modified = json.Replace("\"DefaultProfile\"",
				"\"UnknownField\":42,\"DefaultProfile\"");

			var loaded = vJoySerialFeeder.Configuration.LoadFromJSONString(modified);
			Assert.That(loaded, Is.Not.Null);
			Assert.That(loaded.GetProfile("test"), Is.Not.Null);
		}

		[Test]
		public void LoadFromJSONString_UnicodeInProfileName()
		{
			var config = new vJoySerialFeeder.Configuration();
			config.PutProfile("プロファイル", new vJoySerialFeeder.Configuration.Profile {
				COMPort = "日本語テスト"
			});

			var json = config.ToJSONString();
			var loaded = vJoySerialFeeder.Configuration.LoadFromJSONString(json);

			Assert.That(loaded.GetProfile("プロファイル"), Is.Not.Null);
			Assert.That(loaded.GetProfile("プロファイル").COMPort, Is.EqualTo("日本語テスト"));
		}

		// ── Upgrade() in test context ───────────────────────────
		// RuntimeVersion is 0.0.0.0 in tests (no entry assembly),
		// so Upgrade() never triggers. Pin this behavior.

		[Test]
		public void Upgrade_InTestContext_AlwaysReturnsFalse()
		{
			var config = new vJoySerialFeeder.Configuration();
			Assert.That(config.Upgrade(), Is.False);
		}

		[Test]
		public void Upgrade_WithHighVersion_StillReturnsFalse()
		{
			// Even with version set high, RuntimeVersion is 0.0.0.0
			// so prevVersion (99.0) > currentVersion (0.0) — no upgrade
			var config = new vJoySerialFeeder.Configuration();
			config.Version = "99.0.0.0";
			Assert.That(config.Upgrade(), Is.False);
		}

		[Test]
		public void Upgrade_InvalidVersionString_DoesNotThrow()
		{
			// Invalid version string is caught internally
			var config = new vJoySerialFeeder.Configuration();
			config.Version = "not_a_version";
			Assert.DoesNotThrow(() => config.Upgrade());
		}

		[Test]
		public void Upgrade_NullVersion_DoesNotThrow()
		{
			var config = new vJoySerialFeeder.Configuration();
			config.Version = null;
			Assert.DoesNotThrow(() => config.Upgrade());
		}

		[Test]
		public void Upgrade_EmptyVersion_DoesNotThrow()
		{
			var config = new vJoySerialFeeder.Configuration();
			config.Version = "";
			Assert.DoesNotThrow(() => config.Upgrade());
		}

		// ── RuntimeVersion in test context ──────────────────────

		[Test]
		public void RuntimeVersion_InTestContext_IsZero()
		{
			// Pin the fact that tests run with version 0.0.0.0
			Assert.That(vJoySerialFeeder.Configuration.RuntimeVersion,
				Is.EqualTo(new Version(0, 0, 0, 0)));
		}

		// ── Merge edge cases ────────────────────────────────────

		[Test]
		public void Merge_BothFlags_False_NoChanges()
		{
			var config = new vJoySerialFeeder.Configuration {
				WebSocketEnabled = false, WebSocketPort = 40000
			};
			config.PutProfile("keep", new vJoySerialFeeder.Configuration.Profile());

			var source = new vJoySerialFeeder.Configuration {
				WebSocketEnabled = true, WebSocketPort = 55555
			};
			source.PutProfile("new", new vJoySerialFeeder.Configuration.Profile());

			config.Merge(source, importGlobalOptions: false, importProfiles: false);

			Assert.That(config.WebSocketEnabled, Is.False);
			Assert.That(config.WebSocketPort, Is.EqualTo(40000));
			Assert.That(config.Profiles.Count, Is.EqualTo(1));
			Assert.That(config.GetProfile("new"), Is.Null);
		}

		[Test]
		public void Merge_ProfileNamedWithUnderscore_ConflictResolution()
		{
			// Source has profile "a", target has "a" and "a_1"
			// Merge should create "a_2"
			var config = new vJoySerialFeeder.Configuration();
			config.PutProfile("a", new vJoySerialFeeder.Configuration.Profile { COMPort = "orig" });
			config.PutProfile("a_1", new vJoySerialFeeder.Configuration.Profile { COMPort = "also_orig" });

			var source = new vJoySerialFeeder.Configuration();
			source.PutProfile("a", new vJoySerialFeeder.Configuration.Profile { COMPort = "imported" });

			config.Merge(source, importGlobalOptions: false, importProfiles: true);

			Assert.That(config.GetProfile("a").COMPort, Is.EqualTo("orig"));
			Assert.That(config.GetProfile("a_1").COMPort, Is.EqualTo("also_orig"));
			Assert.That(config.GetProfile("a_2").COMPort, Is.EqualTo("imported"));
		}

		[Test]
		public void Merge_EmptyProfileName()
		{
			// Profile with empty string as name
			var config = new vJoySerialFeeder.Configuration();
			var source = new vJoySerialFeeder.Configuration();
			source.PutProfile("", new vJoySerialFeeder.Configuration.Profile { COMPort = "empty_name" });

			config.Merge(source, importGlobalOptions: false, importProfiles: true);

			Assert.That(config.GetProfile(""), Is.Not.Null);
			Assert.That(config.GetProfile("").COMPort, Is.EqualTo("empty_name"));
		}

		// ── ProfilesEqual behavior ──────────────────────────────

		[Test]
		public void ProfilesEqual_SameDataDifferentMappingOrder_NotEqual()
		{
			// Serialization preserves order, so different order = not equal
			var p1 = new vJoySerialFeeder.Configuration.Profile();
			p1.Mappings.Add(new AxisMapping { Channel = 0 });
			p1.Mappings.Add(new ButtonMapping { Channel = 1 });

			var p2 = new vJoySerialFeeder.Configuration.Profile();
			p2.Mappings.Add(new ButtonMapping { Channel = 1 });
			p2.Mappings.Add(new AxisMapping { Channel = 0 });

			Assert.That(vJoySerialFeeder.Configuration.ProfilesEqual(p1, p2), Is.False);
		}

		[Test]
		public void ProfilesEqual_EmptyMappingsLists_Equal()
		{
			var p1 = new vJoySerialFeeder.Configuration.Profile();
			var p2 = new vJoySerialFeeder.Configuration.Profile();
			Assert.That(vJoySerialFeeder.Configuration.ProfilesEqual(p1, p2), Is.True);
		}

		[Test]
		public void ProfilesEqual_SameMappingsSameOrder_Equal()
		{
			var p1 = new vJoySerialFeeder.Configuration.Profile();
			p1.Mappings.Add(new AxisMapping { Channel = 0 });
			p1.Mappings.Add(new ButtonMapping { Channel = 1 });

			var p2 = new vJoySerialFeeder.Configuration.Profile();
			p2.Mappings.Add(new AxisMapping { Channel = 0 });
			p2.Mappings.Add(new ButtonMapping { Channel = 1 });

			Assert.That(vJoySerialFeeder.Configuration.ProfilesEqual(p1, p2), Is.True);
		}

		// ── Version field survives serialization ────────────────

		[Test]
		public void ToJSONString_RoundTrips_VersionField()
		{
			var config = new vJoySerialFeeder.Configuration();
			config.Version = "1.6.0.0";

			var json = config.ToJSONString();
			var loaded = vJoySerialFeeder.Configuration.LoadFromJSONString(json);

			Assert.That(loaded.Version, Is.EqualTo("1.6.0.0"));
		}

		[Test]
		public void ToJSONString_RoundTrips_NullVersion()
		{
			var config = new vJoySerialFeeder.Configuration();
			config.Version = null;

			var json = config.ToJSONString();
			var loaded = vJoySerialFeeder.Configuration.LoadFromJSONString(json);

			Assert.That(loaded.Version, Is.Null);
		}
	}
}
