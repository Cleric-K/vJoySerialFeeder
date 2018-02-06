/*
 * Created by SharpDevelop.
 * User: Cleric
 * Date: 8.6.2017 г.
 * Time: 16:58 ч.
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.IO.Ports;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;

namespace vJoySerialFeeder
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		public static MainForm instance;
		
		public int[] Channels { get { return channels; } }
		public int ActiveChannels { get { return activeChannels; } }
		public VJoy VJoy { get { return vJoy; } }
		
		private int[] channels = new int[255];
		private int activeChannels;
		
		public event EventHandler OnChannelData;
		
		private List<Mapping> mappings = new List<Mapping>();
		private bool connected = false;
		private SerialPort serialPort;
		
		private VJoy vJoy = new VJoy();
		private Profiles profiles = new Profiles();
		
		private double updateRate;
		
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			instance = this;
			
			if(!VJoy.Init())
				Application.Exit();
			
			reloadComPorts();
			reloadJoysticks();
			OnChannelData += onChannelData;
			
			comboProfiles.Items.AddRange(profiles.GetProfileNames());
			var defaultProfile = profiles.GetDefaultProfile();
			if(!String.IsNullOrEmpty(defaultProfile)) {
				comboProfiles.Text = defaultProfile;
				ButtonLoadProfileClick(null, null);
			}
			
			toolStripStatusLabel.Text = "Disconnected";
		}
		
		/// <summary>
		/// Called from the Mapping class when the mapping should remove
		/// itself from the MainForm
		/// </summary>
		/// <param name="m"></param>
		public void RemoveMapping(Mapping m) {
			panelMappings.Controls.Remove(m.GetControl());
			mappings.Remove(m);
		}
		
		
		
		
		
		
		private void saveProfile(XmlElement e) {
			using(XmlWriter xw = e.CreateNavigator().AppendChild()) {
				xw.WriteElementString("com", comboPorts.Text);
				xw.WriteElementString("baud", textBaud.Text);
				xw.WriteElementString("vjoy-id", comboJoysticks.Text);
			}
			foreach(Mapping m in mappings) {
				if(m is AxisMapping) {
					var x = e.OwnerDocument.CreateElement("axis-mapping");
					m.SaveToXmlElement(x);
					e.AppendChild(x);
				}
			}
		}
			
		private void loadProfile(XmlElement e) {
			while(mappings.Count > 0)
				mappings[0].Remove();
			//panelMappings.Controls.Clear();
			//mappings.Clear();
			XPathNavigator x = e.CreateNavigator();
				
			x.MoveToFirstChild();
			do {
				switch(x.Name) {
					case "com":
						comboPorts.SelectedItem = x.Value;
						break;
					case "baud":
						textBaud.Text = x.Value;
						break;
					case "vjoy-id":
						comboJoysticks.SelectedItem = x.Value;
						break;
					case "axis-mapping":
						addAxis().ReadFromXmlElement((XmlElement)x.UnderlyingObject);
						break;
				}
			} while(x.MoveToNext());
				
			
		}
		
		private void reloadProfiles() {
			comboProfiles.Items.Clear();
			comboProfiles.Items.AddRange(profiles.GetProfileNames());
		}
		
		private void reloadComPorts() {
			object prevPort = comboPorts.SelectedItem;
			comboPorts.Items.Clear();
			comboPorts.Items.AddRange(SerialPort.GetPortNames());
			comboPorts.SelectedItem = prevPort;
			if(comboPorts.SelectedItem == null)
				comboPorts.SelectedIndex = 0;
		}
		
		private void reloadJoysticks() {
			object prevJoy = comboJoysticks.SelectedItem;
			comboJoysticks.Items.Clear();
			comboJoysticks.Items.AddRange(VJoy.GetJoysticks());
			comboJoysticks.SelectedItem = prevJoy;
			if(comboJoysticks.SelectedItem == null)
				comboJoysticks.SelectedIndex = 0;
		}
		
		private void connect() {
			int baudRate;
			string errmsg;
			if((errmsg = VJoy.Acquire(uint.Parse(comboJoysticks.SelectedItem.ToString()))) != null) {
				MessageBox.Show(errmsg);
				return;
			}
			try {
				baudRate = (int)UInt32.Parse(textBaud.Text);
			}
			catch(Exception) {
				MessageBox.Show("Invalid baud rate", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			
			try {
				serialPort = new SerialPort((string)comboPorts.SelectedItem, baudRate);
				serialPort.Open();
			}
			catch(Exception) {
				MessageBox.Show("Can not open the port", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				VJoy.Release();
				return;
			}
			
			backgroundWorker.RunWorkerAsync();
			
			comboPorts.Enabled = false;
			textBaud.Enabled = false;
			buttonPortsRefresh.Enabled = false;
			buttonLoadProfile.Enabled = false;
			buttonConnect.Text = "Disconnect";
			comboJoysticks.Enabled = false;
			connected = true;
		}
		
		private void disconnect() {
			// when the background worker finished disconnect2 is called
			buttonConnect.Text = "Disconnecting";
			VJoy.Release();
			backgroundWorker.CancelAsync();
		}
		
		private void disconnect2() {
			serialPort.Close();
			serialPort = null;
			
			comboPorts.Enabled = true;
			textBaud.Enabled = true;
			buttonPortsRefresh.Enabled = true;
			buttonConnect.Text = "Connect";
			buttonLoadProfile.Enabled = true;
			connected = false;
			toolStripStatusLabel.Text = "Disconnected";
			comboJoysticks.Enabled = true;
		}
		
		void onChannelData(object sender, EventArgs e) {
			if(!ContainsFocus) return;
			foreach(var mapping in mappings) {
				mapping.Paint();
			}
			toolStripStatusLabel.Text = "Connected, "+activeChannels
				+" channels available, Update Rate "+Math.Round(updateRate)+" Hz";
		}
		
		Mapping addAxis() {
			var ax = new AxisMapping();
			mappings.Add(ax);
			panelMappings.Controls.Add(ax.GetControl());
			return ax;
		}
		
		
	
		void ButtonAddAxisClick(object sender, EventArgs e)
		{
			addAxis();
		}
		
		
		void FlowLayoutPanel1MouseEnter(object sender, EventArgs e)
		{
			// trick to make mouse wheel scroll possible without
			// explicitly focusing on the panel
			if(ContainsFocus)
				panelMappings.Focus();
		}
		void ButtonPortsRefreshClick(object sender, EventArgs e)
		{
			reloadComPorts();
		}
		
		void ButtonConnectClick(object sender, EventArgs e)
		{
			if(!connected)
				connect();
			else
				disconnect();
		}
		
		void BackgroundWorkerDoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
		{
			SerialReader sr = new SerialReader(serialPort);
			double nextUpdateTime = 0, prevTime = 0;
			double updateSum = 0;
			int updateCount = 0;
			
			while(true) {
				
				
				if(backgroundWorker.CancellationPending) {
					e.Cancel = true;
					return;
				}
				
				try {
					activeChannels = sr.ReadChannels();
				}
				catch(Exception ex) {
					System.Diagnostics.Debug.WriteLine(ex);
				}
				if(activeChannels > 0) {
					foreach(Mapping m in mappings) {
						m.WriteChannel();
					}
				}
				
				double now = (double)DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
				// update UI on every 100ms
				if(now > prevTime) {
					updateSum += 1000.0/(now - prevTime);
					updateCount++;
				}
				
				if(now >= nextUpdateTime) {
					nextUpdateTime = now + 100;
					if(activeChannels == 0)
						updateRate = 0;
					else if(updateCount > 0) {
						updateRate = updateSum/updateCount;
						updateSum = updateCount = 0;
					}
					backgroundWorker.ReportProgress(0);
				}
				prevTime = now;
			}
		}
		void BackgroundWorkerRunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
		{
			disconnect2();
		}
		
		void BackgroundWorkerProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
		{
			OnChannelData(this, e);
		}
		
		void ButtonSaveProfileClick(object sender, EventArgs e)
		{
			string name = comboProfiles.Text.Trim();
			if(name.Length == 0)
				MessageBox.Show("Enter a profile name");
			saveProfile(profiles.CreateProfileElement(name));
			profiles.Save();
			reloadProfiles();
		}
		void ButtonLoadProfileClick(object sender, EventArgs ea)
		{
			string name = comboProfiles.Text.Trim();
			if(name.Length == 0) {
				MessageBox.Show("Enter a profile name");
				return;
			}
			var e = profiles.GetProfileElement(name);
			if(e == null) {
				MessageBox.Show("No such profile");
				return;
			}
			loadProfile(e);
		}
		void ButtonDeleteProfileClick(object sender, EventArgs e)
		{
			string name = comboProfiles.Text.Trim();
			if(name.Length == 0) {
				MessageBox.Show("Enter a profile name");
				return;
			}
			profiles.DeleteProfile(name);
			reloadProfiles();
		}
		void MainFormFormClosed(object sender, FormClosedEventArgs e)
		{
			profiles.SetDefaultProfile(comboProfiles.Text);
			profiles.Save();
		}

	}
}
