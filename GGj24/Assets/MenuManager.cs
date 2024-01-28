using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private string playSceneName;

    
    
    private void Start()
	{
		playButton.onClick.AddListener(PlayButtonClicked);
		quitButton.onClick.AddListener(QuitButtonClicked);
	}
	private void PlayButtonClicked()
	{
		SceneManager.LoadScene(playSceneName);
	}
	private void QuitButtonClicked()
	{
		Application.Quit();
	}

    
    
}
