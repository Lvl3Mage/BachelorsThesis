using System;
using System.Threading;
using Project.Sounds;
using UnityEngine;

namespace Sounds
{
	public delegate Vector3 SoundTracking();
	public class SoundController : MonoBehaviour
	{
		public AudioSource Source { get; private set; }

		void Awake()
		{
			Source = gameObject.AddComponent<AudioSource>();
		}

		static readonly SoundTracking NullTracking = () => Vector3.zero;
		SoundTracking tracking = NullTracking;
		public void Play(SoundTracking soundTracking = null)
		{
			soundTracking ??= NullTracking;
			tracking = soundTracking;
			Source.Play();
		}

		void Update()
		{
			if (!Source.isPlaying){
				return;
			}

			transform.position = tracking();
		}
	}

}