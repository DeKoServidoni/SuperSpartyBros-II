using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

	[SerializeField] private string nextLevelScene;
	[SerializeField] private string mainMenuScene;

	private string currentScene;

    private void Awake() {
        currentScene = SceneManager.GetActiveScene().name;
    }

    public void OnExitClick() {
		SceneManager.LoadScene(mainMenuScene);
	}

	public void OnRestartClick() {
		SceneManager.LoadScene(currentScene);
	}

	public void OnNextLevelClick() {
		SceneManager.LoadScene(nextLevelScene);
	}
}
