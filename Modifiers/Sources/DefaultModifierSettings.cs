using UnityEngine;

namespace InsaneOne.Modifiers
{
	[CreateAssetMenu(fileName = "DefaultModifierSettings", menuName = "Modifiers/Default modifier settings")]
	public sealed class DefaultModifierSettings : Modifier
	{
		static DefaultModifierSettings instance;

		public static DefaultModifierSettings Get()
		{
			if (!instance)
			{
				instance = Resources.Load<DefaultModifierSettings>("DefaultModifierSettings");
				instance.Init();
			}

			return instance;
		}
	}
}