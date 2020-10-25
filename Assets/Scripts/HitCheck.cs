using UnityEngine;

public class HitCheck : MonoBehaviour {

	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.CompareTag("Player")) {
			var enemyController = GetComponentInParent<Enemy2>();

			if (!enemyController) {
				Debug.LogError("Missing parent enemy controller!");
				return;
			}

			enemyController.Hit(2);
			
		}
	}
}
