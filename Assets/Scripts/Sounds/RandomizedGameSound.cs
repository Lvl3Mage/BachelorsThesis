using UnityEngine;

namespace Project.Sounds
{
	[CreateAssetMenu(fileName = "Randomized Game Sound", menuName = "Sounds/Randomized Game Sound")]
	public class RandomizedGameSound : GameSound
	{
		[SerializeField] AudioClip[] clips;
		[SerializeField][Range(0,1)] float volume = 1;
		[SerializeField] [Range(0,1)] float volumeVariance;
		[SerializeField][Range(0,1)] float pitch = 1;
		[SerializeField] [Range(0,1)] float pitchVariance;


		public override SoundData GetSound()
		{
			if (clips.Length == 0) return null;
			AudioClip clip = clips[Random.Range(0, clips.Length)];
			return new SoundData{
				Clip = clip,
				Volume = volume + Random.Range(-volumeVariance, volumeVariance),
				Pitch = pitch + Random.Range(-pitchVariance, pitchVariance)
			};
		}
	}
}