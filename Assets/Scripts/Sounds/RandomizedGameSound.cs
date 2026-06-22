using UnityEngine;

namespace Project.Sounds
{
	[CreateAssetMenu(fileName = "Randomized Game Sound", menuName = "Sounds/Randomized Game Sound")]
	public class RandomizedGameSound : GameSound
	{
		[SerializeField] AudioClip[] clips;
		[SerializeField] SoundParamConfig config;
		[SerializeField] [Range(0,1)] float volumeVariance;
		[SerializeField] [Range(0,1)] float pitchVariance;


		public override void Apply(AudioSource source)
		{
			if (clips.Length == 0) return;
			AudioClip clip = clips[Random.Range(0, clips.Length)];
			source.clip = clip;
			config.Apply(source);
			source.volume += Random.Range(-volumeVariance, volumeVariance);
			source.pitch +=Random.Range(-pitchVariance, pitchVariance);
		}
	}
}