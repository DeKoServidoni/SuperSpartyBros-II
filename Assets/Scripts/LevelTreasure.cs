using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTreasure : MonoBehaviour {

	private FinalBossManager finalBossManager = null;
	private SpriteRenderer spriteRenderer = null;

    void Awake() {
        finalBossManager = GetComponentInParent<FinalBossManager>();
		spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D (Collider2D other) {
		if (other.CompareTag("Player")) {
			StartEndGame();
		}
	}

	void StartEndGame() {
		StartCoroutine(Dissolve());
		PlayerPrefManager.CleanAlreadyTalkedFlag();
		StartCoroutine(finalBossManager.RunTalk(false));
	}

	IEnumerator Dissolve() {
		var fade = spriteRenderer.material.GetFloat("_Fade");

		while (fade > 0) {
			spriteRenderer.material.SetFloat("_Fade", fade -= 0.1f);
			yield return new WaitForSeconds(1f) ;
		}
    }
}
