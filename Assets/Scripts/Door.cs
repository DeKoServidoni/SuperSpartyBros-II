using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

	private GameManager2 gameManager = null;

    void Start() {
		gameManager = GameManager2.gm;

		if (!gameManager)
			Debug.LogError("Missing game manager instance!");
    }

    void OnTriggerEnter2D (Collider2D other) {
		if (other.CompareTag("Player")) {
			Destroy(other.gameObject);
			gameManager.LevelComplete();
		}
	}
}
