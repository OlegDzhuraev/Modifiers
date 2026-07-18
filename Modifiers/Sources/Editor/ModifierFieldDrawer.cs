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

#if UNITY_5_3_OR_NEWER && INSANEONE_MODIFIERS_UNITY_EXTENSION

using UnityEngine;
using UnityEditor;

namespace InsaneOne.Modifiers.Dev
{
	[CustomPropertyDrawer(typeof(ModifierField))]
	public class ModifierFieldDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);

			var valueProp = property.FindPropertyRelative(nameof(ModifierField.DefaultValue));
			var modifierProp = property.FindPropertyRelative(nameof(ModifierField.Modifier));

			position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

			var widthA = position.width / 4;
			var widthB = position.width - widthA - 5;
			var valueRect = new Rect(position.x, position.y, widthA, position.height);
			var modifierRect = new Rect(position.x + widthA + 5, position.y, widthB, position.height);

			EditorGUI.PropertyField(modifierRect, modifierProp, GUIContent.none);
			EditorGUI.PropertyField(valueRect, valueProp, GUIContent.none);

			EditorGUI.EndProperty();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			var modifierProp = property.FindPropertyRelative(nameof(ModifierField.Modifier));
			var height = EditorGUI.GetPropertyHeight(modifierProp, true);
			return height;
		}
	}
}

#endif