using UnityEngine;
using System.Collections;

public class CameraManager2 : MonoBehaviour {

    private AudioSource audioSource = null;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();

        if(!audioSource)
            Debug.LogError("Missing audio source!");
    }

    public void StopBackgroundMusic() {
        audioSource.Stop();
    }
}
