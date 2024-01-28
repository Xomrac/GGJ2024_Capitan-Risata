using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;
using XomracLabs;

[Serializable]
public struct DialogueData
{
	[TextArea] public string line;
	public DialogueSide side;
	public bool interruptDialogue;
	public UnityEvent onDialogueDisplayed;
	public UnityEvent onDialogueEnded;
}

public enum DialogueSide
{
	Left,
	Right
}

public class DialogueManager : MonoBehaviour
{

	[SerializeField] private float balloonAnimationTime;
	[SerializeField] private AudioBanks popSounds;

	
	[SerializeField] private GameObject balloon;
	[SerializeField] private TextMeshProUGUI balloonText;
	[SerializeField] private Animator characterAnimator;
	[SerializeField] private float minTime = 1;
	[SerializeField] private float timePerLetter = 0.02f;

	[SerializeField] private MouthMover leftCharacterMouth;
	[SerializeField] private MouthMover rightCharacterMouth;

	[SerializeField] private List<DialogueData> lines;
	[SerializeField] private bool skipDialogue;

	
	[SerializeField]private int currentIndex = 0;

	private IEnumerator Start()
	{
		yield return new WaitForSeconds(.7f);
		if (skipDialogue)
		{
			minTime = 0;
			timePerLetter = 0;
		}
		DisplayLine();
	}

	private void OnEnable()
	{
		GameEvents.OnAnimationEnded += DisplayLine;
		BookManager.OnBookFinished += ResetAndDisplay;
	}

	private void OnDisable()
	{
		GameEvents.OnAnimationEnded -= DisplayLine;
		BookManager.OnBookFinished -= ResetAndDisplay;
	}

	public void ResetAndDisplay()
	{
		DisplayLine();
	}

	public void ResetIndex(int index)
	{
		currentIndex = index;
	}

	public void MakeCharacterGoAway()
	{
		balloon.gameObject.SetActive(false);
		leftCharacterMouth.enabled = false;
		rightCharacterMouth.enabled = false;
		characterAnimator.enabled = true;
		characterAnimator.SetTrigger("WalkAway");
	}

	public void StartPunMaking()
	{
		balloon.gameObject.SetActive(false);
	}
	

	public void DisplayLine()
	{
		lines[currentIndex].onDialogueDisplayed?.Invoke();
		balloonText.text = lines[currentIndex].line;
		balloon.transform.localScale=Vector3.zero;
		balloon.gameObject.SetActive(true);

		StartCoroutine(DisplayCoroutine());

		IEnumerator DisplayCoroutine()
		{
			Vector3 finalScale = lines[currentIndex].side == DialogueSide.Left ? Vector3.one : new Vector3(-1, 1, 1);
			balloonText.transform.localScale = finalScale;

			float elapsedTime = 0;
			while (elapsedTime<=balloonAnimationTime)
			{
				balloon.transform.localScale = Vector3.Lerp(Vector3.zero, finalScale,elapsedTime/balloonAnimationTime);
				elapsedTime += Time.deltaTime;
				yield return null;
			}
			AudioManager.instance.PlayClip(popSounds?.GetClip());
			
			if (lines[currentIndex].side == DialogueSide.Right)
			{
				rightCharacterMouth.enabled = true;
				leftCharacterMouth.enabled = false;
			}
			else
			{
				rightCharacterMouth.enabled = false;
				leftCharacterMouth.enabled = true;
			}
			CheckIfLastLine(DisplayLine);
		}
	}

	private void CheckIfLastLine(UnityAction OnNotLastLine)
	{
		if (currentIndex==lines.Count-1)
		{
			StartCoroutine(WaitForEndOfDialogue());
		}
		if (lines[currentIndex].interruptDialogue)
		{
			StartCoroutine(WaitForEndLine());
		}
		else
		{
			StartCoroutine(WaitForNextLine(OnNotLastLine));
		}
	}

	private IEnumerator WaitForNextLine(UnityAction onWaitEnded)
	{
		yield return new WaitForSeconds(minTime + lines[currentIndex].line.Length * timePerLetter);
		currentIndex++;
		onWaitEnded?.Invoke();
	}
		
	private IEnumerator WaitForEndLine()
	{
		yield return new WaitForSeconds(minTime + lines[currentIndex].line.Length * timePerLetter);
		lines[currentIndex].onDialogueEnded?.Invoke();
		
		currentIndex++;
	}

	private IEnumerator WaitForEndOfDialogue()
	{
		yield return new WaitForSeconds(minTime + lines[currentIndex].line.Length * timePerLetter);
		Debug.Log("End of dialogue");
		
		Debug.Log(lines[currentIndex].line);
		lines[currentIndex].onDialogueEnded?.Invoke();
	}

}