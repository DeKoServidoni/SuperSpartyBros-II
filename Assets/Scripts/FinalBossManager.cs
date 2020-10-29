using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class FinalBossManager : MonoBehaviour {

    [SerializeField] private GameObject[] initialTalk;
    [SerializeField] private GameObject[] endTalk;
    [SerializeField] private GameObject boss;
    [SerializeField] private GameObject bossFirstForm;
    [SerializeField] private AudioClip talkSFX;

    private AudioSource audioSource = null;
    private GameManager2 gameManager = null;

    void Awake() {
        audioSource = GetComponent<AudioSource>();
        if (!audioSource)
            Debug.LogError("Missing audio source!");
    }

    void Start() {
        gameManager = GameManager2.gm;
        if (!gameManager)
            Debug.LogError("Missing game manager instance!");
    }

    public IEnumerator RunTalk(bool initial) {
        GameObject previous = null;
        var conversation = initial ? initialTalk : endTalk;

        gameManager.EnablePlayer(false);

        if (!PlayerPrefManager.IsTalked()) {

            foreach (GameObject talk in conversation) {
                if (previous != null)
                    previous.SetActive(false);

                talk.SetActive(true);
                audioSource.PlayOneShot(talkSFX);
                previous = talk;

                yield return new WaitForSeconds(7f);
            }

            previous.SetActive(false);

            PlayerPrefManager.SetAlreadyTalkedFlag();
        }

        if (initial) StartFinalBattle();
        else gameManager.LevelComplete();
    }

    private void StartFinalBattle() {
        gameManager.StartBossBattle();
        gameManager.EnablePlayer(true);
        boss.SetActive(false);
        bossFirstForm.SetActive(true);
    }
}
