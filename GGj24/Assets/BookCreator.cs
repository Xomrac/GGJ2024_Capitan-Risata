using UnityEngine;

namespace XomracLabs
{

	public class BookCreator : MonoBehaviour
	{
		public static string GetSavePath()
		{
			var savePath = "";
			return savePath;
		}

		public void CreatePDF()
		{
			GameEvents.OnBookCreated?.Invoke();
		}
	}

}