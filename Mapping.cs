/*
 * Created by SharpDevelop.
 * User: Cleric
 * Date: 8.6.2017 г.
 * Time: 18:22 ч.
 */
using System;
using System.Windows.Forms;
using System.Xml;

namespace vJoySerialFeeder
{
	/// <summary>
	/// Description of Mapping.
	/// </summary>
	public abstract class Mapping
	{
		public int ChannelValue { get { return MainForm.instance.Channels[channel]; } }
		
		protected int channel;
		
		public void Remove() {
			MainForm.instance.RemoveMapping(this);
		}
		
		abstract public Control GetControl();
		
		abstract public void WriteChannel();
		
		abstract public void Paint();
		
		abstract public void SaveToXmlElement(XmlElement e);

		abstract public void ReadFromXmlElement(XmlElement e);
	}
}
