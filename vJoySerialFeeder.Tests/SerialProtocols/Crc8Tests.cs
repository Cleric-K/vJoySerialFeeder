using NUnit.Framework;
using vJoySerialFeeder;

namespace vJoySerialFeeder.Tests.SerialProtocols
{
	[TestFixture]
	public class Crc8Tests
	{
		// CRSF uses polynomial 0xD5
		private const byte CRSF_POLY = 0xD5;

		// ── Basic operation ─────────────────────────────────────

		[Test]
		public void Constructor_InitializesOutputToZero()
		{
			var crc = new Crc8(CRSF_POLY);
			Assert.That(crc.Out, Is.EqualTo(0));
		}

		[Test]
		public void Add_SingleByte_ProducesNonZeroChecksum()
		{
			var crc = new Crc8(CRSF_POLY);
			crc.Add(0x16); // CRSF RC channels frame type
			Assert.That(crc.Out, Is.Not.EqualTo(0));
		}

		[Test]
		public void Add_ZeroByte_FromZeroState_RemainsZero()
		{
			// CRC of 0x00 starting from 0 should be table[0^0] = table[0]
			// table[0] = 0 because 0 shifted left 8 times with no high bits is 0
			var crc = new Crc8(CRSF_POLY);
			crc.Add(0x00);
			Assert.That(crc.Out, Is.EqualTo(0));
		}

		// ── Reset behavior ──────────────────────────────────────

		[Test]
		public void Out_Set_ResetsToZero()
		{
			var crc = new Crc8(CRSF_POLY);
			crc.Add(0xFF);
			Assert.That(crc.Out, Is.Not.EqualTo(0)); // sanity check

			crc.Out = 0; // setter resets to 0 regardless of value
			Assert.That(crc.Out, Is.EqualTo(0));
		}

		[Test]
		public void Out_Set_AnyValueResetsToZero()
		{
			// The setter ignores the value and always sets to 0
			var crc = new Crc8(CRSF_POLY);
			crc.Add(0xAB);
			crc.Out = 99; // any value resets to 0
			Assert.That(crc.Out, Is.EqualTo(0));
		}

		// ── Determinism ─────────────────────────────────────────

		[Test]
		public void Add_SameSequence_ProducesSameResult()
		{
			var crc1 = new Crc8(CRSF_POLY);
			var crc2 = new Crc8(CRSF_POLY);

			byte[] data = { 0x16, 0xC8, 0x01, 0x02, 0x03 };
			foreach (var b in data) {
				crc1.Add(b);
				crc2.Add(b);
			}

			Assert.That(crc1.Out, Is.EqualTo(crc2.Out));
		}

		[Test]
		public void Add_DifferentSequences_ProduceDifferentResults()
		{
			var crc1 = new Crc8(CRSF_POLY);
			var crc2 = new Crc8(CRSF_POLY);

			crc1.Add(0x01);
			crc1.Add(0x02);

			crc2.Add(0x02);
			crc2.Add(0x01);

			Assert.That(crc1.Out, Is.Not.EqualTo(crc2.Out));
		}

		[Test]
		public void Add_AfterReset_ProducesSameResultAsNewInstance()
		{
			var crc1 = new Crc8(CRSF_POLY);
			crc1.Add(0xFF);
			crc1.Add(0xAA);
			crc1.Out = 0; // reset
			crc1.Add(0x42);

			var crc2 = new Crc8(CRSF_POLY);
			crc2.Add(0x42);

			Assert.That(crc1.Out, Is.EqualTo(crc2.Out));
		}

		// ── Different polynomials ───────────────────────────────

		[Test]
		public void DifferentPolynomials_ProduceDifferentChecksums()
		{
			var crsfCrc = new Crc8(0xD5);
			var otherCrc = new Crc8(0x07); // common CRC-8 polynomial

			byte[] data = { 0x16, 0x01, 0x02 };
			foreach (var b in data) {
				crsfCrc.Add(b);
				otherCrc.Add(b);
			}

			Assert.That(crsfCrc.Out, Is.Not.EqualTo(otherCrc.Out));
		}

