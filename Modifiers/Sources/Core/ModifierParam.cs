namespace InsaneOne.Modifiers
{
	[System.Serializable]
	public struct ModifierParam
	{
#if UNITY_5_3_OR_NEWER
		[Modifier]
#endif
		public string Type;
		public float Value;

		public float GetProcessedValue(IModifiersSettings settings)
		{
			var data = settings?.GetModifierParamData(Type);
			var result = Value;

			if (data != null)
			{
				foreach (var processor in data.Processors)
					result = processor.Process(result);
			}

			return result;
		}
	}
}