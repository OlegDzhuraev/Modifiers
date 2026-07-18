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
using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace InsaneOne.Modifiers.Tools
{
	[CustomEditor(typeof(UnityModifiersSettings))]
	public class UnityModifiersSettingsEditor : Editor
	{
		SerializedProperty paramsProp;

		VisualElement root;
		ScrollView paramGroup;

		Texture removeIcon;

		TextField filter;

		public override VisualElement CreateInspectorGUI()
		{
			removeIcon = EditorGUIUtility.IconContent("Toolbar Minus").image;
			var styles = Resources.Load<StyleSheet>("InsaneOne/modifiers");
			paramsProp = serializedObject.FindProperty("supportedParams");
			var groupsProp = serializedObject.FindProperty("groups");

			root = new VisualElement();
			root.styleSheets.Add(styles);

			filter = new TextField("Filter");
			filter.RegisterValueChangedCallback(OnFilterChanged);

			paramGroup = new ScrollView(ScrollViewMode.Vertical);
			paramGroup.horizontalScrollerVisibility = ScrollerVisibility.Hidden;
			paramGroup.AddToClassList("unity-mods-settings-params-group");

			var addBtn = new Button(OnAddClick) { text = "Add new" };

			root.Add(filter);
			root.Add(paramGroup);
			root.Add(addBtn);
			root.Add(new PropertyField(groupsProp));

			RebuildParamsUI();

			return root;
		}

		void OnFilterChanged(ChangeEvent<string> evt) => RebuildParamsUI();

		void RebuildParamsUI()
		{
			paramGroup.Clear();

			for (var i = 0; i < paramsProp.arraySize; i++)
			{
				var property = paramsProp.GetArrayElementAtIndex(i);
				var name = property.FindPropertyRelative(nameof(ModifierParamData.Name)).stringValue;

				if (string.IsNullOrWhiteSpace(filter.value) || name.Contains(filter.value, StringComparison.InvariantCultureIgnoreCase))
					AddElement(i);
			}

			if (paramGroup.childCount == 0)
				paramGroup.Add(new HelpBox("Nothing found", HelpBoxMessageType.Info));

			paramsProp.serializedObject.Update();
		}

		void AddElement(int idx)
		{
			var line = new VisualElement();
			line.AddToClassList("unity-mods-settings-line");

			var property = paramsProp.GetArrayElementAtIndex(idx);
			var field = new PropertyField(property);
			field.AddToClassList("modifier-param-data");
			field.BindProperty(property);

			var removeBtn = new Button { style = { justifyContent = Justify.Center } };
			removeBtn.Add(new Image { image = removeIcon });
			removeBtn.RegisterCallback<ClickEvent>(ce => OnRemoveClick(ce, idx));

			line.Add(field);
			line.Add(removeBtn);

			paramGroup.Add(line);
		}

		void OnRemoveClick(ClickEvent evt, int idx)
		{
			paramsProp.DeleteArrayElementAtIndex(idx);
			ApplyChanges();
		}

		void OnAddClick()
		{
			paramsProp.InsertArrayElementAtIndex(paramsProp.arraySize - 1);
			ApplyChanges();
		}

		void ApplyChanges()
		{
			paramsProp.serializedObject.ApplyModifiedProperties();
			RebuildParamsUI();
			EditorUtility.SetDirty(target);
		}
	}
}
#endif