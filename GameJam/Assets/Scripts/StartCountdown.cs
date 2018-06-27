using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCountdown : MonoBehaviour {

    public GameObject Countdown;

	// Use this for initialization
	void Start () {

        StartCoroutine("StartCountDown");

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator StartCountDown()
    {
        Time.timeScale = 0;

        float pauseTime = Time.realtimeSinceStartup + 5f;

        while(Time.realtimeSinceStartup < pauseTime)
        {
            yield return 0;
        }

        Countdown.gameObject.SetActive(false);
        Time.timeScale = 1;
    }
}
