using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour {
	[SerializeField] private GameObject explosion;

	void OnTriggerEnter2D (Collider2D other) {
		if (other.CompareTag("Player")) {
			if (explosion)
				Instantiate(explosion,transform.position,transform.rotation);

			other.gameObject.GetComponent<CharacterController2D>().CollectPotion();
			Destroy(gameObject);
		}
	}
}
