/*
 * Created by SharpDevelop.
 * User: Cleric
 * Date: 7.2.2018 г.
 * Time: 21:21 ч.
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Windows.Forms;

namespace vJoySerialFeeder
{
	/// <summary>
	/// Persists the Profiles in the Settings storage.
	/// The whole Configuration is serialized and stored in the 'config' Settings field.
	/// </summary>
	[DataContract]
	public class Configuration
	{
		public const int DEFAULT_WEBSOCKET_PORT = 40000;
		/// <summary>
		/// Stores a single profile
		/// </summary>
		[DataContract]
		public class Profile {
			public const int DEFAULT_FAILSAFE_TIME = 500;
			public const int DEFAULT_FAILSAFE_UPDATE_RATE = 100;
			
			[DataMember]
			public string COMPort, VJoyInstance;
			[DataMember]
			public int Protocol;
			[DataMember]
			public List<Mapping> Mappings = new List<Mapping>();
			[DataMember]
			public bool UseCustomSerialParameters;
			[DataMember]
			public SerialParameters SerialParameters;
			[DataMember]
			public string ProtocolConfiguration = "";
			[DataMember]
			public string LuaScript;
			[DataMember]
			public int FailsafeUpdateRate = DEFAULT_FAILSAFE_UPDATE_RATE;
			[DataMember]
			public int FailsafeTime = DEFAULT_FAILSAFE_TIME;
		}
		
		/// <summary>
		/// Stores Serial Port Config
		/// </summary>
		[DataContract]
		public struct SerialParameters
		{
			[DataMember]
			public int BaudRate, DataBits;
			[DataMember]
			public Parity Parity;
			[DataMember]
			public StopBits StopBits;
		}
		
		/// <summary>
		/// Stores the name of the profile to load on startup
		/// </summary>
		[DataMember]
		public string DefaultProfile = "";
		
		/// <summary>
		/// Profiles are stored as map name => profile
		/// </summary>
		[DataMember]
		public Dictionary<string, Profile> Profiles = new Dictionary<string, Profile>();
		
		[DataMember]
		public bool WebSocketEnabled;
		
		[DataMember]
		public int WebSocketPort = DEFAULT_WEBSOCKET_PORT;
		
		[DataMember]
		public string Version = RuntimeVersion.ToString();
		
		[DataMember]
		public bool Autoconnect;
	
		private static DataContractJsonSerializer ConfigSerializer = new DataContractJsonSerializer(
				typeof(Configuration),
				new Type[] {typeof(AxisMapping), typeof(ButtonMapping), typeof(ButtonBitmapMapping)}
			);
		
		private static DataContractJsonSerializer ProfileSerializer = new DataContractJsonSerializer(
				typeof(Profile),
				new Type[] {typeof(AxisMapping), typeof(ButtonMapping), typeof(ButtonBitmapMapping)}
			);
		
		
		
		public static Version RuntimeVersion { get { return Assembly.GetEntryAssembly().GetName().Version; } }
		
		
		public static Configuration Load() {
            var json = Settings.Default.config;

            if(json.Trim().Equals("")) {
				Settings.Default.Upgrade();
                Settings.Default.Save();
                
                json = Settings.Default.config;
			}
			
			if(!json.Trim().Equals("")) {
				try {
            		var c = LoadFromJSONString(json);

            		if(c.Upgrade()) {
            			c.Save();
            		}
            		
					return c;
				}
				catch(SerializationException ) {
					MessageBox.Show("Could not load configuration");
				}
			}
			
			return new Configuration();
		}

		
		/// <summary>
		/// Loads configuration fron JSON string.
		/// Might throw SerializationException
		/// </summary>
		/// <param name="cfg"></param>
		/// <returns></returns>
		public static Configuration LoadFromJSONString(string cfg) {
			var ms = new MemoryStream(Encoding.UTF8.GetBytes(cfg));
			ms.Position = 0;
			return (Configuration)ConfigSerializer.ReadObject(ms);
		}
		
		/// <summary>
		/// Upgrade configuration to current version
		/// </summary>
		/// <returns>true if upgrade was performed</returns>
		public bool Upgrade() {
			var currentVersion = RuntimeVersion;
			var prevVersion = new Version(0,0,0,0);
			
			try {
				prevVersion = new Version(this.Version);
			}
			catch(Exception){}
			
            if(prevVersion < currentVersion) {
                DoUpgrade(prevVersion, currentVersion);
                Version = currentVersion.ToString();
				return true;
			}
			
			return false;
		}
		
		public string ToJSONString() {
			return serialize(this, ConfigSerializer);
		}
		
		public void Save() {
			Settings.Default.config = ToJSONString();
			Settings.Default.Save();
		}
		
		public string[] GetProfileNames() {
			return Profiles.Keys.ToArray();
		}
		
		public void PutProfile(string name, Profile p) {
			Profiles[name] = p;
		}
		
		public Profile GetProfile(string name) {
			if(!Profiles.ContainsKey(name))
				return null;
			return Profiles[name];
		}
		
		public void DeleteProfile(string name) {
			if(Profiles.ContainsKey(name))
				Profiles.Remove(name);
		}
		
		public static bool ProfilesEqual(Profile p1, Profile p2) {
			if(p1 == null && p2 == null)
				return true;
			if(p1 == null || p2 == null)
				return false;
			return serialize(p1, ProfileSerializer).Equals(serialize(p2, ProfileSerializer));
		}
		
		/// <summary>
		/// Merges the provided configuration to this.
		/// </summary>
		/// <param name="cfg">Configuration to merge</param>
		/// <param name="importGlobalOptions"></param>
		/// <param name="importProfiles">If Profiles are imported they will not overwrite
		/// profiles with the same name in this configuration. Instead they will be renamed
		/// with _N suffix.</param>
		public void Merge(Configuration cfg, bool importGlobalOptions, bool importProfiles) {
			if(importGlobalOptions) {
				WebSocketEnabled = cfg.WebSocketEnabled;
				WebSocketPort = cfg.WebSocketPort;
			}
			
			if(importProfiles) {
				foreach(var k in cfg.Profiles.Keys) {
					var finalk = k;
					var i = 1;
					
					while(Profiles.ContainsKey(finalk))
						finalk = k + "_" + i++;
					
					Profiles[finalk] = cfg.Profiles[k];
				}
			}
		}
		
		static string serialize(object obj, DataContractJsonSerializer ser) {
			var ms = new MemoryStream();
			ser.WriteObject(ms, obj);
			ms.Position = 0;
			return new StreamReader(ms).ReadToEnd();
		}
		
		/// <summary>
		/// Actual version specific upgrade code must be implemented here
		/// </summary>
		/// <param name="fromVersion"></param>
		/// <param name="toVersion"></param>
		void DoUpgrade(Version fromVersion, Version toVersion) {
			
			
			if(fromVersion <= new Version("1.2.0.0")) {
				WebSocketPort = DEFAULT_WEBSOCKET_PORT;
				
				foreach(var p in Profiles.Values) {
					p.FailsafeTime = Profile.DEFAULT_FAILSAFE_TIME;
					p.FailsafeUpdateRate = Profile.DEFAULT_FAILSAFE_UPDATE_RATE;
					
					foreach(var m in p.Mappings) {
						if(m is AxisMapping)
							((AxisMapping)m).Parameters.Failsafe = AxisMapping.AxisParameters.DEFAULT_FAILSAFE;
					}
				}
			}
			
			
			
		}
	}
}
