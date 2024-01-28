
using System;

using System.Collections.Generic;


namespace XomracLabs
{
	
	public static class BookManager
	{
	
		
		public static int maxPages=5;

		public static List<string> puns = new();

		public static Action OnBookFinished;


		public static bool HasFinished => maxPages == puns.Count;
		
		
		
		public static void AddPun(string newPun)
		{
			puns.Add(newPun);
		}
	}

}
