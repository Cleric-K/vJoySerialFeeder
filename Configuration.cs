/*
 * Created by SharpDevelop.
 * User: Cleric
 * Date: 7.2.2018 г.
 * Time: 21:21 ч.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
			public string COMPort, BaudRate, VJoyInstance;
			
			[DataMember]
			public List<Mapping> Mappings = new List<Mapping>();
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
		
	
		private static DataContractJsonSerializer Serializer = new DataContractJsonSerializer(
				typeof(Configuration),
				new Type[] {typeof(AxisMapping), typeof(ButtonMapping)}
			);
		
		
		
		
		
		public static Configuration Load() {
			// "empty" is the default setting value
			if(Settings.Default.config.Equals("empty"))
				Settings.Default.Upgrade();
        	
			var json = Settings.Default.config;
			
			if(!json.Equals("empty")) {
				try {
					var ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
					ms.Position = 0;
					return (Configuration)Serializer.ReadObject(ms);
				}
				catch(SerializationException ) {
					MessageBox.Show("Could not load configuration");
				}
			}
			
			return new Configuration();
		}
		
		public void Save() {
			var ms = new MemoryStream();
			Serializer.WriteObject(ms, this);
			ms.Position = 0;
			var json = new StreamReader(ms).ReadToEnd();
			//System.Diagnostics.Debug.WriteLine(json);
			Settings.Default.config = json;
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
	}
}
