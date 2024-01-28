using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using XomracLabs;

public class AnswerBalloon : MonoBehaviour
{
	[SerializeField] private Button button;
	[SerializeField] private TextMeshProUGUI text;
	[SerializeField] private float animationTime;
	[SerializeField] private AudioBanks popSouds;
	[SerializeField] private GameObject arrow;

	private bool firstTime = true;

	public void Setup(float delay, float clickableDelay, string text, UnityAction OnClick, UnityAction<string> onClickValue = null)
	{
		button.enabled = false;
		if (clickableDelay > 0)
		{
			StartCoroutine(WaitForClickableCoroutine());
		}
		StartCoroutine(DisplayCoroutine());
		return;

		IEnumerator WaitForClickableCoroutine()
		{
			yield return new WaitForSeconds(clickableDelay);
			button.enabled = true;
		}

		IEnumerator DisplayCoroutine()
		{
			if (arrow != null)
			{
				arrow.SetActive(firstTime);
			}
			firstTime = false;
			this.text.text = text;
			transform.localScale = Vector3.zero;
			yield return new WaitForSeconds(delay);
			float elapsedTime = 0;
			while (elapsedTime <= animationTime)
			{
				transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, elapsedTime / animationTime);
				elapsedTime += Time.deltaTime;
				yield return null;
			}


			AudioManager.instance.PlayClip(popSouds?.GetClip());
			button.onClick.RemoveAllListeners();
			button.onClick.AddListener(() => { onClickValue?.Invoke(text); });
			button.onClick.AddListener(() =>
			{
				transform.localScale = Vector3.zero;
				OnClick?.Invoke();
			});
		}
	}

}