using System;
using Project.Sounds;
using Sounds;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace Project.Sounds
{
	public static class AudioManager
	{
		static Pool<SoundController> pool = new Pool<SoundController>(() => {
			GameObject obj = new GameObject("PooledAudioSource");
			Object.DontDestroyOnLoad(obj);
			return obj.AddComponent<SoundController>();
		}, (sp) => !sp.Source.isPlaying);

		public static void Play(IGameSound data, SoundTracking tracking = null)
		{
			SoundController controller = pool.GetReleased();
			ResetSource(controller.Source);
			data.Apply(controller.Source);
			controller.Play();
		}

		static void ResetSource(AudioSource source)
		{
			source.clip = null;
			source.volume = 1;
			source.pitch = 1;
			source.spatialBlend = 0;
			source.minDistance = 1;
			source.maxDistance = 500;
		}
	}
}