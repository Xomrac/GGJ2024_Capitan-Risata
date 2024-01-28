using UnityEngine;

namespace XomracLabs
{

	public class AnimationEndNotifier : MonoBehaviour
	{
		[SerializeField] private AudioClip audioClip;

		
		public void PlaySound()
		{AudioManager.instance.PlayClip(audioClip);}
		public void NotifyAnimationEnded()
		{
			GameEvents.OnAnimationEnded?.Invoke();
		}
	}

}