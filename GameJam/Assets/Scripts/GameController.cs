using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityStandardAssets._2D;
using UnityStandardAssets.CrossPlatformInput;
using XInputDotNetPure;

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

    float rumbleTimer = 0.0f;
    float rumbleDuration = 1.5f;
    bool isRumble = false;

    bool buffCountdownOn = false;
    bool finalSoundPlaying = false;

    public AudioClip chargeSound;
    public AudioClip finishedSound;
    AudioSource source;

    public AudioSource backgroundMusic;
    float backgroundMusicVol;

    public AudioClip loseBuffSound;

    // Use this for initialization
    void Start()
    {
        source = GetComponent<AudioSource>();
        backgroundMusicVol = backgroundMusic.volume;
        SelectPlayer();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(buffCountdownOn)
        {
            foreach(GameObject player in allPlayers)
            {
                player.GetComponent<Platformer2DUserControl>().canAttack = false;
            }

            backgroundMusic.volume = 0.0f;

            Time.timeScale = 0.0f;
            if (!source.isPlaying)
            {
                buffCountdownOn = false;
                Time.timeScale = 1.0f;
                currentlySelectedPlayer.GetComponent<Platformer2DUserControl>().StartBuff();
                buffActive = true;
                source.PlayOneShot(finishedSound);
                finalSoundPlaying = true;

                foreach (GameObject player in allPlayers)
                {
                    player.GetComponent<Platformer2DUserControl>().canAttack = true;
                }
            }
        }

        if(finalSoundPlaying)
        {
            backgroundMusic.volume = 0.0f;
            if (!source.isPlaying)
            {
                finalSoundPlaying = false;
            }
        }
        else if(!buffCountdownOn)
        {
            backgroundMusic.volume = backgroundMusicVol;
        }


        if(isRumble)
        {
            if(allPlayers[0] != currentlySelectedPlayer)
            {
                GamePad.SetVibration((PlayerIndex)0, 15.0f, 15.0f);
            }
            if (allPlayers[1] != currentlySelectedPlayer)
            {
                GamePad.SetVibration((PlayerIndex)1, 15.0f, 15.0f);
            }
            if (allPlayers[2] != currentlySelectedPlayer)
            {
                GamePad.SetVibration((PlayerIndex)2, 15.0f, 15.0f);
            }
            if (allPlayers[3] != currentlySelectedPlayer)
            {
                GamePad.SetVibration((PlayerIndex)3, 15.0f, 15.0f);
            }

            rumbleTimer += Time.deltaTime;
            if(rumbleTimer > rumbleDuration)
            {
                isRumble = false;
                GamePad.SetVibration((PlayerIndex)0, 0.0f, 0.0f);
                GamePad.SetVibration((PlayerIndex)1, 0.0f, 0.0f);
                GamePad.SetVibration((PlayerIndex)2, 0.0f, 0.0f);
                GamePad.SetVibration((PlayerIndex)3, 0.0f, 0.0f);
                rumbleTimer = 0.0f;
            }
        }


        if (shouldPickNewPlayer)
        {
            voter.DisableText();
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
                    source.PlayOneShot(chargeSound);
                    voter.DisableText();
                    buffCountdownOn = true;
                }

                if(currentlySelectedPlayer == allPlayers[0] && (CrossPlatformInputManager.GetButton(bumperPlayer1)))
                {
                    voter.DisableText();
                    source.PlayOneShot(chargeSound);
                    buffCountdownOn = true;
                }
                else if (currentlySelectedPlayer == allPlayers[1] && (CrossPlatformInputManager.GetButton(bumperPlayer2)))
                {
                    voter.DisableText();
                    source.PlayOneShot(chargeSound);
                    buffCountdownOn = true;
                }
                else if (currentlySelectedPlayer == allPlayers[2] && (CrossPlatformInputManager.GetButton(bumperPlayer3)))
                {
                    voter.DisableText();
                    source.PlayOneShot(chargeSound);
                    buffCountdownOn = true;
                }
                else if (currentlySelectedPlayer == allPlayers[3] && (CrossPlatformInputManager.GetButton(bumperPlayer4)))
                {
                    voter.DisableText();
                    source.PlayOneShot(chargeSound);
                    buffCountdownOn = true;
                }
            }
            else
            {
                voter.EnableArrows();
                powerCharge -= Time.deltaTime * powerDrainRate;
                if(powerCharge < 0)
                {
                    source.PlayOneShot(loseBuffSound);
                    buffActive = false;
                    currentlySelectedPlayer.GetComponent<Platformer2DUserControl>().LoseBuff();
                    shouldPickNewPlayer = true;
                }
            }

            if(!currentlySelectedPlayer.activeInHierarchy)
            {
                shouldPickNewPlayer = true;
                source.PlayOneShot(loseBuffSound);
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

            isRumble = true;

        }
    }

}
