using UnityEngine;

namespace Project.Sounds
{
	[CreateAssetMenu(fileName = "Random Game Sound Collection", menuName = "Sounds/Random Game Sound Collection")]
	public class RandomGameSoundCollection : GameSound
	{
		[SerializeField] GameSound[] sounds;


		public override void Apply(AudioSource source)
		{
			if (sounds.Length == 0) return;
			GameSound sound = sounds[Random.Range(0, sounds.Length)];
			sound.Apply(source);
		}
	}
}