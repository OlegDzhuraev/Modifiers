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

#if UNITY_5_3_OR_NEWER
		[UnityEngine.SerializeReference, SubclassSelector]
#endif
		public ModifierProcessor[] Processors;
	}
}