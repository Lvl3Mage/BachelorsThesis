using UnityEngine;

namespace Project.Sounds
{
	[CreateAssetMenu(fileName = "Random Game Sound Collection", menuName = "Sounds/Random Game Sound Collection")]
	public class RandomGameSoundCollection : GameSound
	{
		[SerializeField] GameSound[] sounds;


		public override SoundData GetSound()
		{
			if (sounds.Length == 0) return null;
			GameSound sound = sounds[Random.Range(0, sounds.Length)];
			return sound.GetSound();
		}
	}
}