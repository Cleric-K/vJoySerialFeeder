using System;
using System.IO.Ports;
using NUnit.Framework;
using vJoySerialFeeder;

namespace vJoySerialFeeder.Tests.SerialProtocols
{
	[TestFixture]
	public class ProtocolReaderPropertiesTests
	{
		// ── GetDefaultSerialParameters ──────────────────────────

		[TestCase(typeof(IbusReader), 115200, 8, Parity.None, StopBits.One)]
		[TestCase(typeof(SbusReader), 100000, 8, Parity.Even, StopBits.Two)]
		[TestCase(typeof(DsmReader), 115200, 8, Parity.None, StopBits.One)]
		[TestCase(typeof(KissReader), 115200, 8, Parity.None, StopBits.One)]
		[TestCase(typeof(MultiWiiReader), 115200, 8, Parity.None, StopBits.One)]
		[TestCase(typeof(CrsfReader), 420000, 8, Parity.None, StopBits.One)]
		[TestCase(typeof(FportReader), 115200, 8, Parity.None, StopBits.One)]
		[TestCase(typeof(DummyReader), 115200, 8, Parity.None, StopBits.One)]
		[TestCase(typeof(DjiControllerReader), 115200, 8, Parity.None, StopBits.One)]
		public void GetDefaultSerialParameters_ReturnsExpectedValues(
			Type readerType, int baudRate, int dataBits, Parity parity, StopBits stopBits)
		{
			var reader = (SerialReader)Activator.CreateInstance(readerType);
			var sp = reader.GetDefaultSerialParameters();

			Assert.That(sp.BaudRate, Is.EqualTo(baudRate),
				$"{readerType.Name} BaudRate");
			Assert.That(sp.DataBits, Is.EqualTo(dataBits),
				$"{readerType.Name} DataBits");
			Assert.That(sp.Parity, Is.EqualTo(parity),
				$"{readerType.Name} Parity");
			Assert.That(sp.StopBits, Is.EqualTo(stopBits),
				$"{readerType.Name} StopBits");
		}

		// ── ProtocolName ────────────────────────────────────────

		[TestCase(typeof(IbusReader), "IBUS")]
		[TestCase(typeof(SbusReader), "SBUS")]
		[TestCase(typeof(DsmReader), "DSM")]
		[TestCase(typeof(KissReader), "KISS")]
		[TestCase(typeof(MultiWiiReader), "MultiWii")]
		[TestCase(typeof(CrsfReader), "CRSF")]
		[TestCase(typeof(FportReader), "FPort")]
		[TestCase(typeof(DummyReader), "Dummy")]
		[TestCase(typeof(DjiControllerReader), "DJI Controller")]
		public void ProtocolName_ReturnsExpectedValue(Type readerType, string expected)
		{
			var reader = (SerialReader)Activator.CreateInstance(readerType);
			Assert.That(reader.ProtocolName, Is.EqualTo(expected));
		}

		// ── Configurable ────────────────────────────────────────

		[TestCase(typeof(IbusReader), true)]
		[TestCase(typeof(SbusReader), true)]
		[TestCase(typeof(DsmReader), false)]
		[TestCase(typeof(KissReader), true)]
		[TestCase(typeof(MultiWiiReader), true)]
		[TestCase(typeof(CrsfReader), false)]
		[TestCase(typeof(FportReader), true)]
		[TestCase(typeof(DummyReader), true)]
		[TestCase(typeof(DjiControllerReader), false)]
		public void Configurable_ReturnsExpectedValue(Type readerType, bool expected)
		{
			var reader = (SerialReader)Activator.CreateInstance(readerType);
			Assert.That(reader.Configurable, Is.EqualTo(expected));
		}
	}
}
