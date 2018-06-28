using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawn : MonoBehaviour {

    public int Powerupchosen;

    public GameObject[] PowerUpPoints;

    public GameObject IncreaseBuff;

    public GameObject DecreaseBuff;

	// Use this for initialization
	void Start () {

        PowerUpPoints = GameObject.FindGameObjectsWithTag("PowerUpPoint");

        InvokeRepeating("RunPowerups", 15.0f, 10.0f);

	}
	
    public void ChoosePowerUp()
    {

        Powerupchosen = Random.Range(1, 2);

    }

    GameObject ChooseSpawnLoc()
    {
        return PowerUpPoints[Random.Range(0, PowerUpPoints.Length)];
    }

    public void RunPowerups()
    {
        ChoosePowerUp();
        ChooseSpawnLoc();

        if (Powerupchosen == 1)
        {
            Instantiate(IncreaseBuff, ChooseSpawnLoc().transform.position, ChooseSpawnLoc().transform.rotation);
        }

        if(Powerupchosen == 2)
        {
            Instantiate(DecreaseBuff, ChooseSpawnLoc().transform.position, ChooseSpawnLoc().transform.rotation);
        }
    }
}
