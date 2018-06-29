using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Voting : MonoBehaviour {

    public Text Red_Txt, Blue_Txt, Green_Txt, Yellow_Txt;
    public Text Red_Voted, Blue_Voted, Green_Voted, Yellow_Voted;
    private bool Choice_Red, Choice_Blue, Choice_Green, Choice_Yellow;
    private bool Choice_Red2, Choice_Blue2, Choice_Green2, Choice_Yellow2;
    private bool Choice_Red3, Choice_Blue3, Choice_Green3, Choice_Yellow3;
    private bool Choice_Red4, Choice_Blue4, Choice_Green4, Choice_Yellow4;
    public int Red, Blue, Green, Yellow;
    private int Red_t, Blue_t, Yellow_t, Green_t;
    private int Red_t2, Blue_t2, Yellow_t2, Green_t2;

    public GameObject VotingArrows;

    public GameController controller;

    void Vote_Count()
    {
        Red_Txt.text = "" + Red;
        Blue_Txt.text = "" + Blue;
        Green_Txt.text = "" + Green;
        Yellow_Txt.text = "" + Yellow;

        if(Red >= 3)
        {
            Red_Voted.gameObject.SetActive(true);
            Invoke("DisableText", 3f);
            Red = 0;
            Yellow = 0;
            Blue = 0;
            Green = 0;
            controller.allPlayers[0].GetComponent<Platformer2DUserControl>().SetHealth(999);
            //Set players percentage to 999 here
        }

        else if (Yellow >= 3)
        {
            Yellow_Voted.gameObject.SetActive(true);
            Invoke("DisableText", 3f);
            Red = 0;
            Yellow = 0;
            Blue = 0;
            Green = 0;
            controller.allPlayers[3].GetComponent<Platformer2DUserControl>().SetHealth(999);
            // StartCoroutine(VoteWait());
            // Yellow_Voted.gameObject.SetActive(false);
            //set players percentage to 999 here
        }
        else if (Green >= 3)
        {
            Green_Voted.gameObject.SetActive(true);
            Invoke("DisableText", 3f);
            Red = 0;
            Yellow = 0;
            Blue = 0;
            Green = 0;
            controller.allPlayers[2].GetComponent<Platformer2DUserControl>().SetHealth(999);
            // StartCoroutine(VoteWait());
            // Green_Voted.gameObject.SetActive(false);
            //set players percentage to 999 here
        }
        else if (Blue >= 3)
        {
            Blue_Voted.gameObject.SetActive(true);
            Invoke("DisableText", 3f);
            Red = 0;
            Yellow = 0;
            Blue = 0;
            Green = 0;
            controller.allPlayers[1].GetComponent<Platformer2DUserControl>().SetHealth(999);
            // StartCoroutine(VoteWait());
            // Blue_Voted.gameObject.SetActive(false);
            //set players percentage to 999 here
        }
    }


    public string player1AxisVertical;
    public string player1AxisHorizontal;

    public string player2AxisVertical;
    public string player2AxisHorizontal;

    public string player3AxisVertical;
    public string player3AxisHorizontal;

    public string player4AxisVertical;
    public string player4AxisHorizontal;

    private bool p1votedR, p1votedG, p1votedB, p1votedY;
    private bool p2votedR, p2votedG, p2votedB, p2votedY;
    private bool p3votedR, p3votedG, p3votedB, p3votedY;
    private bool p4votedR, p4votedG, p4votedB, p4votedY;

    bool WaitCompleted, WaitCompleted2, WaitCompleted3, WaitCompleted4;

    private void Start()
    {
        WaitCompleted = true;
    }

    private void Update()
    {
        if(WaitCompleted == true && Choice_Blue == false && UnityStandardAssets.CrossPlatformInput.CrossPlatformInputManager.GetAxisRaw(player1AxisHorizontal) == 1)
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

            StartCoroutine(VoteWait());

        }
        else if(WaitCompleted == true && Choice_Yellow == false && UnityStandardAssets.CrossPlatformInput.CrossPlatformInputManager.GetAxisRaw(player1AxisHorizontal) == -1)
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

            StartCoroutine(VoteWait());
        }
        else if (WaitCompleted == true && Choice_Red == false && UnityStandardAssets.CrossPlatformInput.CrossPlatformInputManager.GetAxisRaw(player1AxisVertical) == 1)
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

            StartCoroutine(VoteWait());
        }
        else if (WaitCompleted == true && Choice_Green == false && UnityStandardAssets.CrossPlatformInput.CrossPlatformInputManager.GetAxisRaw(player1AxisVertical) == -1)
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

            StartCoroutine(VoteWait());
        }

        /////////////////////////////////////////////////////////////////////// Player 2 //////////////////////////////////////////////////////////////////////////////////////////////////////////

        else if (WaitCompleted2 == true && Choice_Blue2 == false && UnityStandardAssets.CrossPlatformInput.CrossPlatformInputManager.GetAxisRaw(player2AxisHorizontal) == 1)
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

            StartCoroutine(VoteWait());
        }
        else if (WaitCompleted2 == true && Choice_Yellow2 == false && UnityStandardAssets.CrossPlatformInput.CrossPlatformInputManager.GetAxisRaw(player2AxisHorizontal) == -1)
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

            StartCoroutine(VoteWait());
        }
        else if (WaitCompleted2 == true && Choice_Red2 == false && UnityStandardAssets.CrossPlatformInput.CrossPlatformInputManager.GetAxisRaw(player2AxisVertical) == 1)
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

            StartCoroutine(VoteWait());
        }

        else if (WaitCompleted2 == true && Choice_Green2 == false && UnityStandardAssets.CrossPlatformInput.CrossPlatformInputManager.GetAxisRaw(player2AxisVertical) == -1)
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

            StartCoroutine(VoteWait());
        }

        /////////////////////////////////////////////////////////////////////// Player 3 //////////////////////////////////////////////////////////////////////////////////////////////////////////

        else if (WaitCompleted3 == true && Choice_Blue3 == false && UnityStandardAssets.CrossPlatformInput.CrossPlatformInputManager.GetAxisRaw(player3AxisHorizontal) == 1)
        {
            Debug.Log("player 3 right");

            if (p3votedR == true)
                Red -= 1;

            if (p3votedG == true)
                Green -= 1;

            if (p3votedY == true)
                Yellow -= 1;

            Blue += 1;
            p3votedB = true;

            Choice_Blue3 = true;
            Choice_Green3 = false;
            Choice_Yellow3 = false;
            Choice_Red3 = false;

            p3votedR = false;
            p3votedY = false;
            p3votedG = false;

            StartCoroutine(VoteWait());
        }
        else if (WaitCompleted3 == true && Choice_Yellow3 == false && UnityStandardAssets.CrossPlatformInput.CrossPlatformInputManager.GetAxisRaw(player3AxisHorizontal) == -1)
        {
            Debug.Log("player 3 left");

            if (p3votedB == true)
                Blue -= 1;

            if (p3votedG == true)
                Green -= 1;

            if (p3votedR == true)
                Red -= 1;

            Yellow += 1;
            p3votedY = true;


            Choice_Yellow3 = true;
            Choice_Blue3 = false;
            Choice_Green3 = false;
            Choice_Red3 = false;

            p3votedG = false;
            p3votedR = false;
            p3votedB = false;

            StartCoroutine(VoteWait());
        }
        else if (WaitCompleted3 == true && Choice_Red3 == false && UnityStandardAssets.CrossPlatformInput.CrossPlatformInputManager.GetAxisRaw(player3AxisVertical) == 1)
        {
            Debug.Log("player 3 up");

            if (p3votedB == true)
                Blue -= 1;

            if (p3votedG == true)
                Green -= 1;

            if (p3votedY == true)
                Yellow -= 1;

            Red += 1;
            p3votedR = true;

            Choice_Red3 = true;
            Choice_Blue3 = false;
            Choice_Green3 = false;
            Choice_Yellow3 = false;

            p3votedB = false;
            p3votedG = false;
            p3votedY = false;

            StartCoroutine(VoteWait());
        }

        else if (WaitCompleted3 == true && Choice_Green3 == false && UnityStandardAssets.CrossPlatformInput.CrossPlatformInputManager.GetAxisRaw(player3AxisVertical) == -1)
        {
            Debug.Log("player 3 down");

            if (p3votedB == true)
                Blue -= 1;

            if (p3votedY == true)
                Yellow -= 1;

            if (p3votedR == true)
                Red -= 1;

            Green += 1;
            p3votedG = true;

            Choice_Green3 = true;
            Choice_Blue3 = false;
            Choice_Yellow3 = false;
            Choice_Red3 = false;

            p3votedY = false;
            p3votedB = false;
            p3votedR = false;

            StartCoroutine(VoteWait());
        }
        /////////////////////////////////////////////////////////////////////// Player 4 //////////////////////////////////////////////////////////////////////////////////////////////////////////

        else if (WaitCompleted4 == true && Choice_Blue4 == false && UnityStandardAssets.CrossPlatformInput.CrossPlatformInputManager.GetAxisRaw(player4AxisHorizontal) == 1)
        {
            Debug.Log("player 4 right");

            if (p4votedR == true)
                Red -= 1;

            if (p4votedG == true)
                Green -= 1;

            if (p4votedY == true)
                Yellow -= 1;

            Blue += 1;
            p4votedB = true;

            Choice_Blue4 = true;
            Choice_Green4 = false;
            Choice_Yellow4 = false;
            Choice_Red4 = false;

            p4votedR = false;
            p4votedY = false;
            p4votedG = false;

            StartCoroutine(VoteWait());
        }
        else if (WaitCompleted4 == true && Choice_Yellow4 == false && UnityStandardAssets.CrossPlatformInput.CrossPlatformInputManager.GetAxisRaw(player4AxisHorizontal) == -1)
        {
            Debug.Log("player 4 left");

            if (p4votedB == true)
                Blue -= 1;

            if (p4votedG == true)
                Green -= 1;

            if (p4votedR == true)
                Red -= 1;

            Yellow += 1;
            p4votedY = true;


            Choice_Yellow4 = true;
            Choice_Blue4 = false;
            Choice_Green4 = false;
            Choice_Red4 = false;

            p4votedG = false;
            p4votedR = false;
            p4votedB = false;

            StartCoroutine(VoteWait());
        }
        else if (WaitCompleted4 == true && Choice_Red4 == false && UnityStandardAssets.CrossPlatformInput.CrossPlatformInputManager.GetAxisRaw(player4AxisVertical) == 1)
        {
            Debug.Log("player 4 up");

            if (p4votedB == true)
                Blue -= 1;

            if (p4votedG == true)
                Green -= 1;

            if (p4votedY == true)
                Yellow -= 1;

            Red += 1;
            p4votedR = true;

            Choice_Red4 = true;
            Choice_Blue4 = false;
            Choice_Green4 = false;
            Choice_Yellow4 = false;

            p4votedB = false;
            p4votedG = false;
            p4votedY = false;

            StartCoroutine(VoteWait());
        }

        else if (WaitCompleted4 == true && Choice_Green4 == false && UnityStandardAssets.CrossPlatformInput.CrossPlatformInputManager.GetAxisRaw(player4AxisVertical) == -1)
        {
            Debug.Log("player 4 down");

            if (p4votedB == true)
                Blue -= 1;

            if (p4votedY == true)
                Yellow -= 1;

            if (p4votedR == true)
                Red -= 1;

            Green += 1;
            p4votedG = true;

            Choice_Green4 = true;
            Choice_Blue4 = false;
            Choice_Yellow4 = false;
            Choice_Red4 = false;

            p4votedY = false;
            p4votedB = false;
            p4votedR = false;

            StartCoroutine(VoteWait());
        }

        if (VotingArrows.gameObject.activeInHierarchy)
        {
            Vote_Count();
        }
        else
        {
            Green = 0;
            Blue = 0;
            Red = 0;
            Yellow = 0;

            Choice_Blue = false;
            Choice_Blue2 = false;
            Choice_Blue3 = false;
            Choice_Blue4 = false;
            Choice_Green = false;
            Choice_Green2 = false;
            Choice_Green3 = false;
            Choice_Green4 = false;
            Choice_Red = false;
            Choice_Red2 = false;
            Choice_Red4 = false;
            Choice_Red3 = false;
            Choice_Yellow = false;
            Choice_Yellow2 = false;
            Choice_Yellow3 = false;
            Choice_Yellow4 = false;
            p1votedB = false;
            p1votedG = false;
            p1votedR = false;
            p1votedY = false;
            p2votedB = false;
            p2votedG = false;
            p2votedR = false;
            p2votedY = false;
            p3votedB = false;
            p3votedG = false;
            p3votedR = false;
            p3votedY = false;
            p4votedB = false;
            p4votedG = false;
            p4votedR = false;
            p4votedY = false;
        }

    }

    IEnumerator VoteWait()
    {
        WaitCompleted = false;
        WaitCompleted2 = false;
        WaitCompleted3 = false;
        WaitCompleted4 = false;
        yield return new WaitForSeconds(2);
        WaitCompleted = true;
        WaitCompleted2 = true;
        WaitCompleted3 = true;
        WaitCompleted4 = true;
    }

    public void DisableText()
    {
        Red_Voted.gameObject.SetActive(false);
        Green_Voted.gameObject.SetActive(false);
        Blue_Voted.gameObject.SetActive(false);
        Yellow_Voted.gameObject.SetActive(false);
        VotingArrows.gameObject.SetActive(false);
    }

    public void EnableArrows()
    {
        VotingArrows.gameObject.SetActive(true);
    }

}
