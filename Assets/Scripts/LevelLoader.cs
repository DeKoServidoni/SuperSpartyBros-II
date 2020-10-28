using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {

    [SerializeField] private Animator transition;
    [SerializeField] private bool isIntro = false;

    private void Start() {
        if(isIntro)
            StartCoroutine(LoadGame());
    }

    public void LoadNextScene(string scene) {
        StartCoroutine(LoadLevel(scene));
    }

    IEnumerator LoadLevel(string scene) {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(scene);
    }

    IEnumerator LoadGame() {
        yield return new WaitForSeconds(40f);
        SceneManager.LoadScene("Level 1");
    }
}