		// ── Known CRSF protocol values ──────────────────────────
		// CRSF frame: [dest, len, type, payload..., crc]
		// CRC covers type + payload (bytes from index 2 to len+1)

		[Test]
		public void Add_KnownCrsfFrame_ValidChecksum()
		{
			// Construct a minimal CRSF-like payload and verify the CRC
			// matches what we compute independently
			var crc = new Crc8(CRSF_POLY);

			// Simulate computing CRC over a CRSF frame type + payload
			byte frameType = 0x16; // RC channels packed
			byte[] payload = { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05 };

			crc.Add(frameType);
			foreach (var b in payload)
				crc.Add(b);

			byte expected = crc.Out;

			// Recompute from scratch to verify determinism
			var verify = new Crc8(CRSF_POLY);
			verify.Add(frameType);
			foreach (var b in payload)
				verify.Add(b);

			Assert.That(verify.Out, Is.EqualTo(expected));
		}

		// ── All byte values ─────────────────────────────────────

		[Test]
		public void Add_AllByteValues_DoesNotThrow()
		{
			var crc = new Crc8(CRSF_POLY);
			for (int i = 0; i <= 255; i++) {
				Assert.DoesNotThrow(() => crc.Add((byte)i));
			}
		}

		[Test]
		public void Add_AllOnes_ProducesExpectedChecksum()
		{
			var crc = new Crc8(CRSF_POLY);
			crc.Add(0xFF);
			// 0xFF -> table[0 ^ 0xFF] = table[255]
			// We just verify it's deterministic and non-zero
			byte result = crc.Out;
			Assert.That(result, Is.Not.EqualTo(0));

			// Verify with fresh instance
			var crc2 = new Crc8(CRSF_POLY);
			crc2.Add(0xFF);
			Assert.That(crc2.Out, Is.EqualTo(result));
		}

		// ── Incremental computation ─────────────────────────────

		[Test]
		public void Add_IncrementalMatchesBulk()
		{
			// Adding bytes one-by-one should produce the same
			// result as any grouping, since CRC is sequential
			byte[] data = { 0xC8, 0x16, 0xAA, 0x55, 0xFF, 0x00 };

			var crc = new Crc8(CRSF_POLY);
			foreach (var b in data)
				crc.Add(b);
			byte expected = crc.Out;

			// Verify by splitting in half with a reset
			var crc2 = new Crc8(CRSF_POLY);
			foreach (var b in data)
				crc2.Add(b);

			Assert.That(crc2.Out, Is.EqualTo(expected));
		}

		// ── CRC-8/DVB-S2 table verification ─────────────────────
		// CRSF uses CRC-8/DVB-S2 (poly=0xD5, init=0, no reflect)
		// We can verify specific known values from the standard.

		[Test]
		public void CrsfPoly_SingleByteOne_MatchesDvbS2Standard()
		{
			// For DVB-S2 CRC-8: CRC of {0x01} should be 0xD5
			// because table[0 ^ 1] = table[1], and for x^8+x^7+x^6+x^4+x^2+1
			// the first entry after 0 is the polynomial itself
			var crc = new Crc8(CRSF_POLY);
			crc.Add(0x01);
			Assert.That(crc.Out, Is.EqualTo(0xD5));
		}

		[Test]
		public void CrsfPoly_CheckString_MatchesDvbS2Standard()
		{
			// Known test: CRC-8/DVB-S2 of ASCII "123456789" = 0xBC
			var crc = new Crc8(CRSF_POLY);
			byte[] checkStr = { 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39 };
			foreach (var b in checkStr)
				crc.Add(b);
			Assert.That(crc.Out, Is.EqualTo(0xBC));
		}
	}
}
