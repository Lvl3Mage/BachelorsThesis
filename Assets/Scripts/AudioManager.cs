using System;
using Project.Sounds;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace Project.Sounds
{
	public static class AudioManager
	{
		static PooledObject<AudioSource> ass;

		class SoundPlayer
		{
			public readonly AudioSource Source;
			SoundControls boundControls;

			public SoundPlayer(AudioSource source)
			{
				Source = source;
			}

			public void Play()
			{
				Source.Play();
			}

			public SoundControls RebindControls()
			{
				boundControls = new SoundControls(Source, self => self == boundControls);
				return boundControls;
			}
		}

		public class SoundControls
		{
			AudioSource source;
			Func<SoundControls,bool> isValid;

			public SoundControls(AudioSource source, Func<SoundControls,bool> isValid)
			{
				this.source = source;
				this.isValid = isValid;
			}

			public void Stop()
			{
				if (!isValid(this)){
					return;
				}
				source.Stop();
			}

			public bool isPlaying => isValid(this) && source.isPlaying;
		}
		static Pool<SoundPlayer> pool = new Pool<SoundPlayer>(() => {
			GameObject obj = new GameObject("AudioSource");
			Object.DontDestroyOnLoad(obj);
			return new SoundPlayer(obj.AddComponent<AudioSource>());
		}, (sp) => !sp.Source.isPlaying);

		public static SoundControls Play(SoundData data)
		{
			SoundPlayer player = pool.GetReleased();
			SoundControls controls = player.RebindControls();
			data.Apply(player.Source);
			player.Play();
			return controls;
		}
	}
}