using NUnit.Framework;

namespace InsaneOne.Modifiers.Tests
{
	public class ModifierTests
	{
		[Test]
		public void IsEmpty_NoParameters_ReturnsTrue()
		{
			var modifier = new Modifier();

			Assert.IsTrue(modifier.IsEmpty());
		}

		[Test]
		public void IsEmpty_HasParameters_ReturnsFalse()
		{
			var modifier = new Modifier();
			modifier.SetParamValue("Health", 10);

			Assert.IsFalse(modifier.IsEmpty());
		}

		[Test]
		public void GetRawValue_ExistingType_ReturnsValue()
		{
			var modifier = new Modifier();
			modifier.SetParamValue("Health", 10);

			Assert.AreEqual(10, modifier.GetRawValue("Health"));
		}

		[Test]
		public void GetRawValue_MissingType_ReturnsZero()
		{
			var modifier = new Modifier();

			Assert.AreEqual(0, modifier.GetRawValue("Health"));
		}

		[TestCase(1f, true)]
		[TestCase(0f, false)]
		[TestCase(-1f, false)]
		public void IsTrue_ReflectsValueSign(float value, bool expected)
		{
			var modifier = new Modifier();
			modifier.SetParamValue("Tag", value);

			Assert.AreEqual(expected, modifier.IsTrue("Tag"));
		}

		[Test]
		public void SetParamValue_NewType_AddsParam()
		{
			var modifier = new Modifier();

			modifier.SetParamValue("Health", 5);

			Assert.AreEqual(1, modifier.Parameters.Count);
			Assert.AreEqual(5, modifier.GetRawValue("Health"));
		}

		[Test]
		public void SetParamValue_ExistingType_OverridesInPlaceInsteadOfDuplicating()
		{
			var modifier = new Modifier();
			modifier.SetParamValue("Health", 5);

			modifier.SetParamValue("Health", 20);

			Assert.AreEqual(1, modifier.Parameters.Count);
			Assert.AreEqual(20, modifier.GetRawValue("Health"));
		}

		[Test]
		public void Clone_CopiesCurrentParameters()
		{
			var modifier = new Modifier();
			modifier.SetParamValue("Health", 10);
			modifier.SetParamValue("Damage", 3);

			var clone = modifier.Clone();

			Assert.AreEqual(10, clone.GetRawValue("Health"));
			Assert.AreEqual(3, clone.GetRawValue("Damage"));
		}

		[Test]
		public void Clone_IsIndependentFromOriginal()
		{
			var modifier = new Modifier();
			modifier.SetParamValue("Health", 10);
			var clone = modifier.Clone();

			modifier.SetParamValue("Health", 999);
			modifier.SetParamValue("NewStat", 1);

			Assert.AreEqual(10, clone.GetRawValue("Health"));
			Assert.AreEqual(1, clone.Parameters.Count);
		}
	}
}
