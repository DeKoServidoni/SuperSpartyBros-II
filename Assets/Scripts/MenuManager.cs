using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

	[SerializeField] private string nextLevelScene;
	[SerializeField] private string mainMenuScene;
	[SerializeField] private Text instructions;

	private string currentScene;

    private void Awake() {
        currentScene = SceneManager.GetActiveScene().name;
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
		SceneManager.LoadScene(nextLevelScene);
	}
}
