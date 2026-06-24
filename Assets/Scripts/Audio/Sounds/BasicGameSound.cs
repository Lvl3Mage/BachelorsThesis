using System;
using UnityEngine;

namespace Project.Sounds
{
	[Serializable]
	public class SoundParamConfig : IGameSound
	{

		[SerializeField][Range(0,1)] float volume = 1;
		[SerializeField][Range(0,1)] float pitch = 1;
		[SerializeField] [Range(0, 1)] float spatialBlend = 0;
		[SerializeField] float minDistance = 1;
		[SerializeField] float maxDistance = 500;


		public void Apply(AudioSource source)
		{
			source.volume = volume;
			source.pitch = pitch;
			source.spatialBlend = spatialBlend;
			source.minDistance = minDistance;
			source.maxDistance = maxDistance;

		}
	}
	[CreateAssetMenu(fileName = "Game Sound", menuName = "Sounds/Basic Game Sound")]
	public class BasicGameSound : GameSound
	{

		[SerializeField] AudioClip clip;
		[SerializeField] SoundParamConfig config;


		public override void Apply(AudioSource source)
		{
			source.clip = clip;
			config.Apply(source);

		}
	}
}