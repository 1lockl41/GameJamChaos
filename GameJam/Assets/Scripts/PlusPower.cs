using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlusPower : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
            GameController.powerCharge = GameController.powerCharge + 10;
        }

        if (collision.gameObject.tag == "Death")
        {
            Destroy(this.gameObject);

        }
    }
}