using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpDestroy : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
        }

        if(collision.gameObject.tag=="DeathWall")
        {
            Destroy(this.gameObject);
        }
    }


}
