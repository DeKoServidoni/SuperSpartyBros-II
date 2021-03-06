﻿using UnityEngine;
using System.Collections;

public class Enemy2 : MonoBehaviour {

	[SerializeField] [Range(0.0f, 10.0f)] private float moveSpeed = 4f;
	[SerializeField] private float damageAmount = 5.0f;
	[SerializeField] private int enemyHealth = 1;
	[SerializeField] private AudioClip hitSFX;
	[SerializeField] private ParticleSystem dustEffect = null;
	[SerializeField] private GameObject hitCheck = null;
	[SerializeField] private bool isBoss = false;

    private GameObject[] myWaypoints = null; 
	private float waitAtWaypointTime = 1f;   
	private bool loopWaypoints = true;
	private int myWaypointIndex = 0;
	private int deathLayer = -1;

	private Rigidbody2D rigidBody;
	private Animator animator;
	private AudioSource[] audioSources;
	SpriteRenderer spriteRenderer = null;

	private float moveTime;
	private float direction = 0.0f;
	private bool moving = true;
	private bool dying = false;

	void Awake() {
		rigidBody = GetComponent<Rigidbody2D> ();
		if (!rigidBody)
			Debug.LogError("Rigidbody2D component missing from this gameobject");
		
		animator = GetComponent<Animator>();
		if (!animator)
			Debug.LogError("Animator component missing from this gameobject");
		
		audioSources = GetComponents<AudioSource> ();
		if (audioSources == null || audioSources.Length == 0)
			Debug.LogWarning("AudioSource component missing from this gameobject. Adding one.");
	
		moveTime = 0f;
		moving = true;

		deathLayer = LayerMask.NameToLayer("Death");
		Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), deathLayer, true);
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

    void Update() {
		if (Time.time >= moveTime) EnemyMovement();
		else {
			animator.SetBool("Moving", false);
			PauseWalkSFX();
		}
    }

    private void OnTriggerEnter2D(Collider2D other) {
		if (dying) return;

        if (other.CompareTag("Player")) {
			Flip(other.transform.position.x-transform.position.x);
			other.GetComponent<CharacterController2D>().ApplyDamage(100);
			rigidBody.isKinematic = true;
		}
    }

    private void OnTriggerStay2D(Collider2D other) {
		if (dying) return;

        if (other.CompareTag("Player")) {
			rigidBody.isKinematic = true;
		}
    }

    private void OnTriggerExit2D(Collider2D other) {
		if (dying) return;

        if (other.CompareTag("Player")) {
			rigidBody.isKinematic = false;
		}
    }

    private void EnemyMovement() {
		if (myWaypoints != null && myWaypoints.Length != 0 && moving) {
			
			Flip (direction);
			
			direction = myWaypoints[myWaypointIndex].transform.position.x- transform.position.x;
			
			if (Mathf.Abs(direction) <= 0.5f) {
				rigidBody.velocity = new Vector2(0, 0);
				myWaypointIndex++;
				
				if(myWaypointIndex >= myWaypoints.Length) {
					if (loopWaypoints) myWaypointIndex = 0;
					else moving = false;
				}
				
				moveTime = Time.time + waitAtWaypointTime;
			} else {
				animator.SetBool("Moving", true);
				rigidBody.velocity = new Vector2(transform.localScale.x * moveSpeed, rigidBody.velocity.y);

				PlayWalkSFX();
				dustEffect.Play();
			}
		}
	}

	private void Flip(float direction) {
		Vector3 localScale = transform.localScale;
		
		if ((direction>0f)&&(localScale.x<0f))
			localScale.x*=-1;
		else if ((direction<0f)&&(localScale.x>0f))
			localScale.x*=-1;
		
		transform.localScale = localScale;
	}

	private void PlayWalkSFX() {
        audioSources[1].volume = 0.1f;

        if (!audioSources[1].isPlaying)
            audioSources[1].Play();
    }

	private void PauseWalkSFX() {
        if (audioSources[1].isPlaying)
            audioSources[1].Pause();
    }

	private void StopWalkSFX() {
		audioSources[1].volume = 0.0f;
		audioSources[1].Stop();
	}

	IEnumerator KillEnemy() {
		dying = true;
		moving = false;
		animator.SetBool("Moving", false);

		dustEffect.Stop();
		rigidBody.velocity = new Vector2(0, 0);

		Destroy(hitCheck);

		var fade = spriteRenderer.material.GetFloat("_Fade");

		while (fade > 0) {
			spriteRenderer.material.SetFloat("_Fade", fade -= 0.1f);
			yield return new WaitForSeconds(0.2f) ;
		}

		Destroy(gameObject);
    }

	public void SetupPatrol(GameObject[] myWaypoints, float waitAtWaypointTime, int myWaypointIndex) {
		this.myWaypoints = myWaypoints;
		this.waitAtWaypointTime = waitAtWaypointTime;
		this.myWaypointIndex = myWaypointIndex;
	}

	public void Hit(int damage) {
		animator.SetTrigger("Hit");
		audioSources[0].PlayOneShot(hitSFX);
		enemyHealth -= damage;

		if (enemyHealth <= 0) {
			dying = true;
			StartCoroutine(KillEnemy());
			if (isBoss) {
				GameManager2.gm.EndBossBattle();
			}
        }
	}

	public float GetDamage() {
		return damageAmount;
	}
}
