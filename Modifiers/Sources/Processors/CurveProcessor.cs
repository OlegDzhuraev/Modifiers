#if UNITY_5_3_OR_NEWER

using System;
using UnityEngine;

namespace InsaneOne.Modifiers.Processors
{
	[Serializable]
	public class CurveProcessor : ModifierProcessor
	{
		[Tooltip("Time is raw value of Param, Value is result for this raw value. For example, you can make armor param, which becomes less powerful when increasing a lot: like 5 armor gives 25% damage reduction, but 20 armor gives 50% damage reduction instead of 100%.")]
		public AnimationCurve Curve;

		public override float Process(float value) => Curve.Evaluate(value);
	}
}

#endif