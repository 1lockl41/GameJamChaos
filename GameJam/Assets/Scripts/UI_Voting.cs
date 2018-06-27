using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Voting : MonoBehaviour {

    public Text Red_Txt, Blue_Txt, Green_Txt, Yellow_Txt;
    private bool Choice_Red, Choice_Blue, Choice_Green, Choice_Yellow;
    public int Red, Blue, Green, Yellow;
    public int Red_t, Blue_t, Yellow_t, Green_t;
    
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        VotePressed();
        Vote_Count();
	}

    void VotePressed()
    {
        if(Choice_Red == false && Input.GetKey("up"))
        {
            Red_t = Red;
            Green = Green_t;
            Yellow = Yellow_t;
            Blue = Blue_t;
            Red += 1;
            Choice_Red = true;
            Choice_Blue = false;
            Choice_Green = false;
            Choice_Yellow = false;
        }
        if (Choice_Green == false && Input.GetKey("down"))
        {
            Green_t = Green;
            Yellow = Yellow_t;
            Blue = Blue_t;
            Red = Red_t;
            Green += 1;
            Choice_Green = true;
            Choice_Blue = false;
            Choice_Yellow = false;
            Choice_Red = false;
        }
        if (Choice_Yellow == false && Input.GetKey("left"))
        {
            Yellow_t = Yellow;
            Green = Green_t;
            Red = Red_t;
            Blue = Blue_t;
            Yellow += 1;
            Choice_Yellow = true;
            Choice_Blue = false;
            Choice_Green = false;
            Choice_Red = false;
        }
        if (Choice_Blue == false && Input.GetKey("right"))
        {
            Blue_t = Blue;
            Green = Green_t;
            Red = Red_t;
            Yellow = Yellow_t;
            Blue += 1;
            Choice_Blue = true;
            Choice_Green = false;
            Choice_Yellow = false;
            Choice_Red = false;
        }
    }

    void Vote_Count()
    {
        Red_Txt.text = "" + Red;
        Blue_Txt.text = "" + Blue;
        Green_Txt.text = "" + Green;
        Yellow_Txt.text = "" + Yellow;
    }
}
