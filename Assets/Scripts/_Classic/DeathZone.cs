using UnityEngine;

public class DeathZone : MonoBehaviour {

	public bool destroyNonPlayerObjects = true;

	void OnCollisionEnter2D (Collision2D other) {
		if (other.gameObject.tag == "Player") {
			other.gameObject.GetComponent<CharacterController2D>().FallDeath ();
		} else if (destroyNonPlayerObjects) {
			Destroy(other.gameObject);
		}
	}
}
