#if UNITY_EDITOR

using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using XomracLabs;

public class TitlesGetter : MonoBehaviour
{
    [FormerlySerializedAs("textAsset")] [SerializeField] private TextAsset titles;
    [SerializeField] private TextAsset punchlines;
    [SerializeField] private PunDatabase punDatabase;

    private void Start()
    {
        var newText = this.titles.text;
        
        
        
        
        var titles = newText.Split("\n");
        punDatabase.titles = new List<string>(titles.ToList());
        
        
        var punchlines = this.punchlines.text.Split("\n");
        punDatabase.punchlines = new List<string>(punchlines.ToList());
        EditorUtility.SetDirty(punDatabase);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

}
#endif