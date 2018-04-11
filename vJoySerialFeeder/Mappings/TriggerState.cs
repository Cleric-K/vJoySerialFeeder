/*
 * Created by SharpDevelop.
 * User: Cleric
 * Date: 4.4.2018 г.
 * Time: 10:47 ч.
 */
using System;

namespace vJoySerialFeeder
{
	/// <summary>
	/// Description of TriggerState.
	/// </summary>
	public class TriggerState
	{
		public enum Edge { Rising, Falling, Both }
		public const int DEFAULT_DURATION = 200;
		
		bool? prevLevel;
		double releaseTriggerAt;
		
		public bool Trigger(bool newLevel, Edge edge, int duration) {
			if(prevLevel == null) {
				prevLevel = newLevel;
				return newLevel;
			}
			
			bool level = false;
			var edgeEvent = prevLevel != newLevel
				&& (
					edge == Edge.Both
				   	|| edge == Edge.Rising && newLevel
				   	|| edge == Edge.Falling && !newLevel
				  );
			prevLevel = newLevel;
			
			if(edgeEvent || releaseTriggerAt != 0) {
				var now = MainForm.Now;
				
				if(edgeEvent) {
					// triggered
					releaseTriggerAt = now + duration;
				}
				
				if(releaseTriggerAt != 0) {
					// trigger is active
					
					if(releaseTriggerAt > now) {
						// trigger is still active
						level = true;
					}
					else {
						// time to release trigger
						releaseTriggerAt = 0;
					}
				}
			}
			
			return level;
		}
	}
}
