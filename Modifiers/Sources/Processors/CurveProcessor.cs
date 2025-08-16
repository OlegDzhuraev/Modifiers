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
using UnityEngine;

namespace InsaneOne.Modifiers.Processors
{
	[Serializable]
	public class CurveProcessor : ModifierProcessor
	{
		[Tooltip("Time is raw value of Param, Value is result for this raw value. For example, you can make armor param, which becomes less powerful when increasing a lot: like 5 armor gives 25% damage reduction, but 20 armor gives 50% damage reduction instead of 100%.")]
		public AnimationCurve Curve;

		public override float Process(float value) => Curve.Evaluate(value);
	}
}

#endif