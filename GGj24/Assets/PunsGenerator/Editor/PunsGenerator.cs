using System.Linq;

namespace XomracLabs
{

	using System;
	using System.Collections;
	using UnityEngine.Networking;
	using System.IO;
	using Unity.EditorCoroutines.Editor;
	using UnityEngine;
	using UnityEditor;
	using Random = UnityEngine.Random;

	/// <summary>
	/// TottiTool is a static class that provides utility methods for downloading a CSV file, saving it locally
	/// and creating a ScriptableObject from the CSV data. It also includes methods for displaying a random pun
	/// from the created database in the Unity editor.
	/// </summary>
	public static class PunsGenerator
	{
		
		
		private static string GetRelativePath(string fileName)
		{
			string[] guids = AssetDatabase.FindAssets("t:Script PunsGenerator");
			if (guids == null || guids.Length == 0)
			{
				return "";
			}
			string relativePath = AssetDatabase.GUIDToAssetPath(guids.First());
			relativePath = Path.GetDirectoryName(relativePath);
			string finalPath = Path.Combine(relativePath ?? string.Empty, fileName);
			return finalPath;
		}
		/// <summary>
		/// The ID of the file.
		/// </summary>
		/// <value>
		/// A string representing the ID of the file.
		/// </value>
		private static readonly string fileID = "1bBjF6dHB3LVZV9AfoqYRj7MtYuJhCSfv";
		/// <summary>
		/// The private static readonly string variable _outputPath holds the path to the output file. </summary> <remarks>
		/// The output file is a CSV file located in the Assets/Resources/PunsGenerator folder and its name is myFile.csv.
		/// The path can be accessed through the _outputPath variable. </remarks>
		/// /
		private static string _outputPath = "Resources/PunsGenerator/PunCsv.csv";
		/// <summary>
		/// Represents the coroutine for downloading content in the editor.
		/// </summary>
		private static EditorCoroutine downloadCoroutine;
		/// <summary>
		/// This variable represents the PunDatabase instance used for data storage and retrieval.
		/// It is marked as private and static, ensuring it is accessible only within its enclosing class
		/// and that there is only one instance shared among all instances of the class.
		/// </summary>
		private static PunDatabase database;

		/// <summary>
		/// Method for downloading the database.
		/// </summary>
		/// <returns>Nothing.</returns>
		/// <remarks>
		/// This method is used to initiate the download of the database in CSV file format.
		/// </remarks>
		[MenuItem("Pun Generator/Create Database #&f")]
		private static void DownloadDatabase()
		{
			if (downloadCoroutine != null)
			{
				EditorCoroutineUtility.StopCoroutine(downloadCoroutine);
			}
			downloadCoroutine = EditorCoroutineUtility.StartCoroutineOwnerless(DownloadCSVFile());
		}

		/// <summary>
		/// Opens a window that displays a random pun.
		/// </summary>
		[MenuItem("Pun Generator/Display Pun #&g")]
		private static void DisplayPun()
		{
			UnityEngine.Object newDatabase = Resources.Load("PunDatabase");
			var punDatabase = newDatabase as PunDatabase;
			string pun = $"{punDatabase.subjects[Random.Range(0, punDatabase.subjects.Count)]} {punDatabase.verbs[Random.Range(0, punDatabase.verbs.Count)]} {punDatabase.objects[Random.Range(0, punDatabase.objects.Count)]}";
			NotifyInScene(pun, 5);
		}

		/// <summary>
		/// Downloads a CSV file from a given URL.
		/// </summary>
		/// <returns>An IEnumerator that can be used as a coroutine.</returns>
		private static IEnumerator DownloadCSVFile()
		{
			Debug.Log("Downloading csv file...");
			string url = $"https://drive.google.com/uc?id={fileID}";
			UnityWebRequest www = UnityWebRequest.Get(url);
			yield return www.SendWebRequest();

			if (www.result == UnityWebRequest.Result.Success)
			{
				var file = new FileInfo(GetRelativePath(_outputPath));
				SaveTheFile(file, www.downloadHandler.data);

				Debug.Log("Download completed!");
			}
			else
			{
				Debug.LogError("Error: " + www.error);
			}
		}
		
		/// <summary>
		/// Saves the given file with the provided content and updates the PunDatabase. </summary> <param name="file">The file to be saved.</param> <param name="content">The content to be saved.</param> <returns>Nothing</returns>
		/// /
		private static void SaveTheFile(FileInfo file, byte[] content)
		{
			Directory.CreateDirectory(file.Directory.FullName);
			File.Create(file.FullName).Close();

			File.WriteAllBytes(file.FullName, content);
			AssetDatabase.Refresh();
			var newDatabase = ScriptableObject.CreateInstance<PunDatabase>();
			string filePath = GetRelativePath("Resources/PunsGenerator/PunCsv.csv");
			string[] lines = File.ReadAllLines(filePath);
			for (int i = 1; i < lines.Length; i++)
			{
				string[] data = lines[i].Split(",");
				string subject = data[0];
				string verb = data[1];
				string _object = data[2];
				if (subject != "")
				{
					newDatabase.subjects.Add(subject);
				}
				if (verb != "")
				{
					newDatabase.verbs.Add(verb);
				}
				if (_object != "")
				{
					newDatabase.objects.Add(_object);
				}
			}
			AssetDatabase.CreateAsset(newDatabase, GetRelativePath("Resources/PunDatabase.asset"));
			AssetDatabase.Refresh();
		}

		/// <summary>
		/// Shows a notification message in the given type of window.
		/// </summary>
		/// <param name="window">The type of the window where the notification should be shown.</param>
		/// <param name="message">The message to display in the notification.</param>
		/// <param name="fadeTime">The duration in seconds for the notification to fade out. Default value is 2 seconds.</param>
		private static void Notify(Type window, string message, float fadeTime = 2f)
		{
			var content = new GUIContent(message);
			EditorWindow.GetWindow(window).ShowNotification(content, fadeTime);
		}

		/// <summary>
		/// Notifies the active scene view with a message.
		/// </summary>
		/// <param name="message">The message to display.</param>
		/// <param name="fadeTime">Optional. The time it takes for the message to fade out. Default is 2 seconds.</param>
		private static void NotifyInScene(string message, float fadeTime = 2f)
		{
			Notify(SceneView.lastActiveSceneView.GetType(), message, fadeTime);
		}

		public static void NotifyInGame(string message, float fadeTime = 2f)
		{
			Type gameView = typeof(EditorWindow).Assembly.GetType("UnityEditor.GameView");
			Notify(gameView, message, fadeTime);
		}
	}

}