using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UI_Percentage : MonoBehaviour {

    public GameObject player1, player2, player3, player4;
    public Text p1HpTxt, p2HpTxt, p3HpTxt, p4HpTxt;
    float p1HP, p2HP, p3HP, p4HP;
	
	// Update is called once per frame
	void Update ()
    {
        p1HP = player1.GetComponent<Platformer2DUserControl>().currentHealthPercent;
        p2HP = player2.GetComponent<Platformer2DUserControl>().currentHealthPercent;
        p3HP = player3.GetComponent<Platformer2DUserControl>().currentHealthPercent;
        p4HP = player4.GetComponent<Platformer2DUserControl>().currentHealthPercent;

        UpdatePercentage();
    }

    void UpdatePercentage()
    {
        p1HpTxt.text = p1HP + "%";
        p2HpTxt.text = p2HP + "%";
        p3HpTxt.text = p3HP + "%";
        p4HpTxt.text = p4HP + "%";

    }
}
