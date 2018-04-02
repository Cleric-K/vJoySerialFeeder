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
		/// <summary>
		/// Stores a single profile
		/// </summary>
		[DataContract]
		public class Profile {
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
			public int FailsafeUpdateRate = 100;
			[DataMember]
			public int FailsafeTime = 500;
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
		public int WebSocketPort = 40000;
	
		private static DataContractJsonSerializer ConfigSerializer = new DataContractJsonSerializer(
				typeof(Configuration),
				new Type[] {typeof(AxisMapping), typeof(ButtonMapping), typeof(ButtonBitmapMapping)}
			);
		
		private static DataContractJsonSerializer ProfileSerializer = new DataContractJsonSerializer(
				typeof(Profile),
				new Type[] {typeof(AxisMapping), typeof(ButtonMapping), typeof(ButtonBitmapMapping)}
			);
		
		
		
		
		
		public static Configuration Load() {
            var currentVersion = Assembly.GetEntryAssembly().GetName().Version;
            var prevVersion = new Version(0,0,0,0);

            try
            {
                var cfgVer = Settings.Default.version.Trim();
                if (cfgVer == "")
                    cfgVer = (string)Settings.Default.GetPreviousVersion("version");
                prevVersion = new Version(cfgVer);
            }
            catch (Exception) {}

            if(prevVersion < currentVersion) {
				Settings.Default.Upgrade();
                Settings.Default.version = currentVersion.ToString();
                Settings.Default.Save();
			}
        	
			var json = Settings.Default.config;
			
			if(!json.Trim().Equals("")) {
				try {
					var ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
					ms.Position = 0;
					var c = (Configuration)ConfigSerializer.ReadObject(ms);
					
                    if(prevVersion < currentVersion) {
                        c.Upgrade(prevVersion, currentVersion);
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
		
		public void Save() {
			Settings.Default.config = serialize(this, ConfigSerializer);
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
		
		static string serialize(object obj, DataContractJsonSerializer ser) {
			var ms = new MemoryStream();
			ser.WriteObject(ms, obj);
			ms.Position = 0;
			return new StreamReader(ms).ReadToEnd();
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="fromVersion"></param>
		/// <param name="toVersion"></param>
		void Upgrade(Version fromVersion, Version toVersion) {
			
			
			if(fromVersion <= new Version("1.2.0.0")) {
				var templateConfiguration = new Configuration();
				var templateProfile = new Profile();
				var templateAxisMapping  = new AxisMapping();
				var templateButtonMapping  = new ButtonMapping();
				
				WebSocketPort = templateConfiguration.WebSocketPort;
				
				foreach(var p in Profiles.Values) {
					p.FailsafeTime = templateProfile.FailsafeTime;
					p.FailsafeUpdateRate = templateProfile.FailsafeUpdateRate;
					foreach(var m in p.Mappings) {
						if(m is AxisMapping)
							((AxisMapping)m).Parameters.Failsafe = templateAxisMapping.Parameters.Failsafe;
						else if(m is ButtonMapping)
							((ButtonMapping)m).Parameters.Failsafe = templateButtonMapping.Parameters.Failsafe;
					}
				}
			}
			
			
			
		}
	}
}
