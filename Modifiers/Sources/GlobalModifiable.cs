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

using UnityEngine;

namespace InsaneOne.Modifiers
{
	public sealed class GlobalModifiable
	{
		static GameObject instance;

		public static GameObject GetSceneInstance()
		{
			if (instance != null)
				return instance;

			var go = new GameObject("GlobalModifiable_Scene");
			go.AddComponent<Modifiable>();

			instance = go;

			return instance;
		}
	}
}