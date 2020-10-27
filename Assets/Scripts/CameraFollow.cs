using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    [SerializeField] private Transform target = null;
    [SerializeField] private Vector3 offset = Vector3.zero;
    [SerializeField] private float cameraLimit = 0.0f;

    private void Awake() {
        if (!target)
            Debug.LogError("Missing target!");
    }

    private void Update() {
        if (target) {
            Vector3 position = transform.position;
            position.x = (target.position + offset).x;

            if (transform.position.x < cameraLimit || position.x < cameraLimit)
                transform.position = position;
        }
    }
}