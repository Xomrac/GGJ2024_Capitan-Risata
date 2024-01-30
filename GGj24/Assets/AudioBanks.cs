using System.Collections.Generic;
using UnityEngine;

namespace XomracLabs
{

	[CreateAssetMenu(fileName = "new AudioBank", menuName = "Data/AudioBank", order = 0)]
	public class AudioBanks : ScriptableObject
	{
		public List<AudioClip> clips;

		[SerializeField] private Vector2 pitchRange;
		public Vector2 PitchRange => pitchRange;

		

		


		public AudioClip GetClip()
		{
			if (clips.Count<1)
			{
				return null;
			}
			return clips[Random.Range(0, clips.Count)];
		}
	}

}