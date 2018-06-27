using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathWall : MonoBehaviour {
    

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "Player" && !collision.isTrigger)
        {
            collision.GetComponent<UnityStandardAssets._2D.Platformer2DUserControl>().KillPlayer();
        }
    }
}
