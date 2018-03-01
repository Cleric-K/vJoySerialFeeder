/*
 * Created by SharpDevelop.
 * User: Cleric
 * Date: 8.6.2017 г.
 * Time: 16:58 ч.
 */
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Windows.Forms;

namespace vJoySerialFeeder
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		public static MainForm Instance {get; private set; }
		
		public int[] Channels { get; private set; }
		public int ActiveChannels { get ; private set; }
		public VJoy VJoy { get; private set;}
		
		public event EventHandler ChannelDataUpdate;
		
		private List<Mapping> mappings = new List<Mapping>();
		private bool connected = false;
		private SerialPort serialPort;
		private SerialReader serialReader;
		private string protocolConfig = "";
		
		private Configuration config;
		private bool useCustomSerialParameters = true;
		private Configuration.SerialParameters serialParameters;
		
		private double updateRate;
		
		private Type[] Protocols = {typeof(IbusReader), typeof(MultiWiiReader)};
		
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			Instance = this;
			 
			Channels = new int[256];
			VJoy = new VJoy();
			
			if(!VJoy.Init())
				Application.Exit();
			
			comboProtocol.SelectedIndex = 0;
			reloadComPorts();
			reloadJoysticks();
			ChannelDataUpdate += onChannelDataUpdate;

			config = Configuration.Load();
			
			reloadProfiles();
			
			var defaultProfile = config.GetProfile(config.DefaultProfile);
			if(defaultProfile == null && comboProfiles.Items.Count > 0) {
				var first = comboProfiles.Items[0].ToString();
				defaultProfile = config.GetProfile(first);
				comboProfiles.Text = first;
			}
			if(defaultProfile != null) {
				comboProfiles.Text = config.DefaultProfile;
				loadProfile(defaultProfile);
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
		
		
		
		
		
		
			
		private void loadProfile(Configuration.Profile p) {
			while(mappings.Count > 0)
				mappings[0].Remove();
			
			if(!connected) {
				// load this stuff only if not connected
				comboProtocol.SelectedIndex = p.Protocol;
				comboPorts.SelectedItem = p.COMPort;
				useCustomSerialParameters = p.UseCustomSerialParameters;
				serialParameters = p.SerialParameters;
				protocolConfig = p.ProtocolConfiguration;
				comboJoysticks.SelectedItem = p.VJoyInstance;
			}
			
			foreach(var m in p.Mappings) {
				addMapping(m.Copy());
			}
		}
		
		private void reloadProfiles() {
			var ps = config.GetProfileNames();
			Array.Sort(ps);
			comboProfiles.Items.Clear();
			comboProfiles.Items.AddRange(ps);
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
			if(comboJoysticks.SelectedItem == null && comboJoysticks.Items.Count > 0)
				comboJoysticks.SelectedIndex = 0;
		}
		
		private SerialReader createSerialReader() {
			return (SerialReader)Activator.CreateInstance(Protocols[comboProtocol.SelectedIndex]);
		}
		
		private void connect() {
			string errmsg;
			if(comboJoysticks.SelectedItem != null && (errmsg = VJoy.Acquire(uint.Parse(comboJoysticks.SelectedItem.ToString()))) != null) {
				MessageBox.Show(errmsg);
				return;
			}

			serialReader = createSerialReader();
			
			var sp = useCustomSerialParameters ?
				serialParameters
				: serialReader.GetDefaultSerialParameters();
			
			try {
				serialPort = new SerialPort((string)comboPorts.SelectedItem, sp.BaudRate, sp.Parity, sp.DataBits, sp.StopBits);
				serialPort.Open();
			}
			catch(Exception) {
				MessageBox.Show("Can not open the port", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				VJoy.Release();
				return;
			}

			comboProtocol.Enabled = false;
			comboPorts.Enabled = false;
			buttonPortSetup.Enabled = false;
			buttonPortsRefresh.Enabled = false;
			buttonProtocolSetup.Enabled = false;
			buttonConnect.Text = "Disconnect";
			comboJoysticks.Enabled = false;
			connected = true;
			
			backgroundWorker.RunWorkerAsync();
		}
		
		private void disconnect() {
			// when the background worker finished disconnect2 is called
			buttonConnect.Text = "Disconnecting";
			backgroundWorker.CancelAsync();
		}
		
		private void disconnect2() {
			VJoy.Release();
			try {
				serialPort.Close();
			} catch(Exception) {}
			serialPort = null;
			
			ActiveChannels = 0;
			comboProtocol.Enabled = true;
			comboPorts.Enabled = true;
			buttonPortSetup.Enabled = true;
			buttonPortsRefresh.Enabled = true;
			buttonProtocolSetup.Enabled = true;
			buttonConnect.Text = "Connect";
			connected = false;
			toolStripStatusLabel.Text = "Disconnected";
			comboJoysticks.Enabled = true;
		}
		
		void onChannelDataUpdate(object sender, EventArgs e) {
			if(!ContainsFocus) return;
			foreach(var mapping in mappings) {
				mapping.Paint();
			}
			toolStripStatusLabel.Text = "Connected, "+ActiveChannels
				+" channels available, Update Rate "+Math.Round(updateRate)+" Hz"
				+" (" + (updateRate < 0.001 ? "∞" : Math.Round(1000/updateRate).ToString()) + " ms)";
		}
		
		void addMapping(Mapping m) {
			mappings.Add(m);
			panelMappings.Controls.Add(m.GetControl());
		}
	
		void ButtonAddAxisClick(object sender, EventArgs e)
		{
			addMapping(new AxisMapping());
		}

		void ButtonAddButtonClick(object sender, EventArgs e)
        {
			addMapping(new ButtonMapping());
        }
		
		void ButtonBitmappedButtonClick(object sender, EventArgs e)
        {
			addMapping(new ButtonBitmapMapping());
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
			serialReader.Init(serialPort, Channels, protocolConfig);
			serialReader.Start();
			
			double nextUpdateTime = 0, prevTime = 0;
			double updateSum = 0;
			int updateCount = 0;
			
			while(true) {
				
				
				if(backgroundWorker.CancellationPending) {
					e.Cancel = true;
					serialReader.Stop();
					return;
				}
				
				try {
					ActiveChannels = serialReader.ReadChannels();
				}
				catch(Exception ex) {
					ActiveChannels = 0;
					System.Diagnostics.Debug.WriteLine(ex.Message);
				}
				if(ActiveChannels > 0) {
					foreach(Mapping m in mappings) {
						m.UpdateJoystick(VJoy);
					}
				}
				VJoy.SetState();
				
				double now = (double)DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
				// update UI on every 100ms
				if(now > prevTime && ActiveChannels > 0) {
					updateSum += 1000.0/(now - prevTime);
					updateCount++;
				}
				
				if(now >= nextUpdateTime) {
					nextUpdateTime = now + 100;
					if(ActiveChannels == 0)
						updateRate = 0;
					else if(updateCount > 0) {
						updateRate = updateSum/updateCount;
						updateSum = updateCount = 0;
					}
					backgroundWorker.ReportProgress(0);
				}
				
				if(ActiveChannels > 0)
					prevTime = now;
			}
		}
		void BackgroundWorkerRunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
		{
			disconnect2();
		}
		
		void BackgroundWorkerProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
		{
			ChannelDataUpdate(this, e);
		}
		
		void ButtonSaveProfileClick(object sender, EventArgs e)
		{
			string name = comboProfiles.Text.Trim();
			if(name.Length == 0) {
				MessageBox.Show("Enter a profile name");
				return;
			}
			
			var p = new Configuration.Profile();
				
			p.Protocol = comboProtocol.SelectedIndex;
			p.COMPort = comboPorts.Text;
			p.UseCustomSerialParameters = useCustomSerialParameters;
			p.SerialParameters = serialParameters;
			p.ProtocolConfiguration = protocolConfig;
			p.VJoyInstance = comboJoysticks.Text;

			p.Mappings = new List<Mapping>();
			
			foreach (var m in mappings)
				p.Mappings.Add(m.Copy());
			
			config.PutProfile(name, p);
			config.DefaultProfile = name;
			config.Save();
			
			reloadProfiles();
		}
		
		void ButtonLoadProfileClick(object sender, EventArgs ea)
		{
			string name = comboProfiles.Text.Trim();
			if(name.Length == 0) {
				MessageBox.Show("Enter a profile name");
				return;
			}
			var p = config.GetProfile(name);
			if(p == null) {
				MessageBox.Show("No such profile");
				return;
			}
			
			loadProfile(p);
			
			config.DefaultProfile = name;
			config.Save();
		}
		
		void ButtonDeleteProfileClick(object sender, EventArgs e)
		{
			string name = comboProfiles.Text.Trim();
			if(name.Length == 0) {
				MessageBox.Show("Enter a profile name");
				return;
			}
			config.DeleteProfile(name);
			config.Save();
			reloadProfiles();
		}
        
        void ButtonMonitorClick(object sender, EventArgs e)
        {
        	var f = new MonitorForm();
        	f.Owner = this;
        	f.Show();
        } 
        
        void ButtonPortSetupClick(object sender, EventArgs e)
        {
        	var sp = useCustomSerialParameters ?
        		serialParameters
        		: createSerialReader().GetDefaultSerialParameters();
        	var d = new PortSetupForm(useCustomSerialParameters, sp);
        	d.ShowDialog();
        	if(d.DialogResult == DialogResult.OK) {
        		useCustomSerialParameters = d.UseCustomSerialParameters;
        		serialParameters = d.SerialParameters;
        	}
        }
        
        void ButtonProtocolSetupClick(object sender, EventArgs e)
        {
        	var c = createSerialReader().Configure(protocolConfig);
        	if(c != null)
        		protocolConfig = c;
        }
        
        void ComboProtocolSelectedIndexChanged(object sender, EventArgs e)
        {
        	buttonProtocolSetup.Visible = createSerialReader().IsConfigurable();
        	protocolConfig = "";
        }
	}
}
