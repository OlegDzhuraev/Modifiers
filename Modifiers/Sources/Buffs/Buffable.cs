using System;
using System.Collections.Generic;
using UnityEngine;

namespace InsaneOne.Modifiers
{
	public class Buffable : MonoBehaviour
	{
		public event Action<Buff> BuffAdded, BuffRemoved;

		[SerializeField] Modifable modifable;

		public Modifable Modifable
		{
			get => modifable;
			set => modifable = value;
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
			modifable.Add(buff.Modifier);
			
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
			modifable.Remove(buff.Modifier);
			buffs.Remove(buff);
			
			BuffRemoved?.Invoke(buff);
		}
	}
}