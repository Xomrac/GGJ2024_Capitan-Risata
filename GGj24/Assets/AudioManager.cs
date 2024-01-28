using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace XomracLabs
{

	public class AudioManager : MonoBehaviour
	{
		public static AudioManager instance;
		[SerializeField] private AudioSource clipPlayer;
		[SerializeField] private Button menuButton;

		

		
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
			clipPlayer.PlayOneShot(clip);
		}
			
		
		
	}

}