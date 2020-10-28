using UnityEngine;

public class Patrol : MonoBehaviour {

	[SerializeField] private GameObject enemy = null;
    [SerializeField] private GameObject[] myWaypoints; 
	[SerializeField] private float waitAtWaypointTime = 1f;   
	[SerializeField] private int myWaypointIndex = 0;

	void Awake() {
		if(!enemy)
			Debug.LogError("Missing enemy!");
	}

    void Start() {
		var newEnemy = Instantiate(enemy,
                new Vector3(transform.position.x, transform.position.y, 0.0f),
                Quaternion.identity);

		newEnemy.transform.parent = transform;
		newEnemy.GetComponent<Enemy2>()
			.SetupPatrol(myWaypoints, waitAtWaypointTime, myWaypointIndex);
    }
}
