using NUnit.Framework;
using vJoySerialFeeder;

namespace vJoySerialFeeder.Tests.Mappings
{
	[TestFixture]
	public class TriggerStateTests
	{
		private TestTimeProvider time;
		private TriggerState trigger;

		[SetUp]
		public void SetUp()
		{
			time = new TestTimeProvider { Now = 1000 };
			trigger = new TriggerState(time);
		}

		// ── First call behavior ─────────────────────────────────

		[Test]
		public void Trigger_FirstCallTrue_ReturnsTrue()
		{
			Assert.That(trigger.Trigger(true, TriggerState.Edge.Rising, 200), Is.True);
		}

		[Test]
		public void Trigger_FirstCallFalse_ReturnsFalse()
		{
			Assert.That(trigger.Trigger(false, TriggerState.Edge.Rising, 200), Is.False);
		}

		// ── Rising edge ─────────────────────────────────────────

		[Test]
		public void Trigger_RisingEdge_FalseToTrue_FiresPulse()
		{
			trigger.Trigger(false, TriggerState.Edge.Rising, 200);

			var result = trigger.Trigger(true, TriggerState.Edge.Rising, 200);

			Assert.That(result, Is.True);
		}

		[Test]
		public void Trigger_RisingEdge_TrueToFalse_DoesNotFire()
		{
			trigger.Trigger(true, TriggerState.Edge.Rising, 200);

			var result = trigger.Trigger(false, TriggerState.Edge.Rising, 200);

			Assert.That(result, Is.False);
		}

		// ── Falling edge ────────────────────────────────────────

		[Test]
		public void Trigger_FallingEdge_TrueToFalse_FiresPulse()
		{
			trigger.Trigger(true, TriggerState.Edge.Falling, 200);

			var result = trigger.Trigger(false, TriggerState.Edge.Falling, 200);

			Assert.That(result, Is.True);
		}

		[Test]
		public void Trigger_FallingEdge_FalseToTrue_DoesNotFire()
		{
			trigger.Trigger(false, TriggerState.Edge.Falling, 200);

			var result = trigger.Trigger(true, TriggerState.Edge.Falling, 200);

			Assert.That(result, Is.False);
		}

		// ── Both edges ──────────────────────────────────────────

		[Test]
		public void Trigger_BothEdge_FalseToTrue_FiresPulse()
		{
			trigger.Trigger(false, TriggerState.Edge.Both, 200);

			var result = trigger.Trigger(true, TriggerState.Edge.Both, 200);

			Assert.That(result, Is.True);
		}

		[Test]
		public void Trigger_BothEdge_TrueToFalse_FiresPulse()
		{
			trigger.Trigger(true, TriggerState.Edge.Both, 200);

			var result = trigger.Trigger(false, TriggerState.Edge.Both, 200);

			Assert.That(result, Is.True);
		}

		// ── No edge on same level ───────────────────────────────

		[Test]
		public void Trigger_SameLevel_TrueTrue_DoesNotFire()
		{
			trigger.Trigger(true, TriggerState.Edge.Both, 200);

			var result = trigger.Trigger(true, TriggerState.Edge.Both, 200);

			Assert.That(result, Is.False);
		}

		[Test]
		public void Trigger_SameLevel_FalseFalse_DoesNotFire()
		{
			trigger.Trigger(false, TriggerState.Edge.Both, 200);

			var result = trigger.Trigger(false, TriggerState.Edge.Both, 200);

			Assert.That(result, Is.False);
		}

		// ── Pulse duration ──────────────────────────────────────

		[Test]
		public void Trigger_PulseStaysActive_BeforeDurationExpires()
		{
			trigger.Trigger(false, TriggerState.Edge.Rising, 200);
			trigger.Trigger(true, TriggerState.Edge.Rising, 200); // fires at t=1000

			time.Now = 1100; // 100ms later, still within 200ms duration
			var result = trigger.Trigger(true, TriggerState.Edge.Rising, 200);

			Assert.That(result, Is.True);
		}

		[Test]
		public void Trigger_PulseReleasesAfterDurationExpires()
		{
			trigger.Trigger(false, TriggerState.Edge.Rising, 200);
			trigger.Trigger(true, TriggerState.Edge.Rising, 200); // fires at t=1000

			time.Now = 1300; // 300ms later, past 200ms duration
			var result = trigger.Trigger(true, TriggerState.Edge.Rising, 200);

			Assert.That(result, Is.False);
		}

		[Test]
		public void Trigger_PulseReleasesExactlyAtDuration()
		{
			trigger.Trigger(false, TriggerState.Edge.Rising, 200);
			trigger.Trigger(true, TriggerState.Edge.Rising, 200); // fires at t=1000, release at 1200

			time.Now = 1200; // exactly at expiry: releaseTriggerAt (1200) > now (1200) is false
			var result = trigger.Trigger(true, TriggerState.Edge.Rising, 200);

			Assert.That(result, Is.False);
		}

		// ── Duration = 0 ────────────────────────────────────────

		[Test]
		public void Trigger_ZeroDuration_EdgeDoesNotProducePulse()
		{
			trigger.Trigger(false, TriggerState.Edge.Rising, 0);

			// releaseTriggerAt = now + 0 = now, so releaseTriggerAt > now is false
			var result = trigger.Trigger(true, TriggerState.Edge.Rising, 0);

			Assert.That(result, Is.False);
		}

		// ── Edge during active pulse ────────────────────────────

		[Test]
		public void Trigger_EdgeDuringActivePulse_ResetsTimer()
		{
			// Rising edge at t=1000
			trigger.Trigger(false, TriggerState.Edge.Both, 200);
			trigger.Trigger(true, TriggerState.Edge.Both, 200);

			// Falling edge at t=1100 (during active pulse), resets to 1300
			time.Now = 1100;
			trigger.Trigger(false, TriggerState.Edge.Both, 200);

			// At t=1250: original pulse would have expired (1200) but reset extended to 1300
			time.Now = 1250;
			var result = trigger.Trigger(false, TriggerState.Edge.Both, 200);

			Assert.That(result, Is.True);
		}

		// ── After pulse expires, new edge triggers again ────────

		[Test]
		public void Trigger_AfterPulseExpires_NewEdgeTriggersAgain()
		{
			trigger.Trigger(false, TriggerState.Edge.Rising, 200);
			trigger.Trigger(true, TriggerState.Edge.Rising, 200); // pulse at t=1000

			time.Now = 1300; // expired
			trigger.Trigger(true, TriggerState.Edge.Rising, 200); // no edge (same level)

			// Now create a new rising edge
			trigger.Trigger(false, TriggerState.Edge.Rising, 200);
			time.Now = 1400;
			var result = trigger.Trigger(true, TriggerState.Edge.Rising, 200);

			Assert.That(result, Is.True);
		}
	}
}
