namespace InsaneOne.Modifiers
{
	[System.Serializable]
	public struct ModifierParam
	{
#if UNITY_5_3_OR_NEWER
		[ModifierAttribute]
#endif
		public string Type;
		public float Value;
	}
}