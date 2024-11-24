using System;

namespace InsaneOne.Modifiers.Processors
{
	[Serializable]
	public class ClampProcessor : ModifierProcessor
	{
		public float MinValue;
		public float MaxValue;

		public override float Process(float value) => Math.Clamp(value, MinValue, MaxValue);
	}
}