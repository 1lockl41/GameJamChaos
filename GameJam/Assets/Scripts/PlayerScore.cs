using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScore : MonoBehaviour {

    public static int PlayerOneScore;
    public static int PlayerTwoScore;
    public static int PlayerThreeScore;
    public static int PlayerFourScore;

    public Text P1TextScore;
    public Text P2TextScore;
    public Text P3TextScore;
    public Text P4TextScore;

    public Platformer2DUserControl player1;
    public Platformer2DUserControl player2;
    public Platformer2DUserControl player3;
    public Platformer2DUserControl player4;

    // Use this for initialization
    void Start () {

        PlayerOneScore = 0;
        PlayerTwoScore = 0;
        PlayerThreeScore = 0;
        PlayerFourScore = 0;

    }
	
	// Update is called once per frame
	void Update () {

        


        P1TextScore.text = "Score:          " + player1.score;
        P2TextScore.text = "Score:          " + player2.score;
        P3TextScore.text = "Score:          " + player3.score;
        P4TextScore.text = "Score:          " + player4.score;
    }

}
