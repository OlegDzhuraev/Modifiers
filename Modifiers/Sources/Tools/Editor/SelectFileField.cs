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

using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace InsaneOne.UIElements
{
	public class SelectFileField : VisualElement
	{
		public string Path => pathField.value;

		TextField pathField;
		string extension;

		public SelectFileField(string title, string extensionFilter)
		{
			extension = extensionFilter;

			var pathRow = new VisualElement
			{
				style = { flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row) },
			};

			pathField = new TextField(title) { style = { flexGrow = 1, maxWidth = 400 } };
			var browseBtn = new Button(OnBrowseClick) { text = "Browse..." };

			pathRow.Add(pathField);
			pathRow.Add(browseBtn);

			Add(pathRow);
		}

		public void SetPath(string path)
		{
			pathField.value = path;
		}

		void OnBrowseClick()
		{
			var path = EditorUtility.OpenFilePanel("Select a file", Application.dataPath, extension);
			if (!string.IsNullOrEmpty(path))
				pathField.value = path;
		}
	}
}