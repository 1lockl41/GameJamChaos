using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawner : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;
    public GameObject player3;
    public GameObject player4;

    public float respawnTime = 5.0f;

    float player1respawnTimer = 0.0f;
    float player2respawnTimer = 0.0f;
    float player3respawnTimer = 0.0f;
    float player4respawnTimer = 0.0f;

    public GameObject[] allRespawnPoints;

    public AudioClip respawnSound;
    AudioSource audioSource;

    private void Start()
    {
        allRespawnPoints = GameObject.FindGameObjectsWithTag("RespawnPoint");
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(!player1.activeInHierarchy)
        {
            player1respawnTimer += Time.deltaTime;
            if(player1respawnTimer > respawnTime)
            {
                player1.transform.position = SelectRandomRespawn().transform.position;
                player1.SetActive(true);
                player1respawnTimer = 0.0f;
                player1.GetComponent<Platformer2DUserControl>().RespawnPlayer();
                audioSource.Stop();
                audioSource.PlayOneShot(respawnSound);
            }
        }

        if (!player2.activeInHierarchy)
        {
            player2respawnTimer += Time.deltaTime;
            if (player2respawnTimer > respawnTime)
            {
                player2.transform.position = SelectRandomRespawn().transform.position;
                player2.SetActive(true);
                player2respawnTimer = 0.0f;
                player2.GetComponent<Platformer2DUserControl>().RespawnPlayer();
                audioSource.Stop();
                audioSource.PlayOneShot(respawnSound);
            }
        }

        if (!player3.activeInHierarchy)
        {
            player3respawnTimer += Time.deltaTime;
            if (player3respawnTimer > respawnTime)
            {
                player3.transform.position = SelectRandomRespawn().transform.position;
                player3.SetActive(true);
                player3respawnTimer = 0.0f;
                player3.GetComponent<Platformer2DUserControl>().RespawnPlayer();
                audioSource.Stop();
                audioSource.PlayOneShot(respawnSound);
            }
        }

        if (!player4.activeInHierarchy)
        {
            player4respawnTimer += Time.deltaTime;
            if (player4respawnTimer > respawnTime)
            {
                player4.transform.position = SelectRandomRespawn().transform.position;
                player4.SetActive(true);
                player4respawnTimer = 0.0f;
                player4.GetComponent<Platformer2DUserControl>().RespawnPlayer();
                audioSource.Stop();
                audioSource.PlayOneShot(respawnSound);
            }
        }
    }


    GameObject SelectRandomRespawn()
    {
        return allRespawnPoints[Random.Range(0, allRespawnPoints.Length)];
    }
}
