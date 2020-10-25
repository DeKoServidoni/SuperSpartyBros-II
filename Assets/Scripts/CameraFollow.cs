using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    [SerializeField] private Transform target = null;
    [SerializeField] private Vector3 offset = Vector3.zero;

    private void Awake() {
        if (!target)
            Debug.LogError("Missing target!");
    }

    private void Update() {

        if (transform.position.x < 81f) {
            Vector3 position = transform.position;
            position.x = (target.position + offset).x;
            transform.position = position;
        }
    }
}