using UnityEngine;

namespace Project.Sounds
{
	public class SoundData
	{
		public AudioClip Clip;
		public float Volume;
		public float Pitch;

		public void Apply(AudioSource source)
		{
			source.clip = Clip;
			source.volume = Volume;
			source.pitch = Pitch;
		}
	}
	public interface IGameSound
	{
		public SoundData GetSound();

	}
	public abstract class GameSound : ScriptableObject, IGameSound
	{
		public abstract SoundData GetSound();
	}
}