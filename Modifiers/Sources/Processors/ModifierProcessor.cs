using System;

namespace InsaneOne.Modifiers.Processors
{
	/// <summary> Can be used to modify output modifier value when reading in runtime. </summary>
	[Serializable]
	public abstract class ModifierProcessor
	{
		public abstract float Process(float value);
	}
}