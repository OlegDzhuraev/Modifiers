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

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace InsaneOne.Modifiers.Buffs
{
	public class Buffable : MonoBehaviour
	{
		public event Action<Buff> BuffAdded, BuffRemoved;

		[SerializeField] Modifiable modifiable;

		public Modifiable Modifiable
		{
			get => modifiable;
			set => modifiable = value;
		}
		
		readonly List<Buff> buffs = new List<Buff>();
		readonly List<BuffTimer> buffTimers = new List<BuffTimer>();

		void Update()
		{
			var dTime = Time.deltaTime;
			
			for (var i = buffTimers.Count - 1; i >= 0; i--)
			{
				var timer = buffTimers[i];
				timer.TimeLeft -= dTime;

				if (timer.TimeLeft <= 0)
				{
					RemoveBuff(timer.Buff);
					buffTimers.RemoveAt(i);
				}
			}
		}

		public void AddBuff(Buff buff)
		{
			if (GetStacksCount(buff) >= buff.MaxStacks)
				return;
			
			buffs.Add(buff);
			modifiable.Add(buff.Modifier);
			
			if (buff.LifeTime > 0)
				buffTimers.Add(new BuffTimer(buff));
			
			BuffAdded?.Invoke(buff);
		}

		int GetStacksCount(Buff buff)
		{
			var stacks = 0;
			
			foreach (var addedBuff in buffs)
				if (addedBuff == buff)
					stacks++;

			return stacks;
		}

		public void RemoveBuff(Buff buff)
		{
			modifiable.Remove(buff.Modifier);
			buffs.Remove(buff);
			
			BuffRemoved?.Invoke(buff);
		}
	}
}