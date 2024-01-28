

namespace XomracLabs
{
	using System.Collections.Generic;
	using UnityEngine;

	[CreateAssetMenu(fileName = "new pun database", menuName = "Data/Totti tool/new pun database", order = 0)]
	public class PunDatabase : ScriptableObject
	{
		public List<string> subjects = new List<string>();
		public List<string> verbs = new List<string>();
        public List<string> objects = new List<string>();
        public List<string> punchlines = new List<string>();
        public List<string> titles = new List<string>();
	}

}