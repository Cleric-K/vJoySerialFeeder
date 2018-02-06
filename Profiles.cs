/*
 * Created by SharpDevelop.
 * User: Cleric
 * Date: 20.6.2017 г.
 * Time: 19:55 ч.
 */
using System;
using System.Collections;
using System.Windows.Forms;
using System.Xml;

namespace vJoySerialFeeder
{
	/// <summary>
	/// Description of Profiles.
	/// </summary>
	public class Profiles
	{
		private XmlDocument doc;
		
		public Profiles()
		{
			doc = new XmlDocument();
			if(Settings.Default.profiles.Equals(""))
				Settings.Default.Upgrade();
			var xml = Settings.Default.profiles;
			try {
				doc.LoadXml(xml);
			}
			catch(XmlException) {
				doc.AppendChild(doc.CreateElement("profiles"));
			}
		}
		
		public object[] GetProfileNames() {
			ArrayList list = new ArrayList();
			foreach(XmlElement p in doc.GetElementsByTagName("profile")) {
				list.Add(p.GetAttribute("name"));
			}
			list.Sort();
			return list.ToArray();
		}
		
		public XmlElement GetProfileElement(string name) {
			foreach(XmlElement p in doc.GetElementsByTagName("profile")) {
				if(p.GetAttribute("name").Equals(name))
					return (XmlElement)p;
			}
			return null;
		}
		
		public XmlElement CreateProfileElement(string name) {
			XmlElement p = GetProfileElement(name);
			if(p == null) {
				p = doc.CreateElement("profile");
				p.SetAttribute("name", name);
				doc.DocumentElement.AppendChild(p);
			}
			else
				p.IsEmpty = true;
			
			return p;
		}
		
		public void DeleteProfile(string name) {
			XmlElement p = GetProfileElement(name);
			if(p != null)
				p.ParentNode.RemoveChild(p);
		}

		public void SetDefaultProfile(string name)
		{
			if(GetProfileElement(name) == null)
				doc.DocumentElement.RemoveAttribute("default-profile");
			else
				doc.DocumentElement.SetAttribute("default-profile", name);
		}
		
		public string GetDefaultProfile()
		{
			return doc.DocumentElement.GetAttribute("default-profile");
		}

		public void Save()
		{
			Settings.Default.profiles = doc.OuterXml;
			Settings.Default.Save();
		}
	}
}
