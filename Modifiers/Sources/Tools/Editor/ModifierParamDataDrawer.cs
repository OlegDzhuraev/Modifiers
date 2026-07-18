/*
 * Copyright 2025 Oleg Dzhuraev <godlikeaurora@gmail.com>
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#if UNITY_5_3_OR_NEWER
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace InsaneOne.Modifiers.Tools
{
	[CustomPropertyDrawer(typeof(ModifierParamData))]
	public class ModifierParamDataDrawer : PropertyDrawer
	{
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			var root = new VisualElement();
			var firstLine = new VisualElement { style = { flexDirection = FlexDirection.Row, alignContent = Align.Stretch } };

			var nameProp = property.FindPropertyRelative(nameof(ModifierParamData.Name));
			var groupProp = property.FindPropertyRelative(nameof(ModifierParamData.Group));

			AddDefaultField(nameProp, firstLine);
			AddDefaultField(groupProp, firstLine);

			root.Add(firstLine);

#if INSANEONE_MODIFIERS_FANCY_FORMAT
			var secondLine = new VisualElement { style = { flexDirection = FlexDirection.Row, alignContent = Align.Stretch } };

			var iconProp = property.FindPropertyRelative(nameof(ModifierParamData.Icon));
			var tmpIconProp = property.FindPropertyRelative(nameof(ModifierParamData.TextMeshIconId));

			AddDefaultField(iconProp, secondLine);
			AddDefaultField(tmpIconProp, secondLine);

			root.Add(secondLine);
#endif
			return root;
		}

		void AddDefaultField(SerializedProperty property, VisualElement parent)
		{
			var field = new PropertyField(property, "")
			{
				tooltip = property.displayName, style = { minWidth = 100, width = 160 },
			};
			parent.Add(field);
		}
	}
}
#endif