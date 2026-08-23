using NUnit.Framework;
using InsaneOne.Modifiers.Processors;

namespace InsaneOne.Modifiers.Tests
{
	public class ModifierParamTests
	{
		[Test]
		public void GetProcessedValue_NullSettings_ReturnsRawValue()
		{
			var param = new ModifierParam { Type = "Health", Value = 42 };

			Assert.AreEqual(42, param.GetProcessedValue(null));
		}

		[Test]
		public void GetProcessedValue_NoDataForType_ReturnsRawValue()
		{
			var param = new ModifierParam { Type = "Health", Value = 42 };
			var settings = new StubModifiersSettings();

			Assert.AreEqual(42, param.GetProcessedValue(settings));
		}

		[Test]
		public void GetProcessedValue_WithClampProcessor_ClampsValue()
		{
			var param = new ModifierParam { Type = "Health", Value = 999 };
			var settings = new StubModifiersSettings();
			settings.Set("Health", new ModifierParamData
			{
				Name = "Health",
				Processors = new ModifierProcessor[] { new ClampProcessor { MinValue = 0, MaxValue = 100 } },
			});

			Assert.AreEqual(100, param.GetProcessedValue(settings));
		}

		[Test]
		public void GetProcessedValue_MultipleProcessors_AppliedInOrder()
		{
			var param = new ModifierParam { Type = "Health", Value = 150 };
			var settings = new StubModifiersSettings();
			settings.Set("Health", new ModifierParamData
			{
				Name = "Health",
				Processors = new ModifierProcessor[]
				{
					new ClampProcessor { MinValue = 0, MaxValue = 100 }, // 150 -> 100
					new ClampProcessor { MinValue = 0, MaxValue = 50 }, // 100 -> 50
				},
			});

			Assert.AreEqual(50, param.GetProcessedValue(settings));
		}
	}
}
