using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager2 : MonoBehaviour {

	public static GameManager2 gm;

	[SerializeField] private int score = 0;
	[SerializeField] private int startLives = 3;
	[SerializeField] private Text UIScore;
	[SerializeField] private Text UILevel;
	[SerializeField] private GameObject[] UILives;
	[SerializeField] private GameObject UIGamePaused;
	[SerializeField] private GameObject UIGameOver;
	[SerializeField] private GameObject UIVictory;
	[SerializeField] private GameObject UIDied;
	[SerializeField] private GameObject bossTreasure;

	private GameObject player;
	private Scene scene;
	private AudioSource[] audioSources;
	private bool gameOver = false;

	private int lives = 0;
	private int highScore = 0;

	void Awake () {
		if (bossTreasure)
			bossTreasure.SetActive(false);

		audioSources = GetComponents<AudioSource>();
		if (audioSources == null)
			Debug.LogError("Missing audio source!");

		if (gm == null)
			gm = GetComponent<GameManager2>();

		SetupDefaults();
		RefreshPlayerState();
		RefreshGUI();
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			if (Time.timeScale > 0f) {
				UIGamePaused.SetActive(true);
				Time.timeScale = 0f;
			} else {
				Time.timeScale = 1f;
				UIGamePaused.SetActive(false);
			}
		}
	}

	void SetupDefaults() {
		if (player == null)
			player = GameObject.FindGameObjectWithTag("Player");
		
		if (!player)
			Debug.LogError("Player not found in Game Manager");

		scene = SceneManager.GetActiveScene();

		if (PlayerPrefManager.GetLives() == 0) lives = startLives;
		else lives = PlayerPrefManager.GetLives();

		if (UIScore == null)
			Debug.LogError ("Need to set UIScore on Game Manager.");
	
		if (UILevel == null)
			Debug.LogError ("Need to set UILevel on Game Manager.");
		
		if (UIGamePaused == null)
			Debug.LogError ("Need to set UIGamePaused on Game Manager.");
	}

	void RefreshPlayerState() {
		score = PlayerPrefManager.GetScore();
		PlayerPrefManager.UnlockLevel();
	}

	void RefreshGUI() {
		UIGamePaused.SetActive(false);
		UIGameOver.SetActive(false);
		UIVictory.SetActive(false);
		UIDied.SetActive(false);

		UIScore.text = score.ToString();
		UILevel.text = scene.name;

		RefreshLives();
	}

	void RefreshLives() {
		if (lives == 1) audioSources[0].Play();
		else audioSources[0].Stop();

		for(int i=0; i<UILives.Length; i++) {
			UILives[i].SetActive(i<lives);
		}
	}

	void PlayLevelCompleteMusic() {
		Camera.main.GetComponent<CameraManager2>().StopBackgroundMusic();
		audioSources[0].Stop();
		if(audioSources.Length >= 4) audioSources[3].Stop();
		audioSources[2].Play();
	}

	void PlayGameOverMusic() {
		Camera.main.GetComponent<CameraManager2>().StopBackgroundMusic();
		audioSources[0].Stop();
		if(audioSources.Length >= 4) audioSources[3].Stop();
		audioSources[1].Play();
	}

	void PlayBossMusic() {
		Camera.main.GetComponent<CameraManager2>().StopBackgroundMusic();
		audioSources[0].Stop();
		audioSources[3].Play();
	}

	public void AddPoints(int amount) {
		score += amount;
		UIScore.text = score.ToString();

		if (score > highScore)
			highScore = score;
	}

	public void AddLife() {
		if (lives == startLives)
			return;

		lives += 1;
		RefreshLives();
	}

	public void YouDied() {
		UIDied.SetActive(true);
	}

    public void ResetGame() {
        lives--;
        RefreshGUI();

		if (lives <= 0) {
			PlayerPrefManager.SavePlayerState(score, highScore, startLives);
			UIGameOver.SetActive(true);
			PlayGameOverMusic();
			gameOver = true;
		} else {
			PlayerPrefManager.SavePlayerState(0, 0, lives);
			SceneManager.LoadScene(scene.name); 
		}
    }

    public void LevelComplete() {
		PlayerPrefManager.SavePlayerState(score, highScore, lives);
		PlayLevelCompleteMusic();
		UIVictory.SetActive(true);
	}

	public void EndBossBattle() {
		if (bossTreasure) {
			audioSources[3].Stop();
			bossTreasure.SetActive(true);
		}
	}

	public void StartBossBattle() {
		PlayBossMusic();
	}

	public bool IsGameOver() {
		return gameOver;
	}

	public int GetStartLives() {
		return startLives;
	}

	public void EnablePlayer(bool enable) {
		var controller = player.GetComponent<CharacterController2D>();

		if (!enable) controller.FreezeMotion();
		else controller.UnFreezeMotion();
	}
}
