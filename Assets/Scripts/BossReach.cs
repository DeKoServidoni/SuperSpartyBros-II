using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossReach : MonoBehaviour {

	[SerializeField] private bool isFinalBoss = false;

    private GameManager2 gameManager = null;
	private FinalBossManager finalBossManager = null;

    void Awake() {
        finalBossManager = GetComponentInParent<FinalBossManager>();
    }

    void Start() {
		gameManager = GameManager2.gm;
		if (!gameManager)
			Debug.LogError("Missing game manager instance!");
    }

    void OnTriggerEnter2D (Collider2D other) {
		if (other.CompareTag("Player")) {

			if (isFinalBoss) StartFinalBossBattle();
			else gameManager.StartBossBattle();
		}
	}

	void StartFinalBossBattle() {
		GetComponent<BoxCollider2D>().enabled = false;
		StartCoroutine(finalBossManager.RunTalk(false));
	}
}
