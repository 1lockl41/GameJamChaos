using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathWall : MonoBehaviour {

    public AudioClip deathSound;
    AudioSource source;

    void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "Player" && !collision.isTrigger)
        {
            collision.GetComponent<Platformer2DUserControl>().KillPlayer();
            source.Stop();
            source.PlayOneShot(deathSound);

            if (collision.GetComponent<Platformer2DUserControl>().lastHitBy != null)
            {
                collision.GetComponent<Platformer2DUserControl>().lastHitBy.GetComponent<Platformer2DUserControl>().score++;
            }
        }
    }
}
