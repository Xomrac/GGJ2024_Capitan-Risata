using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace XomracLabs
{

	public class AudioManager : MonoBehaviour
	{
		public static AudioManager instance;
		[SerializeField] private AudioSource clipPlayer;
		[SerializeField] private Button menuButton;

		[SerializeField] private Vector2 pitchRange;

		

		

		
		private void Awake()
		{
			instance = this;
			menuButton.onClick.AddListener(()=>SceneManager.LoadScene("Menu"));
		}


		public void PlayClip(AudioClip clip)
		{
			if (clip==null)
			{
				return;
			}
			clipPlayer.pitch = 1;
			clipPlayer.PlayOneShot(clip);
		}
		
		public void PlayClipRandomPitched(AudioClip clip)
		{
			if (clip==null)
			{
				return;
			}
			clipPlayer.pitch = Random.Range(pitchRange.x, pitchRange.y);
			clipPlayer.PlayOneShot(clip);
		}
			
		
		
	}

}