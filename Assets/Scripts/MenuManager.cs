using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

	[SerializeField] private string nextLevelScene;
	[SerializeField] private string mainMenuScene;
	[SerializeField] private Text instructions;
	[SerializeField] private LevelLoader levelLoader;

	private string currentScene;

    private void Awake() {
        currentScene = SceneManager.GetActiveScene().name;

		if (!levelLoader)
			Debug.LogError("Missing level loader!");
    }

    private void Start() {
		if (instructions)
			StartCoroutine(DismissInstructions());
    }

	IEnumerator DismissInstructions() {
		yield return new WaitForSeconds(5f) ;
		Destroy(instructions);
    }

    public void OnExitClick() {
		SceneManager.LoadScene(mainMenuScene);
	}

	public void OnRestartClick() {
		SceneManager.LoadScene(currentScene);
	}

	public void OnNextLevelClick() {
		levelLoader.LoadNextScene(nextLevelScene);
	}
}
