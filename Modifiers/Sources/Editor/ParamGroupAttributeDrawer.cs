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
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;

namespace InsaneOne.Modifiers.Dev
{
	[CustomPropertyDrawer(typeof(ParamGroupAttribute))]
	public class ParamGroupAttributeDrawer : PropertyDrawer
	{
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			var root = new VisualElement();
			root.style.flexDirection = FlexDirection.Row;

			if (!UnityModifiersSettings.TryGetEditor(out var settings))
			{
				root.Add(new HelpBox("No Modifier Settings found!", HelpBoxMessageType.Warning));
				return root;
			}

			var groupIndicator = new GroupIndicator();
			groupIndicator.SetGroup(property.stringValue);

			var popup = new PopupField<string>
			{
				tooltip = "You can define custom groups with unique color and other data.",
				value = property.stringValue,
				choices = GetGroups(settings),
				style = { minWidth = 80, width = 140 },
			};

			popup.RegisterValueChangedCallback(OnPopupChanged);

			Undo.undoRedoPerformed += RefreshAfterUndo;

			root.RegisterCallback<DetachFromPanelEvent>(_ =>
			{
				Undo.undoRedoPerformed -= RefreshAfterUndo;
			});

			root.Add(groupIndicator);
			root.Add(popup);

			return root;

			void OnPopupChanged(ChangeEvent<string> evt)
			{
				property.stringValue = evt.newValue;
				groupIndicator.SetGroup(property.stringValue);
				property.serializedObject.ApplyModifiedProperties();
			}

			void RefreshAfterUndo()
			{
				property.serializedObject.Update();
				popup.SetValueWithoutNotify(property.stringValue);
				groupIndicator.SetGroup(property.stringValue);
			}
		}

		static List<string> GetGroups(UnityModifiersSettings settings)
		{
			var result = new List<string> { new ("None") };

			foreach (var group in settings.ParamGroups)
				result.Add(group.Name);

			return result;
		}
	}
}
#endif