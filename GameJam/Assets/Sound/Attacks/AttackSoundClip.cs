using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSoundClip : MonoBehaviour
{
    public AudioClip soundClip;
    AudioSource audioSource;

	// Use this for initialization
	void Awake ()
    {
        audioSource = gameObject.transform.parent.GetComponent<AudioSource>();

    }
	
	void OnEnable()
    {
        audioSource.PlayOneShot(soundClip);
    }
}
