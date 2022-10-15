namespace InsaneOne.Modifiers
{
	public class BuffTimer
	{
		public Buff Buff { get; }
		public float TimeLeft { get; set; }

		public BuffTimer(Buff buff)
		{
			Buff = buff;
			TimeLeft = buff.LifeTime;
		}
	}
}