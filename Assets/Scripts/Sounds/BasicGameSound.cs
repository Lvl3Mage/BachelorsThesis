using UnityEngine;

namespace Project.Sounds
{
	[CreateAssetMenu(fileName = "Game Sound", menuName = "Sounds/Basic Game Sound")]
	public class BasicGameSound : GameSound
	{

		[SerializeField] AudioClip clip;
		[SerializeField][Range(0,1)] float volume = 1;
		[SerializeField][Range(0,1)] float pitch = 1;


		public override SoundData GetSound()
		{
			return new SoundData{
				Clip = clip,
				Volume = volume,
				Pitch = pitch
			};
		}
	}
}