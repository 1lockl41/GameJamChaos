using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Voting : MonoBehaviour {

    public Text Red_Txt, Blue_Txt, Green_Txt, Yellow_Txt;
    private bool Choice_Red, Choice_Blue, Choice_Green, Choice_Yellow;
    private bool Choice_Red2, Choice_Blue2, Choice_Green2, Choice_Yellow2;
    public int Red, Blue, Green, Yellow;
    private int Red_t, Blue_t, Yellow_t, Green_t;
    private int Red_t2, Blue_t2, Yellow_t2, Green_t2;


    //   // Use this for initialization
    //   void Start () {

    //}

    //// Update is called once per frame
    //void Update () {
    //       VotePressed();
    //       Vote_Count();
    //}

    //   void VotePressed()
    //   {
    //       if(Choice_Red == false && Input.GetKey("up"))
    //       {
    //           Red_t = Red;
    //           Green = Green_t;
    //           Yellow = Yellow_t;
    //           Blue = Blue_t;
    //           Red += 1;
    //           Choice_Red = true;
    //           Choice_Blue = false;
    //           Choice_Green = false;
    //           Choice_Yellow = false;
    //       }
    //       if (Choice_Green == false && Input.GetKey("down"))
    //       {
    //           Green_t = Green;
    //           Yellow = Yellow_t;
    //           Blue = Blue_t;
    //           Red = Red_t;
    //           Green += 1;
    //           Choice_Green = true;
    //           Choice_Blue = false;
    //           Choice_Yellow = false;
    //           Choice_Red = false;
    //       }
    //       if (Choice_Yellow == false && Input.GetKey("left"))
    //       {
    //           Yellow_t = Yellow;
    //           Green = Green_t;
    //           Red = Red_t;
    //           Blue = Blue_t;
    //           Yellow += 1;
    //           Choice_Yellow = true;
    //           Choice_Blue = false;
    //           Choice_Green = false;
    //           Choice_Red = false;
    //       }
    //       if (Choice_Blue == false && Input.GetKey("right"))
    //       {
    //           Blue_t = Blue;
    //           Green = Green_t;
    //           Red = Red_t;
    //           Yellow = Yellow_t;
    //           Blue += 1;
    //           Choice_Blue = true;
    //           Choice_Green = false;
    //           Choice_Yellow = false;
    //           Choice_Red = false;
    //       }
    //   }

    void Vote_Count()
    {
        Red_Txt.text = "" + Red;
        Blue_Txt.text = "" + Blue;
        Green_Txt.text = "" + Green;
        Yellow_Txt.text = "" + Yellow;
    }


    public string player1AxisVertical;
    public string player1AxisHorizontal;

    public string player2AxisVertical;
    public string player2AxisHorizontal;

    private bool p1votedR, p1votedG, p1votedB, p1votedY;
    private bool p2votedR, p2votedG, p2votedB, p2votedY;

    private void Update()
    {
        if(Choice_Blue == false && UnityStandardAssets.CrossPlatformInput.CrossPlatformInputManager.GetAxisRaw(player1AxisHorizontal) == 1)
        {
            Debug.Log("player 1 right");

            if (p1votedR == true)
            {
                Red -= 1;

            }

            if (p1votedG == true)
                Green -= 1;

            if (p1votedY == true)
                Yellow -= 1;

            Blue += 1;
            p1votedB = true;


            Choice_Blue = true;
            Choice_Green = false;
            Choice_Yellow = false;
            Choice_Red = false;

            p1votedR = false;
            p1votedY = false;
            p1votedG = false;

        }
        else if(Choice_Yellow == false && UnityStandardAssets.CrossPlatformInput.CrossPlatformInputManager.GetAxisRaw(player1AxisHorizontal) == -1)
        {
            Debug.Log("player 1 left");

            if (p1votedB == true)
                Blue -= 1;

            else if (p1votedG == true)
                Green -= 1;

            else if (p1votedR == true)
                Red -= 1;

            Yellow += 1;
            p1votedY = true;


            Choice_Yellow = true;
            Choice_Blue = false;
            Choice_Green = false;
            Choice_Red = false;

            p1votedG = false;
            p1votedR = false;
            p1votedB = false;
        }
        else if (Choice_Red == false && UnityStandardAssets.CrossPlatformInput.CrossPlatformInputManager.GetAxisRaw(player1AxisVertical) == 1)
        {
            Debug.Log("player 1 up");

            if (p1votedB == true)
                Blue -= 1;

            if (p1votedG == true)
                Green -= 1;

            if (p1votedY == true)
                Yellow -= 1;

            Red += 1;
            p1votedR = true;

            Choice_Red = true;
            Choice_Blue = false;
            Choice_Green = false;
            Choice_Yellow = false;

            p1votedB = false;
            p1votedG = false;
            p1votedY = false;
        }
        else if (Choice_Green == false && UnityStandardAssets.CrossPlatformInput.CrossPlatformInputManager.GetAxisRaw(player1AxisVertical) == -1)
        {
            Debug.Log("player 1 down");

            if (p1votedB == true)
                Blue -= 1;

            if (p1votedY == true)
                Yellow -= 1;

            if (p1votedR == true)
                Red -= 1;

            Green += 1;
            p1votedG = true;

            Choice_Green = true;
            Choice_Blue = false;
            Choice_Yellow = false;
            Choice_Red = false;

            p1votedY = false;
            p1votedB = false;
            p1votedR = false;
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        else if (Choice_Blue2 == false && UnityStandardAssets.CrossPlatformInput.CrossPlatformInputManager.GetAxisRaw(player2AxisHorizontal) == 1)
        {
            Debug.Log("player 2 right");

            if (p2votedR == true)
                Red -= 1;

            if (p2votedG == true)
                Green -= 1;

            if (p2votedY == true)
                Yellow -= 1;
            
            Blue += 1;
            p2votedB = true;

            Choice_Blue2 = true;
            Choice_Green2 = false;
            Choice_Yellow2 = false;
            Choice_Red2 = false;

            p2votedR = false;
            p2votedY = false;
            p2votedG = false;
        }
        else if (Choice_Yellow2 == false && UnityStandardAssets.CrossPlatformInput.CrossPlatformInputManager.GetAxisRaw(player2AxisHorizontal) == -1)
        {
            Debug.Log("player 2 left");

            if (p2votedB == true)
                Blue -= 1;

            if (p2votedG == true)
                Green -= 1;

            if (p2votedR == true)
                Red -= 1;

            Yellow += 1;
            p2votedY = true;


            Choice_Yellow2 = true;
            Choice_Blue2 = false;
            Choice_Green2 = false;
            Choice_Red2 = false;

            p2votedG = false;
            p2votedR = false;
            p2votedB = false;
        }
        else if (Choice_Red2 == false && UnityStandardAssets.CrossPlatformInput.CrossPlatformInputManager.GetAxisRaw(player2AxisVertical) == 1)
        {
            Debug.Log("player 2 up");

            if (p2votedB == true)
                Blue -= 1;

            if (p2votedG == true)
                Green -= 1;

            if (p2votedY == true)
                Yellow -= 1;

            Red += 1;
            p2votedR = true;

            Choice_Red2 = true;
            Choice_Blue2 = false;
            Choice_Green2 = false;
            Choice_Yellow2 = false;

            p2votedB = false;
            p2votedG = false;
            p2votedY = false;
        }

        else if (Choice_Green2 == false && UnityStandardAssets.CrossPlatformInput.CrossPlatformInputManager.GetAxisRaw(player2AxisVertical) == -1)
        {
            Debug.Log("player 2 down");

            if (p2votedB == true)
                Blue -= 1;

            if (p2votedY == true)
                Yellow -= 1;

            if (p2votedR == true)
                Red -= 1;

            Green += 1;
            p2votedG = true;

            Choice_Green2 = true;
            Choice_Blue2 = false;
            Choice_Yellow2 = false;
            Choice_Red2 = false;

            p2votedY = false;
            p2votedB = false;
            p2votedR = false;
        }

        Vote_Count();

    }
}
