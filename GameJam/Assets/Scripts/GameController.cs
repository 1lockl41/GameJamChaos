using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityStandardAssets._2D;
using UnityStandardAssets.CrossPlatformInput;

public class GameController : MonoBehaviour {

    public GameObject[] allPlayers;
    public GameObject currentlySelectedPlayer;
    public bool shouldPickNewPlayer;
    float timeBeforePickingNewPlayer = 5.0f;
    float timeBeforePickingNewPlayerTime = 0.0f;

    public static float powerCharge = 0.0f;
    public float powerChargeRate = 2.0f;
    public float powerDrainRate = 4.0f;

    public string bumperPlayer1;
    public string bumperPlayer2;
    public string bumperPlayer3;
    public string bumperPlayer4;

    bool buffActive = false;

    public UI_Voting voter;

    // Use this for initialization
    void Start()
    {
        SelectPlayer();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (shouldPickNewPlayer)
        {
            powerCharge = 0.0f;
            timeBeforePickingNewPlayerTime += Time.deltaTime;
            if(timeBeforePickingNewPlayerTime > timeBeforePickingNewPlayer)
            {
                SelectPlayer();
                voter.EnableArrows();
            }
        }

        if(!shouldPickNewPlayer)
        {
            if (!buffActive)
            {
                powerCharge += Time.deltaTime * powerChargeRate;
                if (powerCharge > 100)
                {
                    powerCharge = 100;
                    currentlySelectedPlayer.GetComponent<Platformer2DUserControl>().StartBuff();
                    buffActive = true;
                    voter.DisableText();
                }

                if(currentlySelectedPlayer == allPlayers[0] && (CrossPlatformInputManager.GetButton(bumperPlayer1)))
                {
                    currentlySelectedPlayer.GetComponent<Platformer2DUserControl>().StartBuff();
                    buffActive = true;
                    voter.DisableText();
                }
                else if (currentlySelectedPlayer == allPlayers[1] && (CrossPlatformInputManager.GetButton(bumperPlayer2)))
                {
                    currentlySelectedPlayer.GetComponent<Platformer2DUserControl>().StartBuff();
                    buffActive = true;
                    voter.DisableText();
                }
                else if (currentlySelectedPlayer == allPlayers[2] && (CrossPlatformInputManager.GetButton(bumperPlayer3)))
                {
                    currentlySelectedPlayer.GetComponent<Platformer2DUserControl>().StartBuff();
                    buffActive = true;
                    voter.DisableText();
                }
                else if (currentlySelectedPlayer == allPlayers[3] && (CrossPlatformInputManager.GetButton(bumperPlayer4)))
                {
                    currentlySelectedPlayer.GetComponent<Platformer2DUserControl>().StartBuff();
                    buffActive = true;
                    voter.DisableText();
                }
            }
            else
            {
                powerCharge -= Time.deltaTime * powerDrainRate;
                if(powerCharge < 0)
                {
                    buffActive = false;
                    currentlySelectedPlayer.GetComponent<Platformer2DUserControl>().LoseBuff();
                    shouldPickNewPlayer = true;
                }
            }

            if(!currentlySelectedPlayer.activeInHierarchy)
            {
                shouldPickNewPlayer = true;
            }
        }
		
	}

    void SelectPlayer()
    {
        GameObject tempSelection = allPlayers[Random.Range(0, allPlayers.Length)];
        if(tempSelection != currentlySelectedPlayer || currentlySelectedPlayer == null)
        {
            currentlySelectedPlayer = tempSelection;
            shouldPickNewPlayer = false;
            timeBeforePickingNewPlayerTime = 0.0f;
            
            //Rumble everyone but selected player here
        }
    }

}
