using System;
using InsaneOne.Modifiers.Processors;

namespace InsaneOne.Modifiers
{
	[Serializable]
	public class ModifierParamData
	{
		public string Name;

		[ParamGroup]
		public string Group;

#if UNITY_5_3_OR_NEWER && INSANEONE_MODIFIERS_FANCY_FORMAT
		public UnityEngine.UI.Image Icon;
		public string TextMeshIconId;
		public string Description;
#endif

#if UNITY_5_3_OR_NEWER
		[UnityEngine.SerializeReference, SubclassSelector]
#endif
		public ModifierProcessor[] Processors;
	}
}