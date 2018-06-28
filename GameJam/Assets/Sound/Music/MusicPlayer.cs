using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour {

    public AudioClip intro1;
    public AudioClip intro2;
    public AudioClip loop;

    AudioSource audioSource;

    bool hasPlayedIntro1 = false;
    bool hasPlayedIntro2 = false;


	// Use this for initialization
	void Start ()
    {
        audioSource = GetComponent<AudioSource>();

        audioSource.PlayOneShot(intro1);

    }
	
	// Update is called once per frame
	void Update ()
    {
		if(!hasPlayedIntro1)
        {
            if(!audioSource.isPlaying)
            {
                hasPlayedIntro1 = true;
                audioSource.PlayOneShot(intro2);
            }
        }
        else if(!hasPlayedIntro2)
        {
            if(!audioSource.isPlaying)
            {
                hasPlayedIntro2 = true;
                audioSource.loop = true;
                audioSource.clip = loop;
                audioSource.Play();
            }
        }
	}
}
