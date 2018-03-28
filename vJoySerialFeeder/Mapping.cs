/*
 * Created by SharpDevelop.
 * User: Cleric
 * Date: 8.6.2017 г.
 * Time: 18:22 ч.
 */
using System;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace vJoySerialFeeder
{
	/// <summary>
	/// This is the base class for the mappings.
	/// A class mapping consists of UI element and logic which transforms
	/// the raw channel data to the desired joystick command.
	/// </summary>
	
	[DataContract]
	public abstract class Mapping
	{
		/// <summary>
		/// Helper property to get the Channel value
		/// </summary>
		private int _input;
		public int Input { 
			get { return _input; }
			set {
				_input = value;
				Output = Transform(value);
			}
		}
		
		/// <summary>
		/// This is the transformed by the mapping value.
		/// The mapping is responsible for setting it.
		/// </summary>
		private float _output;
		public float Output {
			get { return _output; }
			set {
				_output = Clamp(value);
			}
		}
		
		/// <summary>
		/// Every mapping has a single channel to get data from (although this is not forced)
		/// </summary>
		[DataMember]
		public int Channel;
		
		/// <summary>
		/// Tells if this mapping is no longer in the interface
		/// </summary>
		public bool Removed { get; private set; }
		
		/// <summary>
		/// If the mapping wants to remove itself it MUST call this method.
		/// </summary>
		internal void Remove() {
			MainForm.Instance.RemoveMapping(this);
			Removed = true;
		}
		
		/// <summary>
		/// Should restrict the output value to the meaningful ranges for
		/// the mapping type. It is used by Scripts and Interaction.
		/// </summary>
		/// <param name="val"></param>
		/// <returns></returns>
		abstract protected float Clamp(float val);
		
		/// <summary>
		/// This method should do the actual work of transforming the integer channel
		/// value to the output value that is meaningful for the mapping type.
		/// </summary>
		/// <param name="val"></param>
		/// <returns></returns>
		abstract protected float Transform(int val);
		
		/// <summary>
		/// This method should return a Control element which will be placed in the MainFrame
		/// It should return the same Control on every call.
		/// </summary>
		/// <returns></returns>
		abstract public Control GetControl();
		
		/// <summary>
		/// This method gets called when the mapping should write its joystick.
		/// </summary>
		abstract public void UpdateJoystick(VJoyBase vjoy);
		
		/// <summary>
		/// Request painting of the UI element.
		/// </summary>
		abstract public void Paint();
		
		/// <summary>
		/// To support saving and loading profiles, every mapping should be able to make a copy of itself
		/// which includes only the data that should be persisted in the profile.
		/// </summary>
		/// <returns>Should return a new object of the same derived type with the same value
		/// for the persistable fields</returns>
		abstract public Mapping Copy();
	}
}
