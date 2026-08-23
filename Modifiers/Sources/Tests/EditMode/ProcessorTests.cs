using NUnit.Framework;
using UnityEngine;
using InsaneOne.Modifiers.Processors;

namespace InsaneOne.Modifiers.Tests
{
	public class ProcessorTests
	{
		[TestCase(-5f, 0f, 10f, 0f)]
		[TestCase(15f, 0f, 10f, 10f)]
		[TestCase(5f, 0f, 10f, 5f)]
		public void ClampProcessor_ClampsValueToRange(float input, float min, float max, float expected)
		{
			var processor = new ClampProcessor { MinValue = min, MaxValue = max };

			Assert.AreEqual(expected, processor.Process(input));
		}

		[Test]
		public void CurveProcessor_EvaluatesConfiguredCurve()
		{
			var processor = new CurveProcessor { Curve = AnimationCurve.Linear(0, 0, 10, 100) };

			Assert.AreEqual(50, processor.Process(5), 0.01f);
		}
	}
}
