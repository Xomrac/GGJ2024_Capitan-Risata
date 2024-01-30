using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using XomracLabs;
using Random = UnityEngine.Random;

public class WordsManager : MonoBehaviour
{
	[SerializeField] private List<AnswerBalloon> answerBalloons;
	[SerializeField] private AnswerBalloon finalLineBalloon;
	[SerializeField] private GameObject bookDisplayer;
	[SerializeField] private TextMeshProUGUI progressText;
	[SerializeField] private float balloonsDelay=0.5f;
	[SerializeField] private string currentPun;
	[SerializeField] private float finalPhraseMinTime;
	[SerializeField] private float timePerLetter;

	[SerializeField]private PunDatabase database;
	[SerializeField] private MouthMover mouth;
	[SerializeField] private GameObject normalCharacter;
	[SerializeField] private GameObject thinkingCharacter;

	

	
	private bool firstTime = true;

	private void Awake()
	{
		database = Resources.Load<PunDatabase>("PunDatabase");
		foreach (AnswerBalloon balloon in answerBalloons)
		{
			balloon.gameObject.SetActive(false);
		}
		finalLineBalloon.gameObject.SetActive(false);
	}

	private void OnEnable() { }

	private void OnDisable() { }
	

	public void StartPunCreatrion()
	{
		DisplaySubjects();
	}

	public void UpdatePhrase(string phrasePart)
	{
		currentPun += $" {phrasePart}";
	}

	public void StartFromBegin()
	{
		Debug.Log("New Begin");
		BookManager.puns = new List<string>();
		progressText.text = $"{BookManager.puns.Count} / {BookManager.maxPages}";
		DisplaySubjects();
	}

	public void DisplaySubjects()
	{
		normalCharacter.gameObject.SetActive(false);
		thinkingCharacter.gameObject.SetActive(true);
		mouth.enabled = false;
		if (!firstTime)
		{
			bookDisplayer.gameObject.SetActive(true);
			progressText.text = $"{BookManager.puns.Count}/{BookManager.maxPages}";
		}
		currentPun = "";
		finalLineBalloon.gameObject.SetActive(false);
		var possibleSubjects = new List<string>(database.subjects);

		for (int index = 0; index < answerBalloons.Count; index++)
		{
			AnswerBalloon balloon = answerBalloons[index];
			var randomSubject = possibleSubjects[Random.Range(0, possibleSubjects.Count)];
			var clickableDelay = (balloonsDelay * answerBalloons.Count);
			balloon.gameObject.SetActive(true);
			balloon.Setup(balloonsDelay*index,clickableDelay,randomSubject, DisplayVerbs, UpdatePhrase);
			possibleSubjects.Remove(randomSubject);
		}
	}

	public void DisplayVerbs()
	{
		firstTime = false;
		var possibleVerbs = new List<string>(database.verbs);
		for (int index = 0; index < answerBalloons.Count; index++)
		{
			AnswerBalloon balloon = answerBalloons[index];
			string randomVerb = possibleVerbs[Random.Range(0, possibleVerbs.Count)];
			var clickableDelay = (balloonsDelay * answerBalloons.Count);
			balloon.Setup(balloonsDelay*index,clickableDelay,randomVerb, DisplayObjects, UpdatePhrase);
			possibleVerbs.Remove(randomVerb);
		}
	}

	public void DisplayObjects()
	{
		var possibleObjects = new List<string>(database.objects);
		for (int index = 0; index < answerBalloons.Count; index++)
		{
			AnswerBalloon balloon = answerBalloons[index];
			var randomObject = possibleObjects[Random.Range(0, possibleObjects.Count)];
			var clickableDelay = (balloonsDelay * answerBalloons.Count);
			balloon.Setup(balloonsDelay*index,clickableDelay,randomObject, DisplayFinalPhrase, UpdatePhrase);
			possibleObjects.Remove(randomObject);
		}
	}

	public void DisplayFinalPhrase()
	{
		normalCharacter.gameObject.SetActive(true);
		thinkingCharacter.gameObject.SetActive(false);
		mouth.enabled = true;

		foreach (AnswerBalloon balloon in answerBalloons)
		{
			balloon.gameObject.SetActive(false);
		}
		finalLineBalloon.gameObject.SetActive(true);
		var pun = $"{currentPun}\n{database.punchlines[Random.Range(0, database.punchlines.Count)]}";
		finalLineBalloon.Setup(balloonsDelay,-1,pun, () => CheckForEnd(pun));
		StartCoroutine(WaitCoroutine());
		return;

		IEnumerator WaitCoroutine()
		{
			BookManager.AddPun(currentPun);
			progressText.text = $"{BookManager.puns.Count}/{BookManager.maxPages}";
			yield return new WaitForSeconds(finalPhraseMinTime + pun.Length * timePerLetter);

			CheckForEnd(pun);
		}
	}

	private void CheckForEnd(string pun)
	{
		var hasFinished = BookManager.HasFinished;
		if (!hasFinished)
		{
			DisplaySubjects();
		}
		else
		{
			Debug.Log("Finished");
			foreach (AnswerBalloon balloon in answerBalloons)
			{
				balloon.gameObject.SetActive(false);
			}
			WriteBook();
			BookManager.puns = new List<string>();
			BookManager.OnBookFinished?.Invoke();
		}
	}

	private void WriteBook()
	{
		// Get directory path
		string dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Capitan Risata");
	
		Debug.Log(dir);
		// If directory does not exist, create it
		if (!Directory.Exists(dir))
		{
			Directory.CreateDirectory(dir);
		}
		var randomTitle = database.titles[Random.Range(0, database.titles.Count)];
		// Write each line of puns into the file.
		Debug.Log(randomTitle);
		Debug.Log(dir);
		string invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());

		foreach (char c in invalid)
		{
			randomTitle = randomTitle.Replace(c.ToString(), ""); 
		}

		using (var file = new StreamWriter(Path.Combine(dir, $"{randomTitle}.txt")))
		{
			file.WriteLine(randomTitle + "\n");
			file.WriteLine("------------------------------\n");
			foreach (string pun in BookManager.puns)
			{
				file.WriteLine(pun + "\n");
			}
			file.WriteLine("------------------------------\n");
			file.WriteLine("Er Pupone - Roma " + DateTime.Now);
		}
	}
}