using UnityEngine;

namespace Project.Sounds
{
	public interface IGameSound
	{
		public void Apply(AudioSource source);

	}
	public abstract class GameSound : ScriptableObject, IGameSound
	{
		public abstract void Apply(AudioSource source);
	}
}